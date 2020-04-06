﻿using Newtonsoft.Json;
using ClientModelLibrary;
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

namespace PizzaPalaceCashier.ViewModel
{
    class CategoryViewModel
    {
        HttpClient httpClient = new HttpClient();
        private const string URL = "https://localhost:5001/api";
        private const string ControllerName = "Categories";

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public Category FormCategory { get; set; } = new Category();

        public async Task<Category> AddCategory(Category category)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(category));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this.httpClient.PostAsync(URL + "/" + ControllerName, httpContent);
            category = JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            this.Categories.Add(category);
            return category;
        }

        public async Task UpdateCategory(Category category)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(category));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await this.httpClient.PutAsync(URL + "/" + ControllerName + "/" + category.CategoryID, httpContent);

            for (int i = 0; i < this.Categories.Count; i++)
            {
                if (this.Categories[i].CategoryID == category.CategoryID)
                {
                    this.Categories[i] = category;
                    break;
                }
            }
        }

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

        public async Task DeleteCategory(Category category)
        {
            await this.httpClient.DeleteAsync(URL + "/" + ControllerName + "/" + category.CategoryID);
            this.Categories.Remove(this.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID));
        }
    }
}
