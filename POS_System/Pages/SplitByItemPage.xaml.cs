using MySql.Data.MySqlClient;
using POS.Models;
using POS_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace POS_System.Pages
{
    public partial class SplitByItemPage : Window
    {


        private ObservableCollection<OrderedItem> _allOrderedItem = new ObservableCollection<OrderedItem>();
        private ObservableCollection<OrderedItem> _splitedItem = new ObservableCollection<OrderedItem>();
        public ObservableCollection<OrderedItem> _assignCustomerIDItems { get; set; } = new ObservableCollection<OrderedItem>();

        private int orderID;
        public int currentCustomerId;

        public SplitByItemPage(ObservableCollection<OrderedItem> orderedItems)
        {
            InitializeComponent();

            _splitedItem.Clear();
            _allOrderedItem = orderedItems;
            // Set the ItemsSource of fullListItems
            fullListItems.ItemsSource = _allOrderedItem;

            // Set the ItemsSource of splitOrderedItems
            splitOrderedItems.ItemsSource = _splitedItem;

            // Initialize currentCustomerId to 1
            currentCustomerId = 1;
        }


        public SplitByItemPage(int orderId)
        {
            InitializeComponent();
            this.orderID = orderId;
        }


        private void addItem_Button(object sender, RoutedEventArgs e)
        {
            if (fullListItems.SelectedItem != null)
            {
                // Check if there is a selected item in the first ListView
                OrderedItem selectedItem = (OrderedItem)fullListItems.SelectedItem;

                if (selectedItem != null)
                {
                    // Remove the selected item from the first ListView
                    (_allOrderedItem).Remove(selectedItem);



                    // Add the selected item to the second ListView
                    _splitedItem.Add(selectedItem);




                    // Refresh the ListViews
                    fullListItems.Items.Refresh();
                    splitOrderedItems.Items.Refresh();
                }
            }
        }


        private void removeItem_Button(object sender, RoutedEventArgs e)
        {
            if (splitOrderedItems.SelectedItem != null)
            {
                OrderedItem selectedItem = (OrderedItem)splitOrderedItems.SelectedItem;

                if (selectedItem != null)
                {
                    // Remove the selected item from the second ListView
                    (_splitedItem).Remove(selectedItem);

                    // Add the selected item back to the first ListView
                    (_allOrderedItem).Add(selectedItem);

                    // Refresh the ListViews
                    splitOrderedItems.Items.Refresh();
                    fullListItems.Items.Refresh();
                }
            }
        }

        private void splitBill_Button(object sender, RoutedEventArgs e)
        {
            if (splitOrderedItems.Items.Count > 0)
            {

                foreach (OrderedItem splitedItem in _splitedItem)
                {
                    OrderedItem newSplitByItemBill = new OrderedItem
                    {

                        order_id = splitedItem.order_id,
                        item_id = splitedItem.item_id,
                        item_name = splitedItem.item_name,
                        Quantity = splitedItem.Quantity,
                        origialItemPrice = splitedItem.origialItemPrice,
                        ItemPrice = splitedItem.ItemPrice,
                        IsSavedItem = true,
                        customerID = currentCustomerId
                    };
                    _assignCustomerIDItems.Add(newSplitByItemBill);

                }

                var selectedItems = (_splitedItem as ObservableCollection<OrderedItem>);



                // Group items by customer
                var groupedItems = selectedItems.GroupBy(_splitedItem => _splitedItem.customerID);

                foreach (var group in groupedItems)
                {
                    // Calculate the total for the customer
                    decimal total = (decimal)group.Sum(item => item.ItemPrice);

                    // Create a message to display
                    string message = $"Customer {currentCustomerId} has the following items:\n";
                    foreach (var item in group)
                    {
                        message += $"- {item.item_name}\n";
                    }
                    message += $"\nTotal: {total:C}";

                    // Show a message box with the information
                    MessageBox.Show(message, "Customer Items");

                    // Add the customer and items information to the ListBox
                    AddCustomerItemsToListBox(currentCustomerId, group.Select(item => item.item_name).ToList());




                }

                if (_allOrderedItem.Count > 0)
                {
                    // Increment the customer ID for the next customer
                    currentCustomerId++;
                    _splitedItem.Clear();
                }
                else if (_allOrderedItem.Count.Equals(0))
                {
                    DialogResult = true;

                }
            }
        }

        private void AddCustomerItemsToListBox(int customerId, List<string> items)
        {
            var customerLabel = $"Customer {customerId}";
            var customerData = new CustomerData { CustomerLabel = customerLabel, Items = items };
            customerItemsListBox.Items.Add(customerData);
        }

        public class CustomerData
        {
            public string CustomerLabel { get; set; }
            public List<string> Items { get; set; }
        }


        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            // ... your other code ...

            // Group the SplitBill items by CustomerId
            var groupedSplitBills = _splitedItem.GroupBy(_splitedItem => _splitedItem.customerID);

            // Create a collection to hold the grouped data
            var groupedOrders = new ObservableCollection<OrderedItem>();

            // Iterate through the groups and add OrderedItem objects
            foreach (var group in groupedSplitBills)
            {
                foreach (var item in group)
                {
                    // Create OrderedItem objects from the SplitBill items
                    var orderedItem = new OrderedItem
                    {
                        item_name = item.item_name,
                        ItemPrice = item.ItemPrice,
                        // Set other properties as needed
                        customerID = item.customerID,
                    };

                    groupedOrders.Add(orderedItem);
                }
            }



            // Close the SplitByItemPage
            this.Close();
        }
    }
}