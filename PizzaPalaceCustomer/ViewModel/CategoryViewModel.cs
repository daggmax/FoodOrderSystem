using Newtonsoft.Json;
using PizzaPalaceClientModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace PizzaPalaceCustomer.ViewModel
{
    class CategoryViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Categories";

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public Category FormCategory { get; set; } = new Category();
        
        public async Task FetchCategories()
        {
            var response = await this.httpClient.GetAsync(URL + "/" + ControllerName);
            var categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(await response.Content.ReadAsStringAsync());
            for (int i = 0; i < this.Categories.Count; i++)
            {
                if (categories.FirstOrDefault(c => c.CategoryID == this.Categories[i].CategoryID) == null)
                {
                    this.Categories.RemoveAt(i--);
                }
            }
            
            foreach (var category in categories)
            {
                if (this.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID) == null)
                {
                    this.Categories.Add(category);
                }
            }
        }
    }
}
