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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaPalaceCashier.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryView : Page
    {
        CategoryViewModel categoryViewModel = new CategoryViewModel();
        public CategoryView()
        {

            this.InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic); // To unfocus form controls allowing notify to fire text changed
            await Task.Run(() => Thread.Sleep(20)); // So notify can do its thing before we rely on our bindings

            if (!categoryViewModel.FormCategory.Validate())
            {
                return;
            }

            if (categoryViewModel.FormCategory.CategoryID == 0)
            {
                await categoryViewModel.AddCategory(categoryViewModel.FormCategory);
                categoryViewModel.FormCategory.SetDefaults();
            }
            else
            {
                await categoryViewModel.UpdateCategory(categoryViewModel.FormCategory);
                categoryViewModel.FormCategory.SetDefaults();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
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
