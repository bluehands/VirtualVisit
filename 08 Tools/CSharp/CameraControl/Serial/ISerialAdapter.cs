namespace Serial
{
    public interface ISerialAdapter
    {
        void Open();
        void Send(string command);
        string Read();
        void Dispose();
    }
}
