using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebBanNoiThat.Models;

namespace WebBanNoiThat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly BanNoiThatContext _context;
        public OrderController(BanNoiThatContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var orders = from m in _context.DonHangs
                         select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(d => d.CustomerName.ToLower().Contains(searchString) || d.Phone.Contains(searchString) || d.Address.Contains(searchString));
            }
            return View(await orders.ToListAsync());
        }
        public ActionResult Details(int id)
        {
            var item = _context.DonHangs.Find(id);
            return View(item);
        }


    }
}
