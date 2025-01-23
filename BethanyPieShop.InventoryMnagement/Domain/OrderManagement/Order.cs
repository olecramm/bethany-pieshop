using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BethanyPieShop.InventoryMnagement.Domain.OrderManagement
{
    public class Order
    {
        #region Properties
        public int Id { get; private set; }
        public DateTime OrderFulfillmentDate { get; private set; }
        public List<OrderItem> OrderItems { get;}
        public bool Fulfilled {  get; set; } = false;
        #endregion

        #region Constructor
        public Order()
        {
            Id = new Random().Next(9999999);
            int numberofSeconds = new Random().Next(100);
            OrderFulfillmentDate = DateTime.Now.AddSeconds(numberofSeconds);

            OrderItems = new List<OrderItem>();
        }
        #endregion

        #region Methods
        public string ShowOrderDetails()
        {
            StringBuilder orderDetails = new StringBuilder();

            orderDetails.AppendLine($"Order ID: {Id}");
            orderDetails.AppendLine($"Order fulfillment date: {OrderFulfillmentDate.ToShortTimeString}");

            if (OrderItems != null)
            {
                foreach (OrderItem item in OrderItems)
                {
                    orderDetails.AppendLine();
                }
            }
            return orderDetails.ToString();
        }
        #endregion
    }
}
