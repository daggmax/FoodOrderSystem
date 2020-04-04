using Newtonsoft.Json;
using PizzaPalaceCashier.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PizzaPalaceCashier.ViewModel
{
    class CategoryViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:44360/api";
        private const string ControllerName = "Categories";

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public Category FormCategory { get; set; } = new Category();

        public async Task<Category> AddCategory(Category category)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(category));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(URL + "/" + ControllerName, httpContent);
            category = JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            Categories.Add(category);
            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(category));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync(URL + "/" + ControllerName + "/" + category.CategoryID, httpContent);
            category = JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            //
            // Uppdatera den redan befintliga categoryn med CopyFrom !
            //
            return category;
        }
    }
}
