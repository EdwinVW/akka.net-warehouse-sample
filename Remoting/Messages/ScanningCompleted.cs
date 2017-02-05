namespace Messages
{
    public class ScanningCompleted
    {
        public int ScannerId { get; private set; }

        public ScanningCompleted(int scannerId)
        {
            ScannerId = scannerId;
        }
    }
}
