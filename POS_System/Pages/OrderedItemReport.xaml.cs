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
    public partial class OrderedItemReport : Window
    {

        public OrderedItemReport()
        {
            InitializeComponent();
            getDataOrderedListTable();

        }

        private void getDataOrderedListTable()
        {
            //Tutorial used https://www.youtube.com/watch?v=OPDPI5exPp8

            //db = new DatabaseHelper("localhost", "pos_db", "root", "password");

            //String to make connection to database
            string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("select * from pos_db.ordered_itemlist order by 5", connection);

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

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Tutorial used https://www.youtube.com/watch?v=qZwT_NWQ6Mk&t=14s   

            var row = sender as DataGridRow;
            var order = row.DataContext as Order;

            MessageBox.Show(order.OrderId.ToString());
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            String fromD = fromDate.ToString();
            String untilD = untilDate.ToString();
            String orderID = orderIdBoxFilter.Text;
            String itemID = itemIdBoxFilter.Text;
            String category = categoryBoxFilter.Text;
            String fromQuantity = fromQuantityBoxFilter.Text;
            String toQuantity = toQuantityBoxFilter.Text;
            String fromPrice = fromPriceBoxFilter.Text;
            String toPrice = toPriceBoxFilter.Text;

            //String to be added to sql query
            String orderIdQuery = "";
            String itemIdQuery = "";
            String categoryQuery = "";
            String fromToQuantityQuery = "";
            String fromToPriceQuery = "";
            String fromToDateQuery = "";

            int[] lengthcount = new int[6];     

            String sqlquery = "select * from pos_db.ordered_itemlist where ";

            if (fromD.Length + untilD.Length + orderID.Length + itemID.Length + category.Length + fromQuantity.Length + toQuantity.Length + fromPrice.Length + toPrice.Length < 1)
            {
                getDataOrderedListTable();
            } else {

                //This part checks for the filter that was entered by counting the length of the filter inputs.
                //If length of filter input > 0, then that filter was used, which then triggers the formulation of a sql query for that filter
                if (orderID.Length > 0)
                {
                    orderIdQuery = OrderIdFilter(orderID);
                    lengthcount[0] = 1;
                }

                if (itemID.Length > 0)
                {
                    itemIdQuery = ItemIdFilter(itemID);
                    lengthcount[1] = 1;
                }

                if (category.Length > 0)
                {
                    categoryQuery = CategoryFilter(category);
                    lengthcount[2] = 1;
                }

                if (fromQuantity.Length > 0 || toQuantity.Length > 0)
                {
                    fromToQuantityQuery = FromToQuantityFilter(fromQuantity, toQuantity);
                    lengthcount[3] = 1;
                }

                if (fromPrice.Length > 0 || toPrice.Length > 0)
                {
                    fromToPriceQuery = FromToPriceFilter(fromPrice, toPrice);
                    lengthcount[4] = 1;
                }

                if (fromD.Length > 0 || untilD.Length > 0)
                {
                    fromToDateQuery = FromToDateFilter(fromD, untilD);
                    lengthcount[5] = 1;
                }

                //This part of the code will build the sqlquery to be executed, by adding segments of sqlquery from the filter that was used
                if (lengthcount[0] == 1)
                {
                    sqlquery += orderIdQuery;
                }

                if (lengthcount[1] == 1 && lengthcount[1] == 1)
                {
                    sqlquery += " and " + itemIdQuery;
                } 
                else if (lengthcount[1] == 1 && lengthcount[0] == 0)
                {
                    sqlquery += itemIdQuery;
                }

                if (lengthcount[2] == 1 && ((lengthcount[0] == 1 || lengthcount[1] == 1)))
                {
                    sqlquery += " and " + categoryQuery;
                }
                else if (lengthcount[2] == 1 && ((lengthcount[0] == 0 && lengthcount[1] == 0)))
                {
                    sqlquery += categoryQuery;
                }

                if (lengthcount[3] == 1 && ((lengthcount[0] == 1 || lengthcount[1] == 1 || lengthcount[2] == 1)))
                {
                    sqlquery += " and " + fromToQuantityQuery;
                }
                else if (lengthcount[3] == 1 && ((lengthcount[0] == 0 && lengthcount[1] == 0 && lengthcount[2] == 0)))
                {
                    sqlquery += fromToQuantityQuery;
                }

                if (lengthcount[4] == 1 && ((lengthcount[0] == 1 || lengthcount[1] == 1 || lengthcount[2] == 1 || lengthcount[3] == 1)))
                {
                    sqlquery += " and " + fromToPriceQuery;
                }
                else if (lengthcount[4] == 1 && ((lengthcount[0] == 0 && lengthcount[1] == 0 && lengthcount[2] == 0 && lengthcount[3] == 0)))
                {
                    sqlquery += fromToPriceQuery;
                }

                if (lengthcount[5] == 1 && ((lengthcount[0] == 1 || lengthcount[1] == 1 || lengthcount[2] == 1 || lengthcount[3] == 1 || lengthcount[4] == 1)))
                {
                    sqlquery += " and " + fromToDateQuery;
                }
                else if (lengthcount[5] == 1 && ((lengthcount[0] == 0 && lengthcount[1] == 0 && lengthcount[2] == 0 && lengthcount[3] == 0 && lengthcount[4] == 0)))
                {
                    sqlquery += fromToDateQuery;
                }

                sqlquery = sqlquery + " order by 5;";

                //String to make connection to database
                string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

                //Create a connection object
                MySqlConnection connection = new MySqlConnection(connectionString);

                //SQL query
                MySqlCommand cmd = new MySqlCommand(sqlquery, connection);

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


        }

        private String FromToDateFilter(String fromD, String untilD)
        {
            String fromToDateFilterString = "";

            String fromFormatted = "";
            String toFormatted = "";
            String fromReplace = "";
            String toReplace = "";
            String[] fromSplit;
            String[] toSplit;

            //input validation. if input not empty, proceed to format input string to be used in sql query
            if (fromD.Length > 0)
            {
                fromReplace = fromD.Replace('/', ' ');
                fromSplit = fromReplace.Split(' ');
                fromFormatted = fromSplit[2] + '-' + fromSplit[0] + '-' + fromSplit[1].ToString();
            }

            //input validation. if input not empty, proceed to format input string to be used in sql query
            if (untilD.Length > 0)
            {
                toReplace = untilD.Replace('/', ' ');
                toSplit = toReplace.Split(' ');
                toFormatted = toSplit[2] + '-' + toSplit[0] + '-' + toSplit[1].ToString();

            }

            //sql query formulation based on selected filter

            //when only the until-date is selected
            if (fromFormatted.Length > 0 && toFormatted.Length < 1)
            {
                fromToDateFilterString = " order_timestamp >= '" + fromFormatted + "'";

                //when only the from-date is selected
            }
            else if (toFormatted.Length > 0 && fromFormatted.Length < 1)
            {
                fromToDateFilterString = " order_timestamp <= '" + toFormatted + "' + interval 1 day";

                //when a date-range is selected
            }
            else
            {
                fromToDateFilterString = " order_timestamp between '" + fromFormatted + "' and '" + toFormatted + "' + interval 1 day";
            }

            return fromToDateFilterString;
        }

        private String OrderIdFilter(String orderID)
        {
            String orderIdFilterString;

            if (orderID.Contains(","))
            {
                orderIdFilterString = "order_id in (" + orderID + ")";
            }
            else if (orderID.Contains(" "))
            {
                orderIdFilterString = "order_id in (" + orderID.Replace(" ", ",") + ")";
            }
            else
            {
                orderIdFilterString = "order_id = " + orderID;
            }

            return orderIdFilterString;
        }

        private String ItemIdFilter(String itemID)
        {
            String itemIdFilterString;

            if (itemID.Contains(','))
            {
                itemIdFilterString = "item_id in (" + itemID + ")";
            }
            else if (itemID.Contains(' '))
            {
                itemIdFilterString = "item_id in (" + itemID.Replace(' ', ',') + ")";
            }
            else
            {
                itemIdFilterString = "item_id = " + itemID;
            }

            return itemIdFilterString;
        }

        private String CategoryFilter(String category)
        {
            String categoryFilterString;

            if (category.Contains(','))
            {
                categoryFilterString = "category in (" + category + ")";
            }
            else if (category.Contains(' '))
            {
                categoryFilterString = "category in (" + category.Replace(' ', ',') + ")";
            }
            else
            {
                categoryFilterString = "category = " + category;
            }

            return categoryFilterString;
        }

        private String FromToQuantityFilter(String fromQuantity, String toQuantity)
        {
            String fromToQuantityFilterString = "";

            if (fromQuantity.Length > 0 && toQuantity.Length > 0)
            {
                fromToQuantityFilterString = "quantity between " + fromQuantity + " and " + toQuantity;
            }
            else if (fromQuantity.Length > 0 && toQuantity.Length < 1)
            {
                fromToQuantityFilterString = "quantity >= " + fromQuantity;
            }
            else if (fromQuantity.Length < 1 && toQuantity.Length > 0)
            {
                fromToQuantityFilterString = "quantity >= " + toQuantity;
            }

            return fromToQuantityFilterString;
        }

        private String FromToPriceFilter(String fromPrice, String toPrice)
        {
            String fromToPriceFilterString = "";

            if (fromPrice.Length > 0 && toPrice.Length > 0)
            {
                fromToPriceFilterString = "price between " + fromPrice + " and " + toPrice;
            }
            else if (fromPrice.Length > 0 && toPrice.Length < 1)
            {
                fromToPriceFilterString = "price >= " + fromPrice;
            }
            else if (fromPrice.Length < 1 && toPrice.Length > 0)
            {
                fromToPriceFilterString = "price >= " + fromPrice;
            }

            return fromToPriceFilterString;
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
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

    }

}
