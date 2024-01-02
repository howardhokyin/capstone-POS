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
using POS.Models;
using POS_System.Models;

namespace POS_System.Dialog
{
    /// <summary>
    /// Interaction logic for WelcomeDialog.xaml
    /// </summary>
    public partial class WelcomeDialog : Window
    {
        public WelcomeDialog()
        {
            InitializeComponent();
            
        }

        public WelcomeDialog(Window parentWindow, string authenticatedUsername):this()
        {
            if (parentWindow != null)
            {
                var parentCenterX = parentWindow.Left + (parentWindow.Width / 2);
                var parentCenterY = parentWindow.Top + (parentWindow.Height / 2);
                var dialogHalfWidth = this.Width / 2;
                var dialogHalfHeight = this.Height / 2 ;

                this.Left = parentCenterX - dialogHalfWidth;
                this.Top = parentCenterY - dialogHalfHeight;
            }

            UserIDTextBlock.Text = authenticatedUsername;
            string userTitle = "";
            if (User.id >99 &&  User.id < 200)
            {
                userTitle = "AdminPicture";
            } 
            
            else if (User.id > 199 && User.id < 300)
            {
                userTitle = "ManagerPicture";
            }

            else
            {
                userTitle = "WaiterPicture";
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"pack://application:,,,/POS_System;component/Images/{userTitle}.png");
            bitmap.EndInit();
            userIcon.Source = bitmap;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OK_Click(sender, e);
            }
        }
    }
}
