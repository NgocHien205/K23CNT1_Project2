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

        // ===================== TRANG CHỦ (hiển thị sản phẩm) =====================
        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm và danh mục
            var sanPhams = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .ToList();

            return View(sanPhams);
        }

        // ===================== TRANG CHI TIẾT SẢN PHẨM =====================
        public IActionResult ChiTiet(int id)
        {
            var sp = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .FirstOrDefault(s => s.MaSanPham == id);

            if (sp == null)
                return NotFound();

            return View(sp);
        }

        // ===================== TRANG PRIVACY + ERROR =====================
        public IActionResult Privacy()
        {
            return View();
        }

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
