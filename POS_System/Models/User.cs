using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Models
{
    public class User : INotifyPropertyChanged
    {
        public static int id;
        public static string name;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public User() { }

        public User(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }



        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(id));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(name));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(password));
            }
        }

        void OnPropertyChanged(string id) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(id));

    }
}

