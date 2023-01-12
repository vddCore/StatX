using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OverCR.StatX.Config
{
    public class Settings
    {
        private string FileName { get; }

        public Dictionary<string, string> Entries { get; } = new Dictionary<string, string>();

        public Settings(string fileName)
        {
            FileName = fileName;

            if (!File.Exists(fileName))
                File.Create(fileName).Dispose();

            using (var sr = new StreamReader(fileName))
            {
                JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
            }
        }

        public void Save()
        {
            using (var sw = new StreamWriter(FileName))
            {
                sw.Write(JsonConvert.SerializeObject(Entries));
            }
        }

        public T GetValue<T>(string key)
        {
            if (!Entries.ContainsKey(key))
                return default(T);

            try
            {
                return (T)Convert.ChangeType(Entries[key], typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public void SetValue<T>(string key, T value)
        {
            if (!Entries.ContainsKey(key))
                Entries.Add(key, value.ToString());
            else
                Entries[key] = value.ToString();
        }
    }
}
