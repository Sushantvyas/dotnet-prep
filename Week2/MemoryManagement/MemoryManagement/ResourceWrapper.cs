namespace MemoryManagement
{
    public class ResourceWrapper : IDisposable
    {
        private bool _disposed;
        private readonly string _name;
        private FileStream _fileStream;

        public ResourceWrapper(string name)
        {
            _name = name;
            var tempFile = Path.GetTempFileName();
            _fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite);
            Console.WriteLine($"{_name} resource acquired");
        }

        public void DoWork()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(_name);
            }
            Console.WriteLine($"{_name} doing work");
        }

        ~ResourceWrapper()
        {
            Console.WriteLine($"{_name} Finalizer called( Dispose was not called properly)");
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            Console.WriteLine($"{_name} Disposed properly - finalizer suppressed");
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _fileStream?.Dispose();
                Console.WriteLine($"{_name} managed resources released");
            }
            _disposed = true;
        }
    }
}
