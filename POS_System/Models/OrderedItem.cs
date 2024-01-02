using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace POS_System.Models
{
    public class OrderedItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _order_id;
        private int _item_id;
        private string _item_name;
        private int _quantity;
        private double _itemPrice;
        private double _originalItemPrice;
        private bool _isSavedItem;
        private bool _isSettled;
        private int _customerID;



        // Properties
        public int order_id
        {
            get { return _order_id; }
            set
            {
                if (_order_id != value)
                {
                    _order_id = value;
                    OnPropertyChanged(nameof(order_id));
                }
            }
        }

        public int item_id
        {
            get { return _item_id; }
            set
            {
                if (_item_id != value)
                {
                    _item_id = value;
                    OnPropertyChanged(nameof(item_id));
                }
            }
        }

        public string item_name
        {
            get { return _item_name; }
            set
            {
                if (_item_name != value)
                {
                    _item_name = value;
                    OnPropertyChanged(nameof(item_name));
                }
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public double ItemPrice
        {
            get { return _itemPrice; }
            set
            {
                if (_itemPrice != value)
                {
                    _itemPrice = value;
                    OnPropertyChanged(nameof(ItemPrice));
                }
            }
        }

        public double origialItemPrice
        {
            get { return _originalItemPrice; }
            set
            {
                if (_originalItemPrice != value)
                {
                    _originalItemPrice = value;
                    OnPropertyChanged(nameof(origialItemPrice));
                }
            }
        }

        public bool IsSavedItem
        {
            get { return _isSavedItem; }
            set
            {
                if (_isSavedItem != value)
                {
                    _isSavedItem = value;
                    OnPropertyChanged(nameof(IsSavedItem));
                }
            }
        }

        public bool isSettled
        {
            get
            {
                return _isSettled;
            }
            set
            {
                if (isSettled != value)
                {
                    _isSettled = value;
                    OnPropertyChanged(nameof(isSettled));
                }
            }
        }

        public int customerID
        {
            get { return _customerID; }
            set
            {
                if (_customerID != value)
                {
                    _customerID = value;
                    OnPropertyChanged(nameof(customerID));
                    OnPropertyChanged(nameof(FormattedCustomerID)); // Also notify that FormattedCustomerID has changed
                }
            }
        }

        // Read-only property dependent on customerID
        public string FormattedCustomerID => $"Customer #{customerID}";

        // Constructors
        public OrderedItem() { }

        public OrderedItem(int order_id, int item_id, string item_name, int quantity, double itemPrice, bool isSavedItem, int customerID, double originalItemPrice, bool isSettled)
        {
            this._order_id = order_id;
            this._item_id = item_id;
            this._item_name = item_name;
            this._quantity = quantity;
            this._itemPrice = itemPrice;
            this._isSavedItem = isSavedItem;
            this._customerID = customerID;
            this._originalItemPrice = originalItemPrice;
            _isSettled = isSettled;
        }
    }
}
