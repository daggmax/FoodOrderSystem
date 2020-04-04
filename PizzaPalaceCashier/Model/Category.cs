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
            get { return categoryID; } 
            set { categoryID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryID))); }
        }

        private string name = "";
        public string Name 
        { 
            get { return name; } 
            set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); } 
        }

        public void SetDefaults()
        {
            this.CategoryID = 0;
            this.Name = "";
        }

        public void CopyFrom(Category category)
        {
            this.CategoryID = category.CategoryID;
            this.Name = category.Name;
        }

        public bool Validate()
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

        public bool Exists { get { return CategoryID > 0; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
