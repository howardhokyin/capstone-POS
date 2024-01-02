using MySql.Data.MySqlClient;
using POS.Models;
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
    /// Interaction logic for Refund.xaml
    /// </summary>
    public partial class Refund : Window
    {

        //String to make connection to database
        string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
        string orderId;
        string paymentId;
        string refundAmount;
        string refundMethod;
        string refundReason;
        string userId;

        public Refund()
        {
            InitializeComponent();
            getDataPaymentTable();
        }

        private void getDataPaymentTable()
        {
            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("select * from pos_db.payment", connection);

            //Open up connection with the user table
            connection.Open();

            //create a datatable object to capture the database table
            DataTable dt = new DataTable();

            //Execute the command and the load the result of reader inside the datatable
            dt.Load(cmd.ExecuteReader());

            //Close connection to user table
            connection.Close();

            //Bind data table to the DataGrid on XAML
            paymentGrid.DataContext = dt;
        }        
        
        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //This method populates the refundPaymentIdBox with the paymentId of the selected row in the DataGrid whenever a row is selected
        private void paymentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)paymentGrid.SelectedItem;
            if (selectedRow != null)
            {
                refundPaymentIdBox.Text = selectedRow["payment_id"].ToString();
                orderId = selectedRow["order_id"].ToString();
            }
        }

        private void RefundBtn_Click(object sender, RoutedEventArgs e)
        { 
            paymentId = refundPaymentIdBox.Text;
            refundAmount = refundAmountBox.Text;
            refundMethod = refundMethodComboBox.Text;
            refundReason = refundReasonBox.Text;
            userId = User.id.ToString();
            orderId = GetOrderID(paymentId);

            //MessageBox.Show("OrderID : "+ orderId + "\n" + "Payment ID : " + paymentId + "\n" + "Refund Amount : " +refundAmount + "\n" + "Refund Method : "+refundMethod + "\n" +"Refund Reason : "+ refundReason);
            
            if (paymentId.Length < 1 || refundAmount.Length < 1 || refundMethod.Length < 1 || refundReason.Length < 1)
            {
                MessageBox.Show("Please enter all fields");
            } else
            {
                //Create a connection object
                MySqlConnection connection = new MySqlConnection(connectionString);

                String sqlquery = "insert into pos_db.refund values (0, " + orderId + ", " + paymentId + ", " + refundAmount + ", '" + refundMethod + "', '" + refundReason + "', " + userId + ", sysdate());";

                //SQL query
                MySqlCommand cmd = new MySqlCommand(sqlquery, connection);

                //Open up connection with the user table
                connection.Open();

                cmd.ExecuteReader();

                connection.Close();

                MessageBox.Show("Refund Complete");
            }
        }

        private String GetOrderID(String paymentID)
        {
            String orderIDHolder;
            
            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //Open up connection with the user table
            connection.Open();

            String sqlquery = "select order_id from pos_db.payment where payment_id = @paymentID";
            //String sqlquery = "select order_id from pos_db.payment where payment_id = " + paymentID + " ;";

            //SQL query
            MySqlCommand cmd = new MySqlCommand(sqlquery, connection);

            //pass the value of paymentID into the sqlquery placeholder @paymentID
            cmd.Parameters.AddWithValue("@paymentID", int.Parse(paymentID));

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orderIDHolder = reader.GetValue(0).ToString();
            }

            return orderIDHolder = reader.GetValue(0).ToString();

            //Close connection to user table
            connection.Close();

            
        }
        
    }
}
