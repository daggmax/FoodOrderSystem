using PizzaPalace.Model;
using PizzaPalace.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PizzaPalace.View
{
    /// <summary>
    /// Dismissing current ContentDialog if active
    /// </summary>
    internal static class ContentDialogMaker
    {
        public static async void CreateContentDialog(ContentDialog Dialog, bool awaitPreviousDialog) { await CreateDialog(Dialog, awaitPreviousDialog); }
        public static async Task CreateContentDialogAsync(ContentDialog Dialog, bool awaitPreviousDialog) { await CreateDialog(Dialog, awaitPreviousDialog); }

        static async Task CreateDialog(ContentDialog Dialog, bool awaitPreviousDialog)
        {
            if (ActiveDialog != null)
            {
                if (awaitPreviousDialog)
                {
                    await DialogAwaiter.Task;
                    DialogAwaiter = new TaskCompletionSource<bool>();
                }
                else ActiveDialog.Hide();
            }
            ActiveDialog = Dialog;
            ActiveDialog.Closed += ActiveDialog_Closed;
            await ActiveDialog.ShowAsync();
            ActiveDialog.Closed -= ActiveDialog_Closed;
        }

        public static ContentDialog ActiveDialog;
        static TaskCompletionSource<bool> DialogAwaiter = new TaskCompletionSource<bool>();
        private static void ActiveDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args) 
        {
            DialogAwaiter.TrySetResult(true); 
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        ItemViewModel itemViewModel = new ItemViewModel();
        CategoryViewModel categoryViewModel = new CategoryViewModel();
        OrderViewModel orderViewModel = new OrderViewModel();
        private bool destroyed;

        public MainView()
        {
            this.InitializeComponent();  
            //Fetches from backend while page is active
            Task.Run(async () =>
            {
                while (!this.destroyed)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await this.categoryViewModel.FetchCategories();
                        await this.itemViewModel.FetchItems(categoryViewModel.Categories);
                    });
                    if (this.orderViewModel.FormOrder.OrderID > 0)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            await this.orderViewModel.FetchOrder(this.orderViewModel.FormOrder.OrderID);
                            if (this.orderViewModel.FormOrder.FinishTime != null)
                            {
                                this.orderViewModel.FormOrder.SetDefaults();
                                this.orderViewModel.FormOrder.NotifyTotalCost();
                                await this.DisplayOrderReadyDialog();
                            }
                        });
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Thread.Sleep(10000);
                    }
                }
            });
            this.Unloaded += OnUnloaded;
        }
        /// <summary>
        /// Eliminates fetch task on unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.destroyed = true;
        }
        public async Task DisplayOrderReadyDialog()
        {
            await ContentDialogMaker.CreateContentDialogAsync(new ContentDialog
            {
                Title = "Order ready!",
                Content = new TextBlock
                {
                    Text = "Your order is ready for pickup.",
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "OK"
            }, awaitPreviousDialog: false);
        }
        public async Task DisplayOrderPlacedDialog()
        {
            await ContentDialogMaker.CreateContentDialogAsync(new ContentDialog
            {
                Title = "Order placed!",
                Content = new TextBlock
                {
                    Text = "Your order has been placed. A notification will pop up once your order is ready for pick up.",
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "OK"
            }, awaitPreviousDialog: false);
        }
        public async Task DisplayPleaseWaitDialog()
        {
            await ContentDialogMaker.CreateContentDialogAsync(new ContentDialog
            {
                Title = "Please wait!",
                Content = new TextBlock
                {
                    Text = "Your order has already been placed. A notification will pop up once your order is ready for pick up.",
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "OK"
            }, awaitPreviousDialog: false);
        }
        private void ItemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.orderViewModel.FormOrder.OrderID == 0 && ItemGridView.SelectedItem != null)
            {
                this.orderViewModel.FormOrder.AddItem(ItemGridView.SelectedItem as Item);
                this.orderViewModel.FormOrder.NotifyTotalCost();
            }
            ItemGridView.SelectedItem = null;
        }
        private async void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderViewModel.FormOrder.OrderID > 0)
            {
                await DisplayPleaseWaitDialog();
                return;
            }
            var index = this.GetIndexFromListViewByOriginalSource(e.OriginalSource);
            this.orderViewModel.FormOrder.Items[index].Amount++;
            this.orderViewModel.FormOrder.NotifyTotalCost();
            
        }
        private async void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderViewModel.FormOrder.OrderID > 0)
            {
                await DisplayPleaseWaitDialog();
                return;
            }
            var index = this.GetIndexFromListViewByOriginalSource(e.OriginalSource);
            this.orderViewModel.FormOrder.Items[index].Amount--;
            if (this.orderViewModel.FormOrder.Items[index].Amount <= 0)
            {
                this.orderViewModel.FormOrder.Items.RemoveAt(index);
            }
            this.orderViewModel.FormOrder.NotifyTotalCost();
        }
        private async void ClearCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderViewModel.FormOrder.OrderID > 0)
            {
                await DisplayPleaseWaitDialog();
                return;
            }
            this.orderViewModel.FormOrder.Items.Clear();
            this.orderViewModel.FormOrder.NotifyTotalCost();
        }
        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderViewModel.FormOrder.OrderID > 0)
            {
                await DisplayPleaseWaitDialog();
                return;
            }
            if (this.orderViewModel.FormOrder.Items.Count == 0)
            {
                return;
            }
            await this.orderViewModel.AddOrder(this.orderViewModel.FormOrder);
            this.orderViewModel.FormOrder.NotifyTotalCost();
            await DisplayOrderPlacedDialog();
        }
        /// <summary>
        /// Needed for manipulating Amount
        /// </summary>
        /// <param name="originalSource"></param>
        /// <returns></returns>
        public int GetIndexFromListViewByOriginalSource(object originalSource)
        {
            DependencyObject dependencyObject = (DependencyObject)originalSource;
            while ((dependencyObject != null) && !(dependencyObject is ListViewItem))
            {
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            if (dependencyObject == null)
            {
                return 0;
            }
            return CartListView.IndexFromContainer(dependencyObject);
        }
    }
}
