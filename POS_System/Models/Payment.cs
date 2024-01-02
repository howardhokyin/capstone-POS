using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace POS_System.Models
{
    public class Payment : INotifyPropertyChanged
    {
        private int _customerID;
        private int _paymentID;
        private long _orderID;
        private string _tableNumber;
        private string _orderType;
        private string _paymentMethod;
        private double _baseAmount;
        private double _GST;
        private double _customerPaymentTotalAmount;
        private double _grossAmount;
        private double _customerChangeAmount;
        private double _tip;
        private List<OrderedItem> _itemList = new List<OrderedItem>();

        public ObservableCollection<OrderedItem> eachCustomerItems { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Payment()
        {

        }

        public Payment(int customerID, int paymentID, long orderID,string tableNumber ,string orderType, string paymentMethod, double baseAmount, double GST, double customerPaymentTotalAmount, double grossAmount, double customerChangeAmount, double tip, List<OrderedItem> eachCustomerItems)
        {

            this._customerID = customerID;
            this._paymentID = paymentID;
            this._orderID = orderID;
            this._tableNumber = tableNumber;
            this._orderType = orderType;
            this._paymentMethod = paymentMethod;
            this._baseAmount = baseAmount;
            this._GST = GST;
            this._customerPaymentTotalAmount = customerPaymentTotalAmount;
            this._grossAmount = grossAmount;
            this._customerChangeAmount = customerChangeAmount;
            this._tip = tip;
            this._itemList = eachCustomerItems;
        }

        public int customerID
        {
            get => _customerID;
            set
            {
                if (_customerID != value)
                {
                    _customerID = value;
                    OnPropertyChanged(nameof(customerID));
                }
            }
        }

        public int paymentID
        {
            get => _paymentID;
            set
            {
                if (_paymentID != value)
                {
                    _paymentID = value;
                    OnPropertyChanged(nameof(paymentID));
                }
            }
        }

        public long orderID
        {
            get => _orderID;
            set
            {
                if (_orderID != value)
                {
                    _orderID = value;
                    OnPropertyChanged(nameof(orderID));
                }
            }
        }

        public string tableNumber
        {
            get => _tableNumber;
            set
            {
                if (_tableNumber != value)
                {
                    _tableNumber = value;
                    OnPropertyChanged(nameof(tableNumber));
                }
            }
        }

        public string orderType
        {
            get => _orderType;
            set
            {
                if (_orderType != value)
                {
                    _orderType = value;
                    OnPropertyChanged(nameof(orderType));
                }
            }
        }

        public string paymentMethod
        {
            get => _paymentMethod;
            set
            {
                if (_paymentMethod != value)
                {
                    _paymentMethod = value;
                    OnPropertyChanged(nameof(paymentMethod));
                }
            }
        }

        public double baseAmount
        {
            get => _baseAmount;
            set
            {
                if (_baseAmount != value)
                {
                    _baseAmount = value;
                    OnPropertyChanged(nameof(baseAmount));
                }
            }
        }

        public double GST
        {
            get => _GST;
            set
            {
                if (_GST != value)
                {
                    _GST = value;
                    OnPropertyChanged(nameof(GST));
                }
            }
        }

        public double customerPaymentTotalAmount
        {
            get => _customerPaymentTotalAmount;
            set
            {
                if (_customerPaymentTotalAmount != value)
                {
                    _customerPaymentTotalAmount = value;
                    OnPropertyChanged(nameof(customerPaymentTotalAmount));
                }
            }
        }

        public double grossAmount
        {
            get => _grossAmount;
            set
            {
                if (_grossAmount != value)
                {
                    _grossAmount = value;
                    OnPropertyChanged(nameof(grossAmount));
                }
            }
        }

        public double customerChangeAmount
        {
            get => _customerChangeAmount;
            set
            {
                if (_customerChangeAmount != value)
                {
                    _customerChangeAmount = value;
                    OnPropertyChanged(nameof(customerChangeAmount));
                }
            }
        }

        public double tip
        {
            get => _tip;
            set
            {
                if (_tip != value)
                {
                    _tip = value;
                    OnPropertyChanged(nameof(tip));
                }
            }
        }

        public List<OrderedItem> ItemList
        {
            get => _itemList;
            set
            {
                if (_itemList != value)
                {
                    _itemList = value;
                    OnPropertyChanged(nameof(ItemList));
                }
            }
        }


    }
}
