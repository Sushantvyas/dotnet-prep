namespace GenericsNconstraints
{
    public class PriceChangedEventArgs : EventArgs
    {
        public string ProductName { get; set; }
        public decimal OldPrice { get; set; }
        public decimal newPrice { get; set; }
    }

    public class PriceMonitor
    {
        private Dictionary<string, decimal> _prices = new();
        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        public void SetPrice(string Product, decimal price)
        {
            if (_prices.TryGetValue(Product, out decimal oldPrice) && oldPrice != price)
            {
                _prices[Product] = price;
                PriceChanged?.Invoke(this, new PriceChangedEventArgs
                {
                    ProductName = Product,
                    OldPrice = oldPrice,
                    newPrice = price
                });
            }
            else
            {
                _prices[Product] = price;
                Console.WriteLine($"Initial Price Set: {Product} = {price}");
            }
        }
    }
}
