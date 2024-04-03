using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace WebBanNoiThat.Models
{
    public partial class DanhMucSp
    {
        public DanhMucSp()
        {
            SanPhams = new HashSet<SanPham>();
        }
        [Key]
        public int MaDm { get; set; }
        public string TenDm { get; set; }
        public string AnhDm { get; set; }
        public string MoTaDm { get; set; }
        public bool TrangThai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
