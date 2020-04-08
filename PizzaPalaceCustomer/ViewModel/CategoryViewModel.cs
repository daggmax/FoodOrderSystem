using Newtonsoft.Json;
using PizzaPalace.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PizzaPalace.ViewModel
{
    class CategoryViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Categories";

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        
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
