namespace WiseQueue.Core.Server
{
    public class ServerConfiguration
    {
        public static ServerConfiguration Default
        {
            get { return new ServerConfiguration(5); }
        }

        public int MaxTaskPerQueue { get; private set; }

        public ServerConfiguration(int maxTaskPerQueue)
        {
            MaxTaskPerQueue = maxTaskPerQueue;
        }      
    }
}
