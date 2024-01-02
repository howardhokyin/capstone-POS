using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using POS.Models;
using POS_System.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        private string connStr = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
        private string _tableNumber;
        private string _orderType;
        private long _orderId;
        private string _status;
        private bool _hasUnpaidOrders = true;
        private int _numberOfBill;
        private string paymentMethod;
        private double totalItemPriceForCustomer;
        private bool isSettled;
        PaymentPage paymentPage = new PaymentPage();



        private ObservableCollection<OrderedItem> _orderedItems = new ObservableCollection<OrderedItem>();
        private ObservableCollection<OrderedItem> _customerOrderedItems = new ObservableCollection<OrderedItem>();
        private List<Payment> _paymentList = new List<Payment>();
        private MenuPage _menuPage;


        public PaymentWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            {
                if (DisplayCustomerButton_Panel.Children.Count > 0 &&
                    DisplayCustomerButton_Panel.Children[0] is Button firstButton)
                {
                    paymentPageButton_Click(firstButton, new RoutedEventArgs(Button.ClickEvent));
                }
            }

        }




        public PaymentWindow(MenuPage menuPage, ObservableCollection<OrderedItem> orderedItems, string tableNumber, string orderType, long orderId, string status, bool hasUnpaidOrders, int numberOfBill) : this()
        {
            _tableNumber = tableNumber;
            _orderedItems = orderedItems;
            _orderType = orderType;
            _orderId = orderId;
            _status = status;
            _hasUnpaidOrders = hasUnpaidOrders;
            _numberOfBill = numberOfBill;
            _menuPage = menuPage;
            ShowPaymentPageButton(_numberOfBill);


        }



        //Method for show how many button on top based on number of bills.
        private void ShowPaymentPageButton(int _numberOfBill)
        {
            DisplayCustomerButton_Panel.Children.Clear();
            int customerNumber = 1;
            int splitNumber = _numberOfBill;
            do
            {
                Button paymentPageButton = new Button();
                
                paymentPageButton.Content = "Customer#" + customerNumber;
                paymentPageButton.Tag = customerNumber;
                paymentPageButton.Click += paymentPageButton_Click;
                DisplayCustomerButton_Panel.Children.Add(paymentPageButton);

                customerNumber++;
                splitNumber--;
            }
            while (splitNumber > 0);


        }



        private void paymentPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender is Button button)
            {
                _customerOrderedItems.Clear();
                CustomerNumberDisplay_TextBlock.Text = button.Content.ToString();
                string customerID = button.Tag.ToString();
                PaymentPage paymentPage = LoadCustomerPaymentPage(customerID);

                paymentPage.PaymentCompleted += (s, args) =>
                {
                    // Disable the button associated with the completed payment
                    ((Button)sender).IsEnabled = false;
                    

                    ClickNextButton(button);
                };

                PaymentPageFrame.Navigate(paymentPage);

            }
        }

        private void ClickNextButton(Button currentButton)
        {
            var buttons = DisplayCustomerButton_Panel.Children.OfType<Button>().ToList();
            int currentIndex = buttons.IndexOf(currentButton);
            if (currentIndex >= 0 && currentIndex < buttons.Count - 1)
            {
                Button nextButton = buttons[currentIndex + 1];
                if (nextButton != null && nextButton.IsEnabled)
                {
                    paymentPageButton_Click(nextButton, new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }


        //Method for return payment page
        private PaymentPage LoadCustomerPaymentPage(string customerID)
        {
            PaymentPage PaymentBaseOnCustomerID = new PaymentPage();
            int customerNumber = int.Parse(customerID);

            foreach (var order in _orderedItems)
            {
                if (order.customerID == customerNumber)
                {

                    OrderedItem ForEachCustomer = new OrderedItem()
                    {
                        order_id = order.customerID,
                        item_id = order.item_id,
                        item_name = order.item_name,
                        Quantity = order.Quantity,
                        ItemPrice = order.ItemPrice,
                        origialItemPrice = order.origialItemPrice,
                        IsSavedItem = true,
                        customerID = order.customerID,
                        isSettled = false


                    };
                    _customerOrderedItems.Add(ForEachCustomer);


                    PaymentBaseOnCustomerID = new PaymentPage(_menuPage, this, _customerOrderedItems, _tableNumber, _orderType, _orderId, _status, false, customerNumber, _numberOfBill);

                }
                else if (order.customerID == 0)
                {
                    PaymentBaseOnCustomerID = new PaymentPage(_menuPage, this, _orderedItems, _tableNumber, _orderType, _orderId, _status, false, customerNumber, _numberOfBill);

                }


            }
            return PaymentBaseOnCustomerID;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
