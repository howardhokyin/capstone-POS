using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using POS_System.Models;

namespace POS_System.Pages
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class OrderReport : Window
    {

        string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

        public OrderReport()
        {
            InitializeComponent();
            getDataOrderTable();
            getDataOrderedListTable();
            GetDataRefundTable();
        }

        private void getDataOrderTable()
        {
            //Tutorial used https://www.youtube.com/watch?v=OPDPI5exPp8

            //db = new DatabaseHelper("localhost", "pos_db", "root", "password");

            //String to make connection to database


            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("select * from pos_db.order order by 3", connection);

            //Open up connection with the user table
            connection.Open();

            //create a datatable object to capture the database table
            DataTable dt = new DataTable();

            //Execute the command and the load the result of reader inside the datatable
            dt.Load(cmd.ExecuteReader());

            //Close connection to user table
            connection.Close();

            //Bind data table to the DataGrid on XAML
            orderGrid.DataContext = dt;
        }

        //private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    //Tutorial used https://www.youtube.com/watch?v=qZwT_NWQ6Mk&t=14s   

        //    var row = sender as DataGridRow;
        //    var order = row.DataContext as Order;

        //    MessageBox.Show(order.OrderId.ToString());
        //}

        private void filterBtnOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fromTimestamp = fromDateOrder.SelectedDate?.ToString("yyyy-MM-dd");
                string untilTimestamp = untilDateOrder.SelectedDate?.ToString("yyyy-MM-dd");
                string tableNum = specificTableBoxFilter.Text;
                string orderType = OrderTypeComboBox.Text;
                string orderStatus = OrderStatusComboBox.Text;
                string fromAmount = fromAmountBoxFilter.Text;
                string toAmount = toAmountBoxFilter.Text;


                if (string.IsNullOrEmpty(fromTimestamp) && string.IsNullOrEmpty(untilTimestamp) && string.IsNullOrEmpty(tableNum) && string.IsNullOrEmpty(orderType) && string.IsNullOrEmpty(orderStatus) && string.IsNullOrEmpty(fromAmount) && string.IsNullOrEmpty(toAmount))
                {
                    getDataOrderTable();
                }
                else
                {
                    string sqlQuery = "SELECT * FROM pos_db.order WHERE 1=1";

                    if (!string.IsNullOrEmpty(fromTimestamp) && !string.IsNullOrEmpty(untilTimestamp))
                    {
                        sqlQuery += " AND order_timestamp BETWEEN @fromTimestamp AND @untilTimestamp + interval 1 day";
                    }
                    else if (!string.IsNullOrEmpty(fromTimestamp))
                    {
                        sqlQuery += " AND order_timestamp >= @fromTimestamp";
                    }
                    else if (!string.IsNullOrEmpty(untilTimestamp))
                    {
                        sqlQuery += " AND order_timestamp <= @untilTimestamp + interval 1 day";
                    }

                    if (!string.IsNullOrEmpty(tableNum))
                    {
                        sqlQuery += " AND table_num = @tableNum";
                    }

                    if (!string.IsNullOrEmpty(orderType))
                    {
                        sqlQuery += " AND order_type = @orderType";
                    }

                    if (!string.IsNullOrEmpty(orderStatus))
                    {
                        sqlQuery += " AND paid = @orderStatus";
                    }

                    if (!string.IsNullOrEmpty(fromAmount) && !string.IsNullOrEmpty(toAmount))
                    {
                        sqlQuery += " AND total_amount BETWEEN @fromAmount AND @toAmount";
                    }
                    else if (!string.IsNullOrEmpty(fromAmount))
                    {
                        sqlQuery += " AND total_amount >= @fromAmount";
                    }
                    else if (!string.IsNullOrEmpty(toAmount))
                    {
                        sqlQuery += " AND total_amount <= @toAmount";
                    }

                    sqlQuery += " order by 3;";

                    MySqlConnection connection = new MySqlConnection(connectionString);
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);

                    // Add parameters for user inputs        
                    cmd.Parameters.AddWithValue("@fromTimestamp", fromTimestamp);
                    cmd.Parameters.AddWithValue("@untilTimestamp", untilTimestamp);
                    cmd.Parameters.AddWithValue("@tableNum", tableNum);
                    cmd.Parameters.AddWithValue("@orderType", orderType);
                    cmd.Parameters.AddWithValue("@orderStatus", orderStatus);
                    cmd.Parameters.AddWithValue("@fromAmount", fromAmount);
                    cmd.Parameters.AddWithValue("@toAmount", toAmount);

                    connection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    connection.Close();

                    orderGrid.DataContext = dt;

                    OrderTypeComboBox.Text = "";
                    OrderStatusComboBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void printBtnOrder_Click(object sender, RoutedEventArgs e)
        {
            //Tutorial used https://www.youtube.com/watch?v=z7SZsmSjsfM minute 19 onwards

            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(printReport, "Order Report");
                }
            }
            catch (Exception ex)
            {

            }
        }

        /* OrderedItemReport.xaml.cs */
        private void getDataOrderedListTable()
        {
            //Tutorial used https://www.youtube.com/watch?v=OPDPI5exPp8

            //db = new DatabaseHelper("localhost", "pos_db", "root", "password");

            //String to make connection to database
            string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("SELECT oi.order_id, i.item_category, oi.item_id, i.item_name, oi.item_price, oi.quantity, o.order_timestamp FROM ordered_itemlist oi JOIN item i ON oi.item_id = i.item_id JOIN pos_db.order o on oi.order_id = o.order_id order by 7;", connection);

            //Open up connection with the user table
            connection.Open();

            //create a datatable object to capture the database table
            DataTable dt = new DataTable();

            //Execute the command and the load the result of reader inside the datatable
            dt.Load(cmd.ExecuteReader());

            //Close connection to user table
            connection.Close();

            //Bind data table to the DataGrid on XAML
            orderedItemListGrid.DataContext = dt;
        }


        private void filterBtnItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fromTimestamp = fromDateItem.SelectedDate?.ToString("yyyy-MM-dd");
                string untilTimestamp = untilDateItem.SelectedDate?.ToString("yyyy-MM-dd");
                string itemId = itemIdBoxFilter.Text;
                string orderId = orderIdFilter.Text;
                string category = categoryBoxFilter.Text;
                string fromQuantity = fromQuantityBoxFilter.Text;
                string toQuantity = toQuantityBoxFilter.Text;
                string fromPrice = fromPriceBoxFilter.Text;
                string toPrice = toPriceBoxFilter.Text;


                if (string.IsNullOrEmpty(fromTimestamp) && string.IsNullOrEmpty(untilTimestamp) && string.IsNullOrEmpty(itemId) && string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(category) && string.IsNullOrEmpty(fromQuantity) && string.IsNullOrEmpty(toQuantity) && 
                    string.IsNullOrEmpty(fromPrice) && string.IsNullOrEmpty(toPrice))
                {
                    getDataOrderedListTable();
                }
                else
                {
                    string sqlQuery = "SELECT oi.order_id, i.item_category, oi.item_id, i.item_name, oi.item_price, oi.quantity, o.order_timestamp FROM ordered_itemlist oi JOIN item i ON oi.item_id = i.item_id JOIN pos_db.order o on oi.order_id = o.order_id WHERE 1=1";

                    if (!string.IsNullOrEmpty(fromTimestamp) && !string.IsNullOrEmpty(untilTimestamp))
                    {
                        sqlQuery += " AND o.order_timestamp BETWEEN @fromTimestamp AND @untilTimestamp + interval 1 day";
                    }
                    else if (!string.IsNullOrEmpty(fromTimestamp))
                    {
                        sqlQuery += " AND o.order_timestamp >= @fromTimestamp";
                    }
                    else if (!string.IsNullOrEmpty(untilTimestamp))
                    {
                        sqlQuery += " AND o.order_timestamp <= @untilTimestamp + interval 1 day";
                    }

                    if (!string.IsNullOrEmpty(itemId))
                    {
                        sqlQuery += " AND oi.item_id = @itemId";
                    }

                    if (!string.IsNullOrEmpty(orderId))
                    {
                        sqlQuery += " AND oi.order_id = @orderId";
                    }

                    if (!string.IsNullOrEmpty(category))
                    {
                        sqlQuery += " AND i.item_category = @category";
                    }

                    if (!string.IsNullOrEmpty(fromQuantity) && !string.IsNullOrEmpty(toQuantity))
                    {
                        sqlQuery += " AND oi.quantity BETWEEN @fromQuantity AND @toQuantity";
                    }
                    else if (!string.IsNullOrEmpty(fromQuantity))
                    {
                        sqlQuery += " AND oi.quantity >= @fromQuantity";
                    }
                    else if (!string.IsNullOrEmpty(toQuantity))
                    {
                        sqlQuery += " AND oi.quantity <= @toQuantity";
                    }

                    if (!string.IsNullOrEmpty(fromPrice) && !string.IsNullOrEmpty(toPrice))
                    {
                        sqlQuery += " AND oi.item_price BETWEEN @fromPrice AND @toPrice";
                    }
                    else if (!string.IsNullOrEmpty(fromPrice))
                    {
                        sqlQuery += " AND oi.item_price >= @fromPrice";
                    }
                    else if (!string.IsNullOrEmpty(toPrice))
                    {
                        sqlQuery += " AND oi.item_price <= @toPrice";
                    }

                    sqlQuery += " order by 7";



                    MySqlConnection connection = new MySqlConnection(connectionString);
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);


                    // Add parameters for user inputs        
                    cmd.Parameters.AddWithValue("@fromTimestamp", fromTimestamp);
                    cmd.Parameters.AddWithValue("@untilTimestamp", untilTimestamp);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@fromQuantity", fromQuantity);
                    cmd.Parameters.AddWithValue("@toQuantity", toQuantity);
                    cmd.Parameters.AddWithValue("@fromPrice", fromPrice);
                    cmd.Parameters.AddWithValue("@toPrice", toPrice);

                    connection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    connection.Close();

                    orderedItemListGrid.DataContext = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

       

        private void printBtnItem_Click(object sender, RoutedEventArgs e)
        {
            //Tutorial used https://www.youtube.com/watch?v=z7SZsmSjsfM minute 19 onwards

            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(printReport, "Ordered Itemlist Report");
                }
            }
            catch (Exception ex)
            {

            }
        }

        /* RefundReport.xaml.cs */
        private void GetDataRefundTable()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM refund order by 8", connection);

                connection.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                refundGrid.DataContext = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fromTimestamp = refundFromDateFilter.SelectedDate?.ToString("yyyy-MM-dd");
                string untilTimestamp = refundToDateFilter.SelectedDate?.ToString("yyyy-MM-dd");
                string refundId = refundIdFilter.Text;
                string orderId = orderIdFilter.Text;
                string paymentId = paymentIdFilter.Text;
                string refundAmount = refundAmountFilter.Text;
                string specificMethod = refundMethodFilter.Text;
                string specificUser = userIdFilter.Text;


                if (string.IsNullOrEmpty(fromTimestamp) && string.IsNullOrEmpty(untilTimestamp) && string.IsNullOrEmpty(specificMethod) && string.IsNullOrEmpty(specificUser) && string.IsNullOrEmpty(refundId) && string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(paymentId) && string.IsNullOrEmpty(refundAmount))
                {
                    GetDataRefundTable();
                }
                else
                {
                    string sqlQuery = "SELECT * FROM refund WHERE 1=1";

                    if (!string.IsNullOrEmpty(fromTimestamp) && !string.IsNullOrEmpty(untilTimestamp))
                    {
                        sqlQuery += " AND refund_timestamp BETWEEN @fromTimestamp AND @untilTimestamp + interval 1 day";
                    }
                    else if (!string.IsNullOrEmpty(fromTimestamp))
                    {
                        sqlQuery += " AND refund_timestamp >= @fromTimestamp";
                    }
                    else if (!string.IsNullOrEmpty(untilTimestamp))
                    {
                        sqlQuery += " AND refund_timestamp <= @untilTimestamp + interval 1 day";
                    }

                    if (!string.IsNullOrEmpty(specificMethod))
                    {
                        sqlQuery += " AND refund_method = @specificMethod";
                    }

                    if (!string.IsNullOrEmpty(specificUser))
                    {
                        sqlQuery += " AND user_id = @specificUser";
                    }

                    if (!string.IsNullOrEmpty(refundId))
                    {
                        sqlQuery += " AND refund_id = @refundId";
                    }

                    if (!string.IsNullOrEmpty(orderId))
                    {
                        sqlQuery += " AND order_id = @orderId";
                    }

                    if (!string.IsNullOrEmpty(paymentId))
                    {
                        sqlQuery += " AND payment_id = @paymentId";
                    }

                    if (!string.IsNullOrEmpty(refundAmount))
                    {
                        sqlQuery += " AND refund_amount = @refundAmount";
                    }

                    sqlQuery += " order by 8";

                    MySqlConnection connection = new MySqlConnection(connectionString);
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);

                    // Add parameters for user inputs        
                    cmd.Parameters.AddWithValue("@fromTimestamp", fromTimestamp);
                    cmd.Parameters.AddWithValue("@untilTimestamp", untilTimestamp);
                    cmd.Parameters.AddWithValue("@specificMethod", specificMethod);
                    cmd.Parameters.AddWithValue("@specificUser", specificUser);
                    cmd.Parameters.AddWithValue("@refundId", refundId);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@paymentId", paymentId);
                    cmd.Parameters.AddWithValue("@refundAmount", refundAmount);

                    connection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    connection.Close();

                    refundGrid.DataContext = dt;

                    refundMethodFilter.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(refundGrid, "Refund Report");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}