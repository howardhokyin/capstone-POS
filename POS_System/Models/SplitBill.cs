using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_System.Models
{
    public class SplitBill
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string SplitType { get; set; }
        public int CustomerId { get; set; }

        public string DisplayText => $"Customer #{CustomerId}";

        public SplitBill() { }

        public SplitBill(int paymentId, int orderId, int itemId, string itemName, int quantity, double price, int customerId, string splitType)
        {
            PaymentId = paymentId;
            OrderId = orderId;
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            Price = price;
            CustomerId = customerId;
            SplitType = splitType;
        }
    }
}



