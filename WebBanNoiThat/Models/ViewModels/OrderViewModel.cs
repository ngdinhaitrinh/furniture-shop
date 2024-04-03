using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebBanNoiThat.Models.ViewModels
{
    public class OrderViewModel
    {
        public int MaDh { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không để trống")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ khổng để trống")]
        public string Address { get; set; }

        public int TypePayment { get; set; }
    }
   
}
