﻿using ClientModelLibrary;
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

// The Blank Page order template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaPalaceCashier.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderView : Page
    {
        OrderViewModel orderViewModel = new OrderViewModel();
        ItemViewModel itemViewModel = new ItemViewModel();
        CategoryViewModel categoryViewModel = new CategoryViewModel();

        private bool destroyed;
        public OrderView()
        {
            this.InitializeComponent();
            Task.Run(async () =>
            {
                await this.categoryViewModel.FetchCategories();
                await this.itemViewModel.FetchItems(this.categoryViewModel.Categories);
                while (!this.destroyed)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await this.orderViewModel.FetchOrders(this.itemViewModel.Items);
                    });
                    Thread.Sleep(5000);
                }
            });
            this.Unloaded += OrderView_Unloaded;
        }

        private void OrderView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.destroyed = true;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic); // To unfocus form controls allowing notify to fire text changed
            await Task.Run(() => Thread.Sleep(20)); // So notify can do its thing before we rely on our bindings

            if (orderViewModel.FormOrder.FinishTime != null || orderViewModel.FormOrder.OrderID == 0)
            {
                return;
            }
            orderViewModel.FormOrder.FinishTime = DateTime.Now;
            await orderViewModel.UpdateOrder(new Order().CopyFrom(orderViewModel.FormOrder)); 
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await orderViewModel.DeleteOrder(new Order().CopyFrom(orderViewModel.FormOrder));
            orderViewModel.FormOrder.SetDefaults();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void OrderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var order = (sender as ListView).SelectedItem as Order;
                orderViewModel.FormOrder.CopyFrom(order);
                orderViewModel.FormOrder.Items.Clear();
                foreach (var item in order.Items)
                {
                    orderViewModel.FormOrder.Items.Add(item);
                }
            }
            else
            {
                orderViewModel.FormOrder.SetDefaults();
            }
        }
    }
}
