﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalaceClientModelLibrary
{
    public class Order : INotifyPropertyChanged
    {
        public int OrderID
        {
            get { return this.orderID; }
            set
            {
                this.orderID = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderID)));
            }
        }
        private int orderID;
        public DateTime? OrderTime
        {
            get { return this.orderTime; }
            set
            {
                this.orderTime = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderTime)));
            }
        }
        private DateTime? orderTime;
        public DateTime? FinishTime
        {
            get { return this.finishTime; }
            set
            {
                this.finishTime = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FinishTime)));
            }
        }
        private DateTime? finishTime;
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
        public ObservableCollection<OrderItem> Items { get; set; } = new ObservableCollection<OrderItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetDefaults()
        {
            this.OrderID = 0;
            this.OrderTime = null;
            this.FinishTime = null;
            this.Items.Clear();
        }
        public Order CopyFrom(Order order)
        {
            this.OrderID = order.OrderID;
            this.OrderTime = order.OrderTime;
            this.FinishTime = order.FinishTime;

            return this;
        }

        public OrderItem AddItem(Item item)
        {
            var orderItem = this.Items.FirstOrDefault(i => i.Item == item);
            if (orderItem != null)
            {
                orderItem.Amount++;
                return orderItem;
            }
            orderItem = new OrderItem();
            orderItem.OrderID = this.OrderID;
            orderItem.ItemID = item.ItemID;
            orderItem.Amount++;
            orderItem.Item = item;
            orderItem.Price = item.Price;
            this.Items.Add(orderItem);
            return orderItem;
        }

        public void NotifyTotalCost()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCost)));
        }
    }
}