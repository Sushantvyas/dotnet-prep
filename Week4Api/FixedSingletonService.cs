namespace Week4Api
{
    public class FixedSingletonService : IBadSingletonService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public FixedSingletonService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public int GetScopedId()
        {
            using var scope = _scopeFactory.CreateScope();
            var counter = scope.ServiceProvider.GetRequiredService<IScopedCounter>();
            return counter.Id;
        }
    }
}
