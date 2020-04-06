using Newtonsoft.Json;
using ClientModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPalaceCustomer.ViewModel
{
    class ItemViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Items";

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public Item FormItem { get; set; } = new Item();

        public async Task FetchItems()
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
                    this.Items.Add(item);
                }
            }
        }
    }
}
