using System;

namespace PizzaPalace.Model
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime? OrderTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
