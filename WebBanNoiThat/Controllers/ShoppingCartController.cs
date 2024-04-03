using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanNoiThat.Models;
using WebBanNoiThat.Models.ViewModels;
using WebBanNoiThat.Repository;
using System.Net.Mail;
namespace WebBanNoiThat.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MailService _emailService;
        private readonly BanNoiThatContext _context;
     
        public ShoppingCartController(BanNoiThatContext context, MailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

      
        public IActionResult Index()
        {
            // Lấy thông tin giỏ hàng từ cơ sở dữ liệu hoặc từ nơi lưu trữ khác
            List<CartItemViewModel> cartItems = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();


            return View(cartItems);
        }
        public async Task<IActionResult> AddToCart(int Id, int quantity)
        {

            SanPham sanPham = await _context.SanPhams.FindAsync(Id);

            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var checkProduct = cart.Where(c => c.MaSp == Id).FirstOrDefault();

            if (checkProduct != null)
            {
                checkProduct.SoLuong += quantity;
                checkProduct.TongTien = checkProduct.SoLuong * checkProduct.GiaSp;
            }
            else
            {
                CartItemViewModel cartItem = new CartItemViewModel
                {
                    MaSp = Id,
                    TenSp = sanPham.TenSp,
                    GiaSp = sanPham.GiaSp,
                    SoLuong = quantity,
                    TongTien = quantity * sanPham.GiaSp
                };

                cart.Add(cartItem);
            }

            HttpContext.Session.SetJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        private string GetUserEmail()
        {
            // Lấy thông tin người dùng hiện tại từ cơ sở dữ liệu
            var currentUser = _context.Accounts.FirstOrDefault(); // Lấy người dùng đầu tiên trong bảng Account (cần chỉnh sửa truy vấn theo nhu cầu của bạn)

            if (currentUser != null)
            {
                return currentUser.Email; // Trả về địa chỉ email của người dùng
            }

            return null; // Trả về null nếu không tìm thấy địa chỉ email
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        [Route("thanh-toan")]

        public IActionResult CheckOut()
        {

            CartViewModel cartViewModel = new CartViewModel();
            cartViewModel.CartItems = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            ViewBag.CheckCart = cartViewModel;
            OrderViewModel orderViewModel = new OrderViewModel();
            return View(orderViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> CheckOutADD( OrderViewModel req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //List<CartViewModel> cartList = HttpContext.Session.GetJson<List<CartViewModel>>("Cart");
            //CartViewModel cart = cartList?.FirstOrDefault();
            List<CartItemViewModel> cartList = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            if (ModelState.IsValid)
            {
                if (cartList?.Count > 0)
                {
                    // Lưu thông tin giao hàng vào cơ sở dữ liệu
                    DonHang order = new DonHang
                    {

                        CustomerName = req.CustomerName,
                        Phone = req.Phone,
                        Address = req.Address,
                        TypePayment = req.TypePayment,
                        TotalAmount = cartList.Sum(item => item.GiaSp * item.SoLuong),
                        NgayTao = DateTime.Now,
                        Status = 1,
                        AccountId = 1 // Gán UserId từ ViewModel

                    };

                    _context.DonHangs.Add(order);
                    _context.SaveChanges();
                    foreach (var cartItem in cartList)
                    {
                        ChiTietDonHang orderDetail = new ChiTietDonHang
                        {
                            MaDh = order.MaDh,
                            MaSp = cartItem.MaSp,
                            SoLuong = cartItem.SoLuong,
                            Gia = cartItem.GiaSp
                        };
                        _context.ChiTietDonHangs.Add(orderDetail);
                        SanPham sanPham = _context.SanPhams.Find(cartItem.MaSp);
                        if (sanPham != null)
                        {
                            sanPham.ChiTietDonHangs.Add(orderDetail);
                        }
                    }
                    _context.SaveChanges();

                    string toEmail = "ngdinhaitrinh@gmail.com";
                    string subject = "Checkout Success";
                    string content = "Thank you for your purchase!";
                    _emailService.SendMail(new List<string> { toEmail }, subject, content);

                    HttpContext.Session.Remove("Cart");
                    return RedirectToAction("CheckOutSuccess");

                }
            }


            return View(req);

        }
        public async Task<IActionResult> Increase(int Id)
        {
            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");
            CartItemViewModel cartItems = cart.Where(c => c.MaSp == Id).FirstOrDefault();
            if (cartItems.SoLuong >= 1)
            {
                ++cartItems.SoLuong;
            }
            else
            {
                cart.RemoveAll(p => p.MaSp == Id);
            }


            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");
            CartItemViewModel cartItems = cart.Where(c => c.MaSp == Id).FirstOrDefault();
            if (cartItems.SoLuong > 1)
            {
                --cartItems.SoLuong;
            }
            else
            {
                cart.RemoveAll(p => p.MaSp == Id);
            }


            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int Id)
        {

            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");
            cart.RemoveAll(p => p.MaSp == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        
    }
}

