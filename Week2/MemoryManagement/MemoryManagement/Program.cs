using MemoryManagement;
using System.Collections;
using System.Diagnostics;
Console.WriteLine("---Demo 1: Value Type vs Reference Type----");

Console.WriteLine("Value type independent behaviour");
int a1 = 5;
int b1 = a1;
b1 = 10;

Console.WriteLine($"Value of a: {a1}, Value of b: {b1}");

Console.WriteLine("Reference type shared behaviour");

var List1 = new List<int> { 1, 2, 3, 4 };
var List2 = List1;

List2[3] = 5;

Console.WriteLine($"4th index value in List1 is {List1[3]} and in List2 is {List2[3]}");

Console.WriteLine("\n\nStruct vs Class copy behaviour");

var ps1 = new pointStruct { X = 1, Y = 2 };
var ps2 = ps1;

ps2.Y = 3;

Console.WriteLine($"pointstruct ps1 Y coordinate value is {ps1.Y}, and ps2 Y coordinate value is {ps2.Y}");

var pc1 = new pointClass { X = 1, Y = 2 };
var pc2 = pc1;

pc2.Y = 3;

Console.WriteLine($"pointclass pc1 Y coordinate value is {pc1.Y}, and pc2 Y coordinate value is {pc2.Y}");
Console.WriteLine("\nBoxing Performance impact");


const int iterations = 10000000;

var aL = new ArrayList();

var st = new Stopwatch();
st.Start();
for (int i = 0; i < iterations; i++)
{
    aL.Add(i);
}
st.Stop();
Console.WriteLine($"Total execution time for arrayList is {st.ElapsedMilliseconds}ms");

var list = new List<int>();
st.Restart();
for (int i = 0; i < iterations; i++)
{
    list.Add(i);
}
st.Stop();
Console.WriteLine($"Total execution time for generic int List is {st.ElapsedMilliseconds}ms");

Console.WriteLine("---Boxing Trap---");

int a = 5;
object b = a;

int unboxed = (int)b;

Console.WriteLine($"Successfully unboxed to: {unboxed}");

try
{
    long longUnboxed = (long)b;
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"Invalid cast exception: {ex.Message}");
}

if (b is int value)
    Console.WriteLine("Safe type casting applied successfully");

Console.WriteLine("---GC Generation observation---");
Console.WriteLine("Initial GC collection count");
Console.WriteLine($"Gen 0 count: {GC.CollectionCount(0)}");
Console.WriteLine($"Gen 1 count: {GC.CollectionCount(1)}");
Console.WriteLine($"Gen 2 count: {GC.CollectionCount(2)}");

var lst = new List<int>();
for (int i = 0; i < 1000000; i++)
{
    var temp = new byte[100];
}

Console.WriteLine("After short-lived allocations");
Console.WriteLine($"Gen 0 count: {GC.CollectionCount(0)}");
Console.WriteLine($"Gen 1 count: {GC.CollectionCount(1)}");
Console.WriteLine($"Gen 2 count: {GC.CollectionCount(2)}");

//Create a long lived object and force it to Gen 2
var longLived = new byte[1000];
Console.WriteLine($"\nNew Object generation: {GC.GetGeneration(longLived)}");
GC.Collect(0);
Console.WriteLine($"After Gen 0 collect: {GC.GetGeneration(longLived)}");
GC.Collect(1);
Console.WriteLine($"After Gen 1 collect: {GC.GetGeneration(longLived)}");
GC.Collect(2);
Console.WriteLine($"After Gen 2 collect: {GC.GetGeneration(longLived)}");

Console.WriteLine("Large Object Heap: (LOH) in action");

var smallObj = new byte[100];

Console.WriteLine($"Small object generation: {GC.GetGeneration(smallObj)}");

var largeObject = new byte[1000000];

Console.WriteLine($"Large object heap generation: {GC.GetGeneration(largeObject)}");

//Demonstrate ArrayPool to avoid LOH pressure:
Console.WriteLine("\n=== ArrayPool vs new allocation ===");
var sw = System.Diagnostics.Stopwatch.StartNew();

// Without pool — allocates large array each time:
for (int i = 0; i < 10000; i++)
{
    var buffer = new byte[100000];
    buffer[0] = 1;
}
sw.Stop();
Console.WriteLine($"Without ArrayPool: {sw.ElapsedMilliseconds}ms");

sw.Restart();
// With pool — rents and returns the same buffer:
for (int i = 0; i < 10_000; i++)
{
    var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(100000);
    try
    {
        buffer[0] = 1;
    }
    finally
    {
        System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
    }
    //var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(100_000);
    //try { buffer[0] = 1; } // use it
    //finally { System.Buffers.ArrayPool<byte>.Shared.Return(buffer); }
}
sw.Stop();
Console.WriteLine($"With ArrayPool:    {sw.ElapsedMilliseconds}ms");

Console.WriteLine("\n=== Memory pressure observation ===\n");

long before = GC.GetTotalMemory(forceFullCollection: false);
Console.WriteLine($"Memory before: {before / 1024}KB");

// Create pressure:
var references = new object[100_000];
for (int i = 0; i < 100_000; i++)
    references[i] = new byte[100]; // keep references so they survive GC

long after = GC.GetTotalMemory(forceFullCollection: false);
Console.WriteLine($"Memory after:  {after / 1024}KB");
Console.WriteLine($"Difference:    {(after - before) / 1024}KB");

// Release references and observe collection:
for (int i = 0; i < 100_000; i++)
    references[i] = null;

long afterRelease = GC.GetTotalMemory(forceFullCollection: true);
Console.WriteLine($"After release: {afterRelease / 1024}KB");
Console.WriteLine($"Reclaimed:     {(after - afterRelease) / 1024}KB");



Console.WriteLine("=== Dispose via using ===\n");

// Normal disposal:
using (var r1 = new ResourceWrapper("R1"))
{
    r1.DoWork();
}  // Dispose called here

Console.WriteLine();

// Disposal even when exception thrown:
Console.WriteLine("=== Dispose on exception ===\n");
try
{
    using (var r2 = new ResourceWrapper("R2"))
    {
        r2.DoWork();
        throw new InvalidOperationException("Something went wrong");
    }  // Dispose STILL called here despite exception
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught exception: {ex.Message}");
}

Console.WriteLine();

// What happens if Dispose is NOT called — finalizer runs:
Console.WriteLine("=== No Dispose — finalizer runs ===\n");

var r3 = new ResourceWrapper("R3");
r3.DoWork();
// no Dispose — r3 goes out of scope


// Force GC to demonstrate finalizer:
GC.Collect();
GC.WaitForPendingFinalizers();
Console.WriteLine("GC collected — check if finalizer message appeared above");

Console.WriteLine();

// Using after Dispose — should throw ObjectDisposedException:
Console.WriteLine("=== Using after Dispose ===\n");
var r4 = new ResourceWrapper("R4");
r4.Dispose();
try
{
    r4.DoWork();  // should throw
}
catch (ObjectDisposedException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}


Console.WriteLine("\n ----Demo - Span<T> vs substring allocation----");
const string csv = "Sushant,29,Dublin,Software Engineer";
const int iters = 1000000;
var sw1 = new Stopwatch();
sw1.Start();
for (int i = 0; i < iters; i++)
{
    string[] parts = csv.Split(",");
    _ = parts[0];
    _ = parts[1];
    _ = parts[2];
}
sw1.Stop();
Console.WriteLine($"\nString Allocation total time elapsed: {sw1.ElapsedMilliseconds}");

sw1.Restart();

for (int i = 0; i < iters; i++)
{
    ReadOnlySpan<char> span = csv.AsSpan();
    int c1 = span.IndexOf(",");
    int c2 = span.Slice(c1 + 1).IndexOf(",") + c1 + 1;
    _ = span.Slice(0, c1);
    _ = span.Slice(c1 + 1, c2 - c1 - 1);
}
sw1.Stop();

Console.WriteLine($"\nSpan total time elapsed: {sw1.ElapsedMilliseconds}");

Console.WriteLine("---Span modifying original array---");

int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
Console.WriteLine($"\nBefore span modification numbers: {string.Join(",", numbers)}");

Span<int> middle = numbers.AsSpan(2, 6);

for (int i = 0; i < middle.Length; i++)
{
    middle[i] *= 10;
}

Console.WriteLine($"\nAfter span modification numbers: {string.Join(",", numbers)}");


Console.WriteLine("\nMemory<T> across async boundary");

async Task ProcessWithMemoryAsync(Memory<byte> buffer)
{
    Console.WriteLine($"Buffer[0] before async task delay: {buffer.Span[0]}");
    await Task.Delay(200);
    Console.WriteLine($"Buffer[0] after async task delay: {buffer.Span[0]}");

    var span = buffer.Span;
    for (int i = 0; i < span.Length; i++)
    {
        span[i] = (byte)(i * 2);
    }
    Console.WriteLine($"After processing buffer[0]: {buffer.Span[0]}, buffer[1]: {buffer.Span[1]}");
}

byte[] data = new byte[100];
data[0] = 99;
await ProcessWithMemoryAsync(data.AsMemory());

Console.WriteLine($"\nOriginal array after async processing: [{string.Join(",", data)}]");

struct pointStruct
{
    public int X;
    public int Y;
}

class pointClass
{
    public int X;
    public int Y;
}