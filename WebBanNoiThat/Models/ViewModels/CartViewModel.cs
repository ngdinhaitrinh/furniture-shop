using System.Collections.Generic;
using System.Linq;

namespace WebBanNoiThat.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TongTien { get; set; }
        public CartViewModel() {
            this.CartItems = new List<CartItemViewModel>();
       }
        public void ClearCart()
        {
            CartItems.Clear();
        }

    }
    public class CartItemViewModel
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public string AnhSp { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaSp { get; set; }
        public decimal TongTien { get; set; }

       
    }

}
