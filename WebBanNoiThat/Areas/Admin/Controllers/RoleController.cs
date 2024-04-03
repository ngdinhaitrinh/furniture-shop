using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebBanNoiThat.Models;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace WebBanNoiThat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly BanNoiThatContext _context;
        public RoleController(BanNoiThatContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
     
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
  
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IdentityRole model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        //        roleManager.Create(model);
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}


    }

}
