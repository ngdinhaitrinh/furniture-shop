using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using WebBanNoiThat.Models;
using System.Linq;
namespace WebBanNoiThat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly BanNoiThatContext _context;

        public HomeController(BanNoiThatContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
           
        }
        public IActionResult DoanhThuThang(DateTime? targetDate, DateTime? targetMonth, int? selectedYear)
        {
            if (targetDate.HasValue)
            {
                // Lấy ngày đầu tiên của ngày được chọn
                DateTime startOfDay = targetDate.Value.Date;
                DateTime startOfNextDay = startOfDay.AddDays(1);
                decimal totalRevenue = CalculateDailyRevenue(startOfDay, startOfNextDay);


                ViewBag.TotalRevenue = totalRevenue;
                ViewBag.TargetDate = targetDate.Value.ToString("dd/MM/yyyy");
            }

            if (targetMonth.HasValue)
            {
                // Lấy ngày đầu tiên của tháng được chọn 
                DateTime firstDayOfMonth = new DateTime(targetMonth.Value.Year, targetMonth.Value.Month, 1);
                DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
                decimal totalMonthRevenue = CalculateMonthlyRevenue(firstDayOfMonth, firstDayOfNextMonth);


                ViewBag.TotalMonthRevenue = totalMonthRevenue;
                ViewBag.TargetMonth = targetMonth.Value.ToString("MM/yyyy");
            }
            if(selectedYear.HasValue)
            {
                int currentYear = DateTime.Now.Year;
                int year = selectedYear ?? currentYear;
                decimal totalYearRevenue = CalculateYearlyRevenue(year);

                ViewBag.TotalYearRevenue = totalYearRevenue;
                ViewBag.SelectedYear = year;
            }

            return View("Index");
        }
        private decimal CalculateDailyRevenue(DateTime startDate, DateTime endDate)
        {
            var orders = _context.DonHangs
                .Where(d => d.NgayTao >= startDate && d.NgayTao < endDate)
                .ToList();

            return orders.Sum(d => d.TotalAmount);
        }
        private decimal CalculateMonthlyRevenue(DateTime startDate, DateTime endDate)
        {
            var orders = _context.DonHangs
                .Where(d => d.NgayTao >= startDate && d.NgayTao < endDate)
                .ToList();

            return orders.Sum(d => d.TotalAmount);
        }
        private decimal CalculateYearlyRevenue(int year)
        {
            DateTime startOfYear = new DateTime(year, 1, 1);
            DateTime endOfYear = startOfYear.AddYears(1).AddSeconds(-1);

            decimal totalYearRevenue = _context.DonHangs
                .Where(d => d.NgayTao >= startOfYear && d.NgayTao <= endOfYear)
                .Sum(d => d.TotalAmount);

            return totalYearRevenue;
        }

    }
}
