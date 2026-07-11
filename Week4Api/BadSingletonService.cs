namespace Week4Api
{
    public interface IBadSingletonService { int GetScopedId(); }
    public class BadSingletonService : IBadSingletonService
    {
        private readonly IScopedCounter _scoped;
        public BadSingletonService(IScopedCounter scoped)
        {
            _scoped = scoped;
        }

        public int GetScopedId()
        {
            return _scoped.Id;
        }
    }
}
