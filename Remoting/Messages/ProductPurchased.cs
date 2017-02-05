namespace Messages
{
    public class ProductPurchased
    {
        public int ProductId { get; private set; }
        public int AmountPurchased { get; private set; }
        public int AmountBackorder { get; private set; }

        public ProductPurchased(int productId, int amountPurchased, int amountBackorder)
        {
            ProductId = productId;
            AmountPurchased = amountPurchased;
            AmountBackorder = amountBackorder;
        }
    }
}
