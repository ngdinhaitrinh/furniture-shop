using WebBanNoiThat.Models;
using Microsoft.AspNetCore.Mvc;
using WebBanNoiThat.Repository;
using System.Linq;

namespace WebBanNoiThat.Components
{
    public class DanhMucSpViewComponent : ViewComponent
    {
        private readonly ILoaiSpRepository _loaiSp;
        public DanhMucSpViewComponent(ILoaiSpRepository loaiSpRepository)
        {
            _loaiSp = loaiSpRepository;
        }
        public IViewComponentResult Invoke()
        {
            var loaisp = _loaiSp.GetAllDanhMucSp().OrderBy(x => x.TenDm);
            return View(loaisp);
        }
    }
}
