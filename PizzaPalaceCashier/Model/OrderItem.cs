using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalaceCashier.Model
{
    public class OrderItem
    {
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
    }
}
