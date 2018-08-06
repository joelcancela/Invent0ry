using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invent0ry.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Invent0ry.Persistence
{
    class InventoryManager
    {
        private static readonly string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SkyNetLabs\\invent0ry";
        private static readonly string Path = DataPath + "\\inventory.json";

        public static void SerializeInventory(Inventory inventory)
        {
            JsonSerializer jsonSerializer =
                new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            //Sets JSON keys to lowercase
            JObject jObjectRoot = new JObject();
            JArray jArrayItems = JArray.FromObject(inventory.Items, jsonSerializer);
            jObjectRoot.Add("inventory", jArrayItems);
            System.IO.File.WriteAllText(Path, JsonConvert.SerializeObject(jObjectRoot));
        }

        public static Inventory DeserializeInventory()
        {
            if (!File.Exists(Path))
            {
                Directory.CreateDirectory(DataPath);
                File.Create(Path).Dispose();
                JObject jRootObject = new JObject {{"inventory", new JArray()}};
                System.IO.File.WriteAllText(Path, JsonConvert.SerializeObject(jRootObject));
                return new Inventory();
            }

            string inventoryJsonContent = System.IO.File.ReadAllText(Path);
            JObject inventoryJObject = JObject.Parse(inventoryJsonContent);
            IList<JToken> itemsJTokenList = inventoryJObject["inventory"].Children().ToList();
            ObservableCollection<Item> items = new ObservableCollection<Item>();
            foreach (JToken result in itemsJTokenList)
            {
                Item searchResult = result.ToObject<Item>();
                items.Add(searchResult);
            }

            Inventory inventory = new Inventory {Items = items};
            return inventory;
        }

        public static List<String> GetMemosPaths()
        {
            var filters = new String[] {"jpg", "jpeg", "png", "gif", "tiff", "bmp"};
            List<String> filesFound = new List<String>();
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(DataPath, $"*.{filter}", SearchOption.TopDirectoryOnly));
            }

            return filesFound;
        }
    }
}