using System.Collections.Generic;
using WebBanNoiThat.Models;
namespace WebBanNoiThat.Repository
{
    public class DanhMucSpRepository : ILoaiSpRepository
    {
        private readonly BanNoiThatContext _context;
        public DanhMucSpRepository(BanNoiThatContext context)
        {
            _context = context;

        }
        public DanhMucSp Add(DanhMucSp loaisp)
        {
            _context.DanhMucSps.Add(loaisp);
            _context.SaveChanges();
            return loaisp;
        }

        public DanhMucSp Delete(string MaDm)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<DanhMucSp> GetAllDanhMucSp()
        {

            return _context.DanhMucSps;
        }

        public DanhMucSp GetDanhMucSp(string MaDm)
        {
            return _context.DanhMucSps.Find(MaDm);
        }

        public DanhMucSp Update(DanhMucSp loaisp)
        {
            _context.Update(loaisp);
            _context.SaveChanges(true);
            return loaisp;
        }
    }
}
