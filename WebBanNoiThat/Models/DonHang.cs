
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebBanNoiThat.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
        [Key]
        public int MaDh { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }
      
        public string Address { get; set; }

        public DateTime NgayTao { get; set; }

        public decimal TotalAmount { get; set; }
        public int TypePayment { get; set; }
        public int Status { get; set; }

        //ForeignKeyAttribute (UserId) References AspNetUsers(Id)
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

    }
}
