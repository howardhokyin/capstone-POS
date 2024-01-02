using POS_System.Models;
using POS_System.Pages;
using System;
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

namespace POS_System
{
    /// <summary>
    /// Interaction logic for SplitBillDialog.xaml
    /// </summary>
    public partial class SplitBillDialog : Window
    {
        private ObservableCollection<SplitBill> _splitBills = new ObservableCollection<SplitBill>();
        private ObservableCollection<OrderedItem> _orderedItems = new ObservableCollection<OrderedItem>();
        public bool SplitByTotalAmount { get; private set; }
        public int NumberOfPeople { get; private set; }

        public string SplitType { get; private set ; }

        double _totalAmount;
        public SplitBillDialog()
        {
            InitializeComponent();
            NumberOfPeopleTextBox.Text = string.Empty;
        }
        public SplitBillDialog(ObservableCollection<OrderedItem>orderedItems,double totalAmount) : this() 
        {
            _orderedItems = orderedItems;
            _totalAmount = totalAmount;
          
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPeopleTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter number of bill on the text box!");
            }
            else
            {
                NumberOfPeople = Convert.ToInt32(NumberOfPeopleTextBox.Text);
                if (NumberOfPeople <= 0 && NumberOfPeople.Equals(""))
                {
                    MessageBox.Show("Please enter a valid number of people.");
                }
                else
                {


                    MessageBoxResult result = MessageBox.Show($"Do you want to split into {NumberOfPeople} bills?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        NumberOfPeople = int.Parse(NumberOfPeopleTextBox.Text);

                    }
                    else
                    {
                        return;
                    }


                    DialogResult = true;
                    Close();


                }
            }
 
            }

        }


    
}
