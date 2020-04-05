using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalaceCashier.Model
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ItemID
        {
            get { return this.itemID; }
            set
            {
                this.itemID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exists)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }
        private int itemID;
        public int CategoryID
        {
            get { return this.categoryID; }
            set
            {
                this.categoryID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }
        private int categoryID;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }
        private string name;
        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }
        private string description;
        public float Price
        {
            get { return this.price; }
            set
            {
                this.price = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }
        private float price;
        public string ImageURL 
        { 
            get { return imageURL; } 
            set
            {
                this.imageURL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageURL)));
            }
        }
        private string imageURL;
        public bool Exists { get { return this.ItemID > 0; } }
        public bool IsValid
        {
            get
            {
                if (!(this.ItemID >= 0))
                {
                    return false;
                }
                if (!(this.Name.Length > 0))
                {
                    return false;
                }
                if (!(this.CategoryID > 0))
                {
                    return false;
                }
                if (!(this.Price > 0))
                {
                    return false;
                }
                return true;
            }
        }

        public void SetDefaults()
        {
            this.ItemID = 0;
            this.CategoryID = 0;
            this.Name = "";
            this.Description = "";
            this.Price = 0;
            this.ImageURL = "";
        }
        public Item CopyFrom(Item item)
        {
            this.ItemID = item.ItemID;
            this.CategoryID = item.CategoryID;
            this.Name = item.Name;
            this.Description = item.Description;
            this.Price = item.Price;
            this.ImageURL = item.ImageURL;
            return this;
        }
    }
}
