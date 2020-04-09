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
    class OrderViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Orders";

        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public Order FormOrder { get; set; } = new Order();
        /// <summary>
        /// Adds an order in backend and frontend.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task UpdateOrder(Order order)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(order));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await this.httpClient.PutAsync(URL + "/" + ControllerName + "/" + order.OrderID, httpContent);

            for (int i = 0; i < this.Orders.Count; i++)
            {
                if (this.Orders[i].OrderID == order.OrderID)
                {
                    this.Orders[i] = order;
                    break;
                }
            }
        }
        /// <summary>
        /// Gets orders from backend. Updates list in frontend if changes occured in backend.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task FetchOrders(ObservableCollection<Item> items)
        {
            var response = await this.httpClient.GetAsync(URL + "/" + ControllerName);
            var orders = JsonConvert.DeserializeObject<ObservableCollection<Order>>(await response.Content.ReadAsStringAsync());
            for (int i = 0; i < this.Orders.Count; i++)
            {
                if (orders.FirstOrDefault(x => x.OrderID == this.Orders[i].OrderID) == null)
                {
                    this.Orders.RemoveAt(i--);
                }
            }

            foreach (var order in orders)
            {
                if (this.Orders.FirstOrDefault(x => x.OrderID == order.OrderID) == null)
                {
                    response = await this.httpClient.GetAsync(URL + "/OrderItems" + "/" + order.OrderID);
                    order.Items = JsonConvert.DeserializeObject<ObservableCollection<OrderItem>>(await response.Content.ReadAsStringAsync());
                    foreach (var item in order.Items)
                    {
                        item.Item = items.Where(i => i.ItemID == item.ItemID).FirstOrDefault();
                    }
                    this.Orders.Add(order);
                }
                else
                {
                    int index = this.Orders.IndexOf(this.Orders.FirstOrDefault(i => i.OrderID == order.OrderID));
                    if (!this.Orders[index].FieldEquals(order))
                    {
                        response = await this.httpClient.GetAsync(URL + "/OrderItems" + "/" + order.OrderID);
                        order.Items = JsonConvert.DeserializeObject<ObservableCollection<OrderItem>>(await response.Content.ReadAsStringAsync());
                        foreach (var item in order.Items)
                        {
                            item.Item = items.Where(i => i.ItemID == item.ItemID).FirstOrDefault();
                        }
                        this.Orders[index] = order;
                    }
                }
            }
        }
        /// <summary>
        /// Deletes order in backend and frontend.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task DeleteOrder(Order order)
        {
            await this.httpClient.DeleteAsync(URL + "/" + ControllerName + "/" + order.OrderID);
            this.Orders.Remove(this.Orders.FirstOrDefault(x => x.OrderID == order.OrderID));
        }
    }
}
