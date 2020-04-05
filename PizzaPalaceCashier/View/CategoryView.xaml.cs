using PizzaPalaceCashier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Popups;
using PizzaPalaceCashier.Model;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaPalaceCashier.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryView : Page
    {
        CategoryViewModel categoryViewModel = new CategoryViewModel();
        private bool destroyed;
        public CategoryView()
        {
            this.InitializeComponent();
            Task.Run(async () => {
                while (!destroyed)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await categoryViewModel.FetchCategories();
                    });
                    Thread.Sleep(2000);
                }
            });
            this.Unloaded += CategoryView_Unloaded;
        }

        private void CategoryView_Unloaded(object sender, RoutedEventArgs e)
        {
            destroyed = true;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic); // To unfocus form controls allowing notify to fire text changed
            await Task.Run(() => Thread.Sleep(20)); // So notify can do its thing before we rely on our bindings

            if (!categoryViewModel.FormCategory.IsValid)
            {
                return;
            }

            if (categoryViewModel.FormCategory.CategoryID == 0)
            {
                await categoryViewModel.AddCategory(new Category().CopyFrom(categoryViewModel.FormCategory));
                categoryViewModel.FormCategory.SetDefaults();
            }
            else
            {
                await categoryViewModel.UpdateCategory(new Category().CopyFrom(categoryViewModel.FormCategory));
                categoryViewModel.FormCategory.SetDefaults();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await categoryViewModel.DeleteCategory(new Category().CopyFrom(categoryViewModel.FormCategory));
            categoryViewModel.FormCategory.SetDefaults();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                var category = (sender as ListView).SelectedItem as Category;
                categoryViewModel.FormCategory.CopyFrom(category);
            }
            else
            {
                categoryViewModel.FormCategory.SetDefaults();
            }
        }
    }
}
