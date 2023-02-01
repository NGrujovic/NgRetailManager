using Caliburn.Micro;
using NgRMDesktopUI.Library.Api;
using NgRMDesktopUI.Library.Helpers;
using NgRMDesktopUI.Library.Models;
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
        private BindingList<ProductModel> _products;
        private int _itemQuantity = 1;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private IProductEndpoint _productEndpoint;
        private IConfigHelper _configHelper;
        public  SalesViewModel(IProductEndpoint productEndpoint,IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;


        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        } 
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }
        
        public string SubTotal
        {
            
            get
            {
                
                return CalculateSubTotal().ToString("C");
            }

        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }

        public string Tax
        {
            
            get {
                return CalculateTaxRate().ToString("C");
            }
            
        }

        private decimal CalculateTaxRate()
        {
            decimal taxAmmount = 0;
            decimal taxRate = _configHelper.GetTaxRate();
            Cart.Where(x=> x.Product.IsTaxable).Sum(x=> (x.Product.RetailPrice * x.QuantityInCart)*(taxRate/100));
            
            
            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmmount += (item.Product.RetailPrice * item.QuantityInCart) * (taxRate / 100);
            //    }

            //}

            return taxAmmount;
        }
        public string Total
        {
            //Todo - Replace with calculation
            get {
                decimal total = CalculateSubTotal() + CalculateTaxRate();
                return total.ToString("C"); 
            }

        }
    

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set {
                _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }

        

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set { _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        
        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }



        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(()=>CanAddToCart);
            }
        }

        public bool CanAddToCart
        {
            get
            {

                bool output = false;
                //make sure something is selected
                //Make sure we have quantity put in
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;

                }


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
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if(existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                
                //Hack to trick the system to refresh list (temporary solution)
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);
            }
            
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);



        }

       

        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
        }

        public void Checkout()
        {

        }


    }
}
