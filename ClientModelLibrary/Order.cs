using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalace.Model
{
    public class Order : INotifyPropertyChanged
    {
        private int orderID;
        public int OrderID
        {
            get { return this.orderID; }
            set
            {
                this.orderID = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderID)));
            }
        }
        private DateTime? orderTime;
        public DateTime? OrderTime
        {
            get { return this.orderTime; }
            set
            {
                this.orderTime = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderTime)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InProgress)));
            }
        }
        private DateTime? finishTime;
        public DateTime? FinishTime
        {
            get { return this.finishTime; }
            set
            {
                this.finishTime = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FinishTime)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InProgress)));
            }
        }
        public float TotalCost
        {
            get
            {
                float total = 0;
                foreach (var item in this.Items)
                {
                    total += item.Price * item.Amount;
                }
                return total;
            }
        }
        public bool InProgress
        {
            get
            {
                return this.FinishTime == null && this.OrderTime != null;
            }
        }
        public ObservableCollection<OrderItem> Items { get; set; } = new ObservableCollection<OrderItem>();

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Set default values to this object.
        /// </summary>
        public void SetDefaults()
        {
            this.OrderID = 0;
            this.OrderTime = null;
            this.FinishTime = null;
            this.Items.Clear();
        }
        /// <summary>
        /// Updates this object to match parameter.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order CopyFrom(Order order)
        {
            this.OrderID = order.OrderID;
            this.OrderTime = order.OrderTime;
            this.FinishTime = order.FinishTime;
            if (order.Items.Count > 0)
            {
                this.Items.Clear();
                foreach (var item in order.Items)
                {
                    this.Items.Add(item);
                }
            }
            return this;
        }
        /// <summary>
        /// Adds OrderItem to this list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public OrderItem AddItem(Item item)
        {
            var orderItem = this.Items.FirstOrDefault(i => i.Item == item);
            if (orderItem != null)
            {
                orderItem.Amount++;
                return orderItem;
            }
            orderItem = new OrderItem
            {
                OrderID = this.OrderID,
                ItemID = item.ItemID,
                Item = item,
                Price = item.Price
            };
            orderItem.Amount++;
            this.Items.Add(orderItem);
            return orderItem;
        }
        /// <summary>
        /// Compares all fields between this and parameter.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool FieldEquals(Order order)
        {
            if (this.OrderID != order.OrderID)
            {
                return false;
            }
            if (this.OrderTime != order.OrderTime)
            {
                return false;
            }
            if (this.FinishTime != order.FinishTime)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Used for notifing GUI to update property.
        /// </summary>
        public void NotifyTotalCost()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCost)));
        }
    }
}
