using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace WebBanNoiThat.Models
{
    public partial class ChiTietDonHang
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MaCtdh { get; set; }
        public int MaDh { get; set; }
        public int MaSp { get; set; }

        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public  DonHang DonHang { get; set; }
        public  SanPham SanPham { get; set; }
    }
}
