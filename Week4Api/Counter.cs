namespace Week4Api
{
    public interface ITransientCounter { int Id { get; } }
    public interface IScopedCounter { int Id { get; } }
    public interface ISingletonCounter { int Id { get; } }
    public class Counter : ITransientCounter, IScopedCounter, ISingletonCounter
    {
        private static int _instanceCount = 0;
        public int Id { get; } = Interlocked.Increment(ref _instanceCount);
    }
}
