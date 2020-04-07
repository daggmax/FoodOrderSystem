using Newtonsoft.Json;
using PizzaPalace.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PizzaPalace.ViewModel
{
    class ItemViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Items";

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public Item FormItem { get; set; } = new Item();

        public async Task<Item> AddItem(Item item)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(item));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this.httpClient.PostAsync(URL + "/" + ControllerName, httpContent);
            item = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());
            this.Items.Add(item);
            return item;
        }

        public async Task UpdateItem(Item item)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(item));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await this.httpClient.PutAsync(URL + "/" + ControllerName + "/" + item.ItemID, httpContent);

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].ItemID == item.ItemID)
                {
                    this.Items[i] = item;
                    break;
                }
            }
        }

        public async Task FetchItems(ObservableCollection<Category> categories)
        {
            var response = await this.httpClient.GetAsync(URL + "/" + ControllerName);
            var items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(await response.Content.ReadAsStringAsync());
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (items.FirstOrDefault(x => x.ItemID == this.Items[i].ItemID) == null)
                {
                    this.Items.RemoveAt(i--);
                }
            }

            foreach (var item in items)
            {
                if (this.Items.FirstOrDefault(x => x.ItemID == item.ItemID) == null)
                {
                    item.Category = categories.Where(c => c.CategoryID == item.CategoryID).FirstOrDefault();
                    this.Items.Add(item);
                }
            }
        }

        public async Task DeleteItem(Item item)
        {
            await this.httpClient.DeleteAsync(URL + "/" + ControllerName + "/" + item.ItemID);
            this.Items.Remove(this.Items.FirstOrDefault(x => x.ItemID == item.ItemID));
        }
    }
}
