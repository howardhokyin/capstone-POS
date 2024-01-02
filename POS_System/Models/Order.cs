using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using POS.Models;

namespace POS_System.Models
{
    class Order
    {
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderTimestamp { get; set; }
        public double TotalAmount { get; set; }
        public double orderType {  get; set; }
        public List<OrderedItem> OrderedItems { get; set; }

        

        public Order(int tableNumber)
        {
            TableNumber = tableNumber;
            OrderTimestamp = DateTime.Now;
            TotalAmount = 0.0;
            OrderedItems = new List<OrderedItem>();
        }

        public void AddItem(Item item, int quantity)
        {
            double itemTotal = item.ItemPrice * quantity;
            OrderedItem orderedItem = new OrderedItem()
            {
                item_name = item.item_name,
                Quantity = quantity,
                ItemPrice = item.ItemPrice
            };
            OrderedItems.Add(orderedItem);
            TotalAmount += itemTotal;
        }


        public void SaveOrderToDatabase()
        {
            string connStr = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();

                // Insert the order into the 'order' table
                string insertOrderSql = "INSERT INTO `order` (table_num, order_timestamp, total_amount) " +
                                        "VALUES (@table_num, @order_timestamp, @total_amount);";
                MySqlCommand insertOrderCmd = new MySqlCommand(insertOrderSql, conn);
                insertOrderCmd.Parameters.AddWithValue("@table_num", TableNumber);
                insertOrderCmd.Parameters.AddWithValue("@order_timestamp", OrderTimestamp);
                insertOrderCmd.Parameters.AddWithValue("@total_amount", TotalAmount);
                insertOrderCmd.ExecuteNonQuery();

                // Retrieve the auto-generated order ID
                OrderId = (int)insertOrderCmd.LastInsertedId;

                // Insert ordered items into the 'ordered_itemlist' table
                foreach (OrderedItem orderedItem in OrderedItems)
                {
                    string insertOrderedItemSql = "INSERT INTO ordered_itemlist (order_id, item_id, quantity, item_price) " +
                                                  "VALUES (@order_id, @item_id, @quantity, @item_price);";
                    MySqlCommand insertOrderedItemCmd = new MySqlCommand(insertOrderedItemSql, conn);
                    insertOrderedItemCmd.Parameters.AddWithValue("@order_id", OrderId);
                    insertOrderedItemCmd.Parameters.AddWithValue("@item_id", orderedItem.item_id);
                    insertOrderedItemCmd.Parameters.AddWithValue("@quantity", orderedItem.Quantity);
                    insertOrderedItemCmd.Parameters.AddWithValue("@item_price", orderedItem.ItemPrice);
                    insertOrderedItemCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }
    }
}
