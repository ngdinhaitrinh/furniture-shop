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
using System;
using System.Net.Mail;
using System.Reflection.Metadata;

namespace WebBanNoiThat.Controllers
{
    public class AccountController : Controller
    {
        private readonly BanNoiThatContext _context;
        private readonly MailService _emailService;
        public AccountController(BanNoiThatContext context, MailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var taikhoan_ = _context.Accounts.FirstOrDefault(x => x.Username == model.UserName && x.Password == model.Password);


            if (taikhoan_ == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác");
                return View();
            }
            // lucs nầy lấy thông tin đẻ lưu côkie
            var clamis = new List<Claim>
            {
                new Claim(ClaimTypes.Name ,taikhoan_.Email),
                new Claim("Username", taikhoan_.Username),

            };
            foreach (var claim in clamis)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            var claimsIdentity = new ClaimsIdentity(
                clamis, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {

            if (ModelState.IsValid)
            {
                var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email);

                if (existingAccount != null)
                {
                    ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
                    return View(model);
                }
                var account = new Account
                {
                    Username = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = false,
                    Status = true
                };
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                // Tạo mã xác nhận email ngẫu nhiên
                var emailConfirmationToken = GenerateRandomToken();
                // Lưu mã xác nhận email vào cơ sở dữ liệu
                account.EmailConfirmationToken = emailConfirmationToken;
                await _context.SaveChangesAsync();
                // Create the email confirmation link
                var emailConfirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = account.AccountId, code = emailConfirmationToken }, Request.Scheme);

                // Send the email confirmation email
                _emailService.SendMail(new List<string> { model.Email }, "Confirm Your Email", $"Please confirm your email by clicking <a href='{emailConfirmationLink}'>here</a>.");

                return RedirectToAction("Login", "Account");

            }
            return View(model);

        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(int userId, string code)
        {

            // Lấy thông tin người dùng từ cơ sở dữ liệu của bạn dựa trên userId
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.AccountId == userId);

            if (user == null)
            {
                return View("Error");
            }

            // Kiểm tra xem mã xác nhận có khớp với mã trong cơ sở dữ liệu hay không
            if (code != user.EmailConfirmationToken)
            {
                return View("Error");
            }

            // Đánh dấu tài khoản là đã xác nhận email
            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;

            // Cập nhật thông tin người dùng trong cơ sở dữ liệu
            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();

            return View("ConfirmEmail");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private string GenerateRandomToken()
        {
            const int tokenLength = 32;
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var token = new char[tokenLength];

            for (int i = 0; i < tokenLength; i++)
            {
                token[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            return new string(token);
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu hay không
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return View("Error");
            }

            // Tạo mật khẩu mới ngẫu nhiên
            string newPassword = GenerateRandomPassword();

            // Cập nhật mật khẩu mới cho người dùng trong cơ sở dữ liệu
            user.Password = newPassword;
            await _context.SaveChangesAsync();

            // Gửi email chứa mật khẩu mới đến người dùng
            _emailService.SendMail(new List<string> { email }, "New Password", $"Your new password: {newPassword}");

            return RedirectToAction("Login", "Account");
        }
        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var newPassword = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return newPassword;
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{


        //    var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "Username");

        //    if (userEmailClaim == null)
        //    {
        //        return BadRequest("Không tìm thấy thông tin người dùng");
        //    }

        //    var userEmail = userEmailClaim.Value;
        //    var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == userEmail);

        //    if (user == null)
        //    {
        //        return BadRequest("Người dùng không tồn tại");
        //    }

        //    var currentPassword = model.CurrentPassword;

        //    if (user.Password != currentPassword)
        //    {
        //        return BadRequest("Mật khẩu cũ không chính xác");
        //    }

        //    if (model.NewPassword.Length < 6 || model.NewPassword.Length > 100)
        //    {
        //        return BadRequest("Mật khẩu phải có từ 6 đến 100 ký tự");
        //    }

        //    if (model.NewPassword != model.ConfirmNewPassword)
        //    {
        //        return BadRequest("Mật khẩu mới và mật khẩu xác nhận không trùng khớp");
        //    }

        //    user.Password = model.NewPassword;
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index", "Home");
        //}

    }
}
