using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using POS.Models;
using POS_System.Dialog;

namespace POS_System.Pages
{
    public partial class TablePage : Window
    {

        private string connStr = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
        public string TableNumber { get; private set; }
        public string OrderType { get; private set; }
        public string userName { get; private set; }
        public string userId { get; private set; }


        public TablePage()
        {
            InitializeComponent();
            UpdateTableColors();

            UserNameTextBox.Text = "Welcome User ID: " + User.id;
            if (User.id >= 300)
            {
                logout_button.Content = "Logout";
                reset_button.Visibility = Visibility.Collapsed;
                Image logoutImage = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/POS_System;component/Images/Logout.png");
                bitmap.EndInit();
                logoutImage.Source = bitmap;
                logout_button.Content = logoutImage;
            } 
            
            else
            {
                logout_button.Content = "Close";
                
            }
        }

        public TablePage(string tableNumber, string orderType)
        {
            InitializeComponent();
            UpdateTableColors();
            
        }




        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if(User.id >= 300)
            {
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
                this.Close();
            }

            else
            {
                this.Close();
            }

        }



        // Handle table number, order number, order type
        private void OpenOrder_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string tableName = button.Name;
                int index = tableName.IndexOf('_');
                string tableNumber = tableName.Substring(index + 1); //D1 if dine-in T1 if Take-Out
                string orderType = tableName.Substring(0, index);

                string Type = "";
                if (orderType.Equals("table"))
                {
                    Type = "Dine-In";
                }
                else if (orderType.Equals("takeOut"))
                {
                    Type = "Take-Out";
                }

                bool hasUnpaidOrders = CheckForUnpaidOrders(tableNumber);

                // If there are unpaid orders, open the existing order
                if (hasUnpaidOrders)
                {
                    MenuPage menuPage = new MenuPage(tableNumber, Type, "Occupied",hasUnpaidOrders);
                    menuPage.Show();
                }
                else
                {
                    // If no unpaid orders exist, create a new order
                    MenuPage menuPage = new MenuPage(tableNumber, Type, "New Order", false);
                    menuPage.Show();
                }

                this.Close();
            }
        }

        // Check if there are unpaid orders for the specified table
        private bool CheckForUnpaidOrders(string tableNumber)
        {

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Check if there are unpaid orders for the specified table
                    string checkUnpaidOrdersSql = "SELECT order_id FROM `order` WHERE table_num = @tableNum AND paid = 'n';";
                    MySqlCommand checkUnpaidOrdersCmd = new MySqlCommand(checkUnpaidOrdersSql, conn);
                    checkUnpaidOrdersCmd.Parameters.AddWithValue("@tableNum", tableNumber);
                    object unpaidOrderId = checkUnpaidOrdersCmd.ExecuteScalar();

                    return (unpaidOrderId != null);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("MySQL Error: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking for unpaid orders: " + ex.ToString());
                    return false;
                }
            }
        }

        //Not working rest table colour
        private void ResetTableButtonColor()
        {
            TablePage tablePage = new TablePage();
            tablePage.Show();
            this.Close();
        }



        // Method to update table colors based on the database
        private void UpdateTableColors()
        {

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();



                    // Query the database for tables with paid=n
                    string query = "SELECT table_num FROM `order` WHERE paid = 'n';";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();


                    
                    while (reader.Read())
                    {
                        
                        // Get the table number from the query result
                        string tableNumber = reader.GetString(0);
                        
                        


                        string tableButtonName = "table_" + tableNumber;
                        string takeOutButtonName = "takeOut_" + tableNumber;


                        // Try to find the button by name
                        Button tableButton = FindName(tableButtonName) as Button;
                        Button takeOutButton = FindName(takeOutButtonName) as Button;

                        if (tableButton != null)
                        {
                            tableButton.Background = Brushes.Green;
                        } 
                        if (takeOutButton != null)
                        {
                            takeOutButton.Background = Brushes.Green;
                        } 


                    }



                    reader.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.ToString());
                }
            }
        }



        private void ResetTable_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Reset every table order?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                RemoveOrderAllTable();
                ResetTableButtonColor();
            }
            else
            {
                return;
            }

            
        }

        private void RemoveOrderAllTable()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();


                    string deleteItemListQuery = "DELETE FROM ordered_itemlist WHERE order_id > 0 ;";
                    MySqlCommand deleteItemListCmd = new MySqlCommand(deleteItemListQuery, conn);
                    deleteItemListCmd.ExecuteNonQuery();


                    string updateOrderStatusQuery = "UPDATE pos_db.order SET paid = 'c' WHERE order_id > 0 and paid = 'n';";


                    MySqlCommand deleteOrderCmd = new MySqlCommand(updateOrderStatusQuery, conn);
                    deleteOrderCmd.ExecuteNonQuery();


                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.ToString());
                }
            }
            

        }

        private void ChangeTable_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ChangeTableDialog();
            dialog.TableColorUpdated += Dialog_TableColorUpdated;
            dialog.ShowDialog();
            TablePage tablePage = new TablePage();
            this.Close();
            tablePage.Show();
        }
        private void Dialog_TableColorUpdated(object sender, EventArgs e)
        {
            UpdateTableColors();

        }



    }
}