using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanNoiThat.Models;
using WebBanNoiThat.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebBanNoiThat.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="Admin")]
    public class AdminAccountController : Controller
    {
        private readonly BanNoiThatContext _context;

        public AdminAccountController(BanNoiThatContext context)
        {
            _context = context;
        }
        [Route("AdminAccount")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        public IActionResult Login()
        {
            return View();
        } 
        
        public IActionResult LoginA(LoginViewModel model)
        {
            var taikhoan_ = _context.Accounts.FirstOrDefault(x=>x.Username == model.UserName && x.Password == model.Password && x.Role == true);

            if (taikhoan_ == null)
            {
                return View("Tài khoản hoặc mật khẩu không chính xác");
            }
            // lucs nầy lấy thông tin đẻ lưu côkie
            var clamis = new List<Claim>
            { 
                new Claim(ClaimTypes.Name ,taikhoan_.Fullname),
                new Claim(ClaimTypes.Role ,"Admin"),
            };
            var claimsIdentity = new ClaimsIdentity(
                clamis, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
            );
            return RedirectToAction("Index","Home");
        }


         public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
    }

}

