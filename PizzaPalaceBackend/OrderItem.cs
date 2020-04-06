using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalaceClientModelLibrary
{
    public class OrderItem : INotifyPropertyChanged
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
        public int ItemID
        {
            get { return this.itemID; }
            set 
            { 
                this.itemID = value; 
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderID))); 
            }
        }
        private int itemID;
        public Item Item
        {
            get { return this.item; }
            set
            {
                this.item = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item)));
            }
        }
        private Item item;
        public float Price
        {
            get { return this.price; }
            set
            {
                this.price = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
            }
        }
        private float price;
        public int Amount
        {
            get { return this.amount; }
            set
            {
                this.amount = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
            }
        }
        private int amount;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
