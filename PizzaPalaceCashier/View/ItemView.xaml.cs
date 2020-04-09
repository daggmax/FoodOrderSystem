using PizzaPalace.Model;
using PizzaPalace.ViewModel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaPalace.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemView : Page
    {
        ItemViewModel itemViewModel = new ItemViewModel();
        CategoryViewModel categoryViewModel = new CategoryViewModel();
        private bool destroyed;
        public ItemView()
        {
            this.InitializeComponent();
            //Fetches from backend while page is active
            Task.Run(async () => {
                while (!destroyed)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await this.categoryViewModel.FetchCategories();
                        await this.itemViewModel.FetchItems(categoryViewModel.Categories);
                    });
                    Thread.Sleep(2000);
                }
            });
            this.Unloaded += ItemView_Unloaded;
        }
        /// <summary>
        /// Eliminates fetch task on unloaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.destroyed = true;
        }
        /// <summary>
        /// Calls AddItem method if item is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic); // To unfocus form controls allowing notify to fire text changed
            await Task.Run(() => Thread.Sleep(20)); // So notify can do its thing before we rely on our bindings

            if (!itemViewModel.FormItem.IsValid)
            {
                return;
            }

            if (itemViewModel.FormItem.ItemID == 0)
            {
                await itemViewModel.AddItem(new Item().CopyFrom(itemViewModel.FormItem));
                var temp = itemViewModel.FormItem.Category;
                itemViewModel.FormItem.SetDefaults();
                this.itemViewModel.FormItem.Category = temp;
            }
            else
            {
                await itemViewModel.UpdateItem(new Item().CopyFrom(itemViewModel.FormItem));
                var temp = itemViewModel.FormItem.Category;
                itemViewModel.FormItem.SetDefaults();
                this.itemViewModel.FormItem.Category = temp;
            }
        }
        /// <summary>
        /// Deletes item in backend, sets default values on FormItem in frontend.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await itemViewModel.DeleteItem(new Item().CopyFrom(itemViewModel.FormItem));
            var temp = itemViewModel.FormItem.Category;
            itemViewModel.FormItem.SetDefaults();
            this.itemViewModel.FormItem.Category = temp;
            ItemListView.SelectedItem = null;
        }
        /// <summary>
        /// Handles selection and mapping CategoryComboBox to selected item's category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = (sender as ListView).SelectedItem as Item;
                itemViewModel.FormItem.CopyFrom(item);       
                this.CategoryComboBox.SelectedItem = this.categoryViewModel.Categories.FirstOrDefault(c => c.CategoryID == item.CategoryID);
            }
            else
            {
                itemViewModel.FormItem.SetDefaults();
            }
        }
        /// <summary>
        /// Updates FormItem to selected category object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.itemViewModel.FormItem.Category = (e.AddedItems[0] as Category);
            }
            else
            {
                this.itemViewModel.FormItem.Category = null;
            }
        }
    }
}
