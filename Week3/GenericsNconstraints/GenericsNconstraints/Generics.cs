namespace GenericsNconstraints
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public class Repository<T> where T : class, IEntity, new()
    {
        private readonly List<T> _store = new();

        private int _nextId = 1;

        public T Add(T entity)
        {
            entity.Id = _nextId++;
            _store.Add(entity);
            Console.WriteLine($"Added {typeof(T).Name} with id={entity.Id}");
            return entity;
        }

        public T GetById(int id)
        {
            return _store.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<T> GetAll() => _store.AsReadOnly();

        public T CreateAndAdd()
        {
            var entity = new T();
            return Add(entity);
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _store.Remove(entity);
            }
            Console.WriteLine($"Removed {typeof(T).Name} with Id = {id}");
        }
    }

    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default User";
        public string Email { get; set; } = "";
    }

    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default Product";
        public decimal Price { get; set; }
    }
}
