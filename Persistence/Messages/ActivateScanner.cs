namespace Messages
{
    public class ActivateScanner
    {
        public string ScannerId { get; private set; }

        public ActivateScanner(string scannerId)
        {
            ScannerId = scannerId;
        }
    }
}
