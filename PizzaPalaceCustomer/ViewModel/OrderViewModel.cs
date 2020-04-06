using Newtonsoft.Json;
using ClientModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPalaceCustomer.ViewModel
{
    class OrderViewModel
    {
        public Order FormOrder { get; set; } = new Order();
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Orders";

        public async Task<Order> AddOrder(Order order)
        {
            //POST in Orders
            order.OrderTime = DateTime.Now;
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(order));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this.httpClient.PostAsync(URL + "/" + ControllerName, httpContent);           
            //POST in OrderItems
            order.CopyFrom(JsonConvert.DeserializeObject<Order>(await response.Content.ReadAsStringAsync()));
            foreach (var item in this.FormOrder.Items)
            {
                item.OrderID = order.OrderID;
                httpContent = new StringContent(JsonConvert.SerializeObject(item));
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await this.httpClient.PostAsync(URL + "/OrderItems", httpContent);
            }
            return order;
        }

        public async Task FetchOrder(int id)
        {
            var response = await this.httpClient.GetAsync(URL + "/" + ControllerName + "/" + id);
            this.FormOrder.CopyFrom(JsonConvert.DeserializeObject<Order>(await response.Content.ReadAsStringAsync()));    
        }
    }
}
