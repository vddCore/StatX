using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace OverCR.StatX.Config
{
    public class Settings
    {
        private bool IsWritingSettings { get; set; }
        private Queue<PendingWriteInfo> PendingUpdates { get; }

        private string FileName { get; }

        public Dictionary<string, object> Entries { get; }

        public Settings(string fileName)
        {
            FileName = fileName;

            if (!File.Exists(fileName))
                File.Create(fileName).Dispose();

            using (var sr = new StreamReader(fileName))
            {
                Entries = JsonConvert.DeserializeObject<Dictionary<string, object>>(sr.ReadToEnd());
            }

            if (Entries == null)
                Entries = new Dictionary<string, object>();

            PendingUpdates = new Queue<PendingWriteInfo>();
        }

        public void Save()
        {
            IsWritingSettings = true;

            using (var sw = new StreamWriter(FileName))
            {
                sw.Write(JsonConvert.SerializeObject(Entries));
            }

            IsWritingSettings = false;

            WritePendingValues();
        }

        public T GetValue<T>(string key)
        {
            if (!Entries.ContainsKey(key))
                return default(T);

            try
            {
                var typeConverter = TypeDescriptor.GetConverter(typeof(T));
                return (T)typeConverter.ConvertFrom(Entries[key]);
            }
            catch
            {
                return default(T);
            }
        }

        public void SetValue<T>(string key, T value)
        {
            if (!IsWritingSettings)
            {
                if (!Entries.ContainsKey(key))
                    Entries.Add(key, value.ToString());
                else
                    Entries[key] = value.ToString();
            }
            else
            {
                if (!Entries.ContainsKey(key))
                    PendingUpdates.Enqueue(new PendingWriteInfo(key, value.ToString(), PendingWriteType.Add));
                else
                    PendingUpdates.Enqueue(new PendingWriteInfo(key, value.ToString(), PendingWriteType.Modify));
            }
        }

        private void WritePendingValues()
        {
            while (PendingUpdates.Count > 0)
            {
                var info = PendingUpdates.Dequeue();

                if (info.Type == PendingWriteType.Add)
                    Entries.Add(info.Key, info.Value);
                else
                    Entries[info.Key] = info.Value;
            }
        }
    }
}
