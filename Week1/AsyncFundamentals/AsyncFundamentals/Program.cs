using System.Diagnostics;
Console.WriteLine("-----Week 1: Async Fundamentals------\n\n");

Console.WriteLine("----Demo 1: Thread vs Task vs Async");

void withoutAsync()
{
    Console.WriteLine($"starting execution on thread: {Environment.CurrentManagedThreadId}");
    Thread.Sleep(2000);
    Console.WriteLine($"finished execution on thread: {Environment.CurrentManagedThreadId}");
}

withoutAsync();

async Task withAsync()
{
    Console.WriteLine($"starting async void execution on thread: {Environment.CurrentManagedThreadId}");
    await Task.Delay(2000);
    Console.WriteLine($"finished async void execution on thread: {Environment.CurrentManagedThreadId}");
}

await withAsync();

Console.WriteLine("----Demo 2: State Machine + Configure Await----");

var httpClient = new HttpClient();
async Task<string> FetchDataAsync(string url)
{
    Console.WriteLine($"Starting fetching string async on thread: {Environment.CurrentManagedThreadId}");
    string res = await httpClient.GetStringAsync(url).ConfigureAwait(false);
    Console.WriteLine($"Finished fetching string async on thread: {Environment.CurrentManagedThreadId}");

    return res;
}

string ans1 = await FetchDataAsync("https://jsonplaceholder.typicode.com/posts/1");
Console.WriteLine("Fetching 2nd post now");
string ans2 = await FetchDataAsync("https://jsonplaceholder.typicode.com/posts/2");

// The compiler rewrites FetchDataAsync into a state machine struct.
// At each await, MoveNext() returns, freeing the thread.
// ConfigureAwait(false) tells the runtime not to resume on the
// captured SynchronizationContext — thread ID changes prove this. no SynchronizationContext exists here since console app so the runtime always resumes on any free pool thread even with configure await.
// ConfigureAwait(false) matters in UI apps (WinForms/WPF) or old
// ASP.NET where a SynchronizationContext exists and would otherwise
// force resumption on the original thread — potentially causing
// deadlocks if combined with .Result blocking.

Console.WriteLine("----Demo 3:WhenAll, WhenAny, CancellationToken----");

var st = new Stopwatch();

st.Start();

var waitTask = Task.Delay(5000);
var post1Task = FetchDataAsync("https://jsonplaceholder.typicode.com/posts/1");
var post2Task = FetchDataAsync("https://jsonplaceholder.typicode.com/posts/2");

await Task.WhenAll(waitTask, post1Task, post2Task);
st.Stop();
Console.WriteLine($"Total time elapsed {st.ElapsedMilliseconds}ms");
string res = post1Task.Result;
string res1 = post2Task.Result;

var cts = new CancellationTokenSource();

cts.CancelAfter(200);
var stw = new Stopwatch();
stw.Start();
var task = Task.Delay(5000, cts.Token);

await Task.Delay(300);
stw.Stop();
Console.WriteLine($"Is task cancelled: {task.IsCanceled} and if so, then it took {stw.ElapsedMilliseconds}ms");


Console.WriteLine("----Demo 4: Async void and FireAndForget-----");

async Task BackgroundWorkThatFails()
{
    await Task.Delay(200);

    throw new InvalidOperationException("Background work failed");
}

async Task BackgroundWorkThatSucceeds()
{
    await Task.Delay(200);

    Console.WriteLine("Background work succeeds");
}

Console.WriteLine("Main execution starts");

BackgroundWorkThatFails().FireAndForget(ex => Console.WriteLine($"Handled background error:{ex.Message}"));

BackgroundWorkThatSucceeds().FireAndForget(ex => Console.WriteLine($"This never reaches:{ex.Message}"));

Console.WriteLine("Main execution continues");
await Task.Delay(500);
Console.WriteLine("Main ends");

public static class TaskExtensions
{
    public static async void FireAndForget(this Task tsk, Action<Exception> onError = null)
    {
        try
        {
            await tsk;
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }
    }
}


