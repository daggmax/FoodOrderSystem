using PizzaPalaceClientModelLibrary;
using PizzaPalaceCashier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaPalaceCashier.View
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
            Task.Run(async () => {
                while (!destroyed)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await this.itemViewModel.FetchItems(categoryViewModel.Categories);
                        await this.categoryViewModel.FetchCategories();
                    });
                    Thread.Sleep(2000);
                }
            });
            this.Unloaded += ItemView_Unloaded;
        }

        private void ItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.destroyed = true;
        }

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
                itemViewModel.FormItem.SetDefaults();
            }
            else
            {
                await itemViewModel.UpdateItem(new Item().CopyFrom(itemViewModel.FormItem));
                itemViewModel.FormItem.SetDefaults();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await itemViewModel.DeleteItem(new Item().CopyFrom(itemViewModel.FormItem));
            itemViewModel.FormItem.SetDefaults();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

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

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.itemViewModel.FormItem.CategoryID = (e.AddedItems[0] as Category).CategoryID;
            }
            else
            {
                this.itemViewModel.FormItem.CategoryID = 0;
            }
        }
    }
}
