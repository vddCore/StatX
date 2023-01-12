namespace OverCR.StatX.Config
{
    public class PendingWriteInfo
    {
        public string Key { get; }
        public string Value { get; }
        public PendingWriteType Type { get; }

        public PendingWriteInfo(string key, string value, PendingWriteType type)
        {
            Key = key;
            Value = value;
            Type = type;
        }
    }
}
