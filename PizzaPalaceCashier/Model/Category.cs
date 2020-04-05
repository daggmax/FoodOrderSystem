using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PizzaPalaceCashier.Model
{
    public class Category : INotifyPropertyChanged
    {
        private int categoryID = 0;
        public int CategoryID
        {
            get { return this.categoryID; }
            set {
                this.categoryID = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exists)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }

        private string name = "";
        public string Name
        {
            get { return this.name; }
            set {
                this.name = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid))); 
            }
        }

        public void SetDefaults()
        {
            this.CategoryID = 0;
            this.Name = "";
        }

        public Category CopyFrom(Category category)
        {
            this.CategoryID = category.CategoryID;
            this.Name = category.Name;
            return this;
        }
        public override string ToString()
        {
            return this.Name;
        }
        public bool IsValid
        {
            get
            {
                if (!(this.CategoryID >= 0))
                {
                    return false;
                }
                if (!(this.Name.Length > 0))
                {
                    return false;
                }
                return true;
            }
        }
        public bool Exists { get { return this.CategoryID > 0; } }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
