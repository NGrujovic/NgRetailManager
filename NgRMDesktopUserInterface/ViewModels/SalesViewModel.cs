using AutoMapper;
using Caliburn.Micro;
using NgRMDesktopUI.Library.Api;
using NgRMDesktopUI.Library.Helpers;
using NgRMDesktopUI.Library.Models;
using NgRMDesktopUserInterface.Models;
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
        private BindingList<ProductDisplayModel> _products;
        private int _itemQuantity = 1;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private IProductEndpoint _productEndpoint;
        private IConfigHelper _configHelper;
        private ISaleEndpoint _saleEndpoint;
        private IMapper _mapper;
        public  SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint
            ,IMapper mapper)
        {
            _mapper = mapper;
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        } 
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            // Mapping to a list of product display model using AutoMapper
            
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
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
            
            taxAmmount = Cart.Where(x=> x.Product.IsTaxable).Sum(x=> (x.Product.RetailPrice * x.QuantityInCart)*(taxRate/100));


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
    

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set {
                _products = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }

        

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set { _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        
        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private CartItemDisplayModel _selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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
                
                bool output = false;
                if (SelectedCartItem != null && SelectedCartItem?.Product.QuantityInStock > 0)
                {
                    output = true;

                }
                return output;
            }
        }

        public bool CanCheckout
        {
            get
            {
                //Make sure something is in cart
                bool output = false;
                if(_cart.Count > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if(existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                
                //Hack to trick the system to refresh list (temporary solution)
                //Cart.Remove(existingItem);
                //Cart.Add(existingItem);
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
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
            NotifyOfPropertyChange(() => CanCheckout);




        }

       

        public void RemoveFromCart()
        {

            


            SelectedCartItem.Product.QuantityInStock += 1;
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
                
            }
            else
            {
               
                Cart.Remove(SelectedCartItem);
            }

            
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => CanRemoveFromCart);
        }

        public async Task Checkout()
        {

            SaleModel sale = new SaleModel();
            foreach(var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
            //Restart cart after successful sale post

        }


    }
}
