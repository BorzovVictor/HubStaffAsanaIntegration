using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace HI.Api.Services
{
    public interface IJsonStoreService
    {
        void Add<T>(T record);
        void AddRange<T>(IEnumerable<T> newRecords);
        void Save<T>(List<T> records);
    }

    public class JsonStoreService : IJsonStoreService
    {
        private string _jsonFile = "./db.json";

        public void AddRange<T>(IEnumerable<T> newRecords)
        {
            var records = LoadJson<T>();
            records.AddRange(newRecords);
            Save(records);
        }
        
        public void Add<T>(T record)
        {
            var records = LoadJson<T>();
            records.Add(record);
            Save(records);
        }
        
        public void Save<T>(List<T> records)
        {
            var json = JsonConvert.SerializeObject(records);
            File.WriteAllText(_jsonFile, json);
        }

        private List<T> LoadJson<T>()
        {
            try
            {
                using (StreamReader r = new StreamReader(_jsonFile))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                }
            }
            catch (Exception)
            {
                // if file not exists
                return new List<T>();
            }
        }
    }
}