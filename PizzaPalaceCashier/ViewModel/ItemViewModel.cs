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
        /// <summary>
        /// Adds an item in backend and frontend.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Item> AddItem(Item item)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(item));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this.httpClient.PostAsync(URL + "/" + ControllerName, httpContent);
            item = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());
            this.Items.Add(item);
            return item;
        }
        /// <summary>
        /// Updates item in backend and frontend.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets items from backend. Updates list in frontend if changes occured in backend.
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
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
                item.Category = categories.Where(c => c.CategoryID == item.CategoryID).FirstOrDefault();
                if (this.Items.FirstOrDefault(i => i.ItemID == item.ItemID) == null)
                {
                    this.Items.Add(item);
                }
                else
                {
                    int index = this.Items.IndexOf(this.Items.FirstOrDefault(i => i.ItemID == item.ItemID));
                    if (!this.Items[index].FieldEquals(item))
                    {
                        this.Items[index] = item;
                    }
                }
            }
        }
        /// <summary>
        /// Deletes item in backend and frontend.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task DeleteItem(Item item)
        {
            await this.httpClient.DeleteAsync(URL + "/" + ControllerName + "/" + item.ItemID);
            this.Items.Remove(this.Items.FirstOrDefault(x => x.ItemID == item.ItemID));
        }
    }
}
