using System.Collections;
using System.Collections.Generic;
using WebBanNoiThat.Models;
namespace WebBanNoiThat.Repository
{
    public interface ILoaiSpRepository
    {
        DanhMucSp Add(DanhMucSp loaisp);
        DanhMucSp Update(DanhMucSp loaisp);
        DanhMucSp Delete(string MaDm);
        DanhMucSp GetDanhMucSp(string MaDm);
        IEnumerable<DanhMucSp> GetAllDanhMucSp();
    }
}
