using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using WebBanNoiThat.Models;

namespace WebBanNoiThat.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly BanNoiThatContext _context;

        public SanPhamController(BanNoiThatContext context)
        {
            _context = context;
          
        }
        public async Task<IActionResult> Index(string searchString)
        {

            var product = from m in _context.SanPhams
                         select m;
            var sp = _context.SanPhams.Include(b => b.MaDmNavigation); 
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                product = product.Where(b => b.TenSp.ToLower().Contains(searchString));
            }
            
            return View(await product.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        public async Task<IActionResult> ProductByCategory(int id)
        {
            //lấy all sản phẩm
            var items = _context.SanPhams.ToList();
            if (id > 0)
            {

                items = items.Where(x => x.MaDm == id).ToList();
            }
            var cate = _context.DanhMucSps.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.TenDm;
            }
            ViewBag.CateId = id;
            return View(items);
        }
        public IActionResult SortByPrice(string sortOrder)
        {
            var items = _context.SanPhams.ToList(); // Lấy danh sách sản phẩm từ nguồn dữ liệu

            // Mặc định sắp xếp từ thấp đến cao
            var sortedProducts = sortOrder switch
            {
                "desc" => items.OrderByDescending(p => p.GiaSp).ToList(),
                _ => items.OrderBy(p => p.GiaSp).ToList()
            };

            return View(sortedProducts);
        }
       

    }
}
