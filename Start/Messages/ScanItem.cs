namespace Messages
{
    public class ScanItem
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }

        public ScanItem(int productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }
    }
}
