using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgRMDesktopUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private int _itemQuantity;
        private BindingList<string> _cart;

        

        public string SubTotal
        {
            //Todo - Replace with calculation
            get { return "$0.00"; }
            
        }

        public string Tax
        {
            //Todo - Replace with calculation
            get { return "$0.00"; }
        }
        public string Total
        {
            //Todo - Replace with calculation
            get { return "$0.00"; }

        }
    

        public BindingList<string> Products
        {
            get { return _products; }
            set {
                _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }

        

        public BindingList<string> Cart
        {
            get { return _cart; }
            set { _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                //make sure something is selected
                //Make sure we have quantity put in


                return output;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                //Make sure something is selected to remove
                bool output = false;
                return output;
            }
        }

        public bool CanCheckout
        {
            get
            {
                //Make sure something is in cart
                bool output = false;
                return output;
            }
        }

        public void AddToCart()
        {

        }

       

        public void RemoveFromCart()
        {

        }

        public void Checkout()
        {

        }


    }
}
