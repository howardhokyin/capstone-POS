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

namespace POS_System
{
    /// <summary>
    /// Interaction logic for EditCategoryDialog.xaml
    /// </summary>
    public partial class EditCategoryDialog : Window
    {
        public int EditedCategoryId { get; set; }
        public string EditedCategoryName { get; private set; }

        public EditCategoryDialog(int currentCategoryId,string currentCategoryName)
        {
            InitializeComponent();
            CategoryIdTextBox.Text = currentCategoryId.ToString();
            CategoryNameTextBox.Text = currentCategoryName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                EditedCategoryName = CategoryNameTextBox.Text;
                EditedCategoryId = int.Parse(CategoryIdTextBox.Text);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }


    }
}
