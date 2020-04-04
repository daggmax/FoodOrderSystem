using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalaceCashier.Model
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
