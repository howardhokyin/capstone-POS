using System;
using System.Collections.Generic;
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

namespace POS_System.Pages
{
    /// <summary>
    /// Interaction logic for ManagerManagement.xaml
    /// </summary>
    public partial class ManagerManagement : Window
    {
        public ManagerManagement()
        {
            InitializeComponent();
        }

        private void ManageUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AdminPage window when the button is clicked
            ManagerPage managerPage = new ManagerPage();
            managerPage.Show();
        }


        private void ManageSalesButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the OrderReport window when the button is clicked
            OrderReport orderReportWindow = new OrderReport();
            orderReportWindow.Show();

        }

        private void ManageTableButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Admin window when the button is clicked
            TablePage adminWindow = new TablePage();
            adminWindow.Show();

        }

        private void ManageRefundButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Admin window when the button is clicked
            Refund adminWindow = new Refund();
            adminWindow.Show();

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Perform logout actions here
            // For example, you can close the current window and navigate back to the login screen
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }
    }
}
