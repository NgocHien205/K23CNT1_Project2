using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebDoDungNhaBep.Models;

namespace WebDoDungNhaBep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDoDungNhaBep02Context _context;

        public HomeController(ILogger<HomeController> logger, ShopDoDungNhaBep02Context context)
        {
            _logger = logger;
            _context = context;
        }

        // ===================== TRANG CHỦ (hiển thị danh sách sản phẩm) =====================
        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm, bao gồm thông tin danh mục
            var sanPhams = _context.SanPhams
                                   .Include(s => s.MaDanhMucNavigation)
                                   .ToList();

            return View(sanPhams);
        }


        // ===================== TÌM KIẾM =====================
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return RedirectToAction("Index");

            var ketQua = _context.SanPhams
                .Include(sp => sp.MaDanhMucNavigation)
                .Where(sp =>
                    sp.TenSanPham.Contains(keyword) ||
                    sp.MoTa.Contains(keyword) ||
                    sp.MaDanhMucNavigation.TenDanhMuc.Contains(keyword))
                .ToList();

            ViewBag.Keyword = keyword;
            return View("Index", ketQua);
        }

        // ===================== TRANG CHI TIẾT SẢN PHẨM =====================
        public IActionResult ChiTiet(int id)
        {
            var sp = _context.SanPhams
                             .Include(s => s.MaDanhMucNavigation)
                             .FirstOrDefault(s => s.MaSanPham == id);
            if (sp == null) return NotFound();
            return View(sp);
        }


        // ===================== TRANG PRIVACY =====================
        public IActionResult Privacy()
        {
            return View();
        }

        // ===================== TRANG ERROR =====================
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
