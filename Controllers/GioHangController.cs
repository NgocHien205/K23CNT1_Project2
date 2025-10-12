using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDoDungNhaBep.Models;

namespace WebDoDungNhaBep.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ShopDoDungNhaBep02Context _context;

        public GioHangController(ShopDoDungNhaBep02Context context)
        {
            _context = context;
        }

        // =================== HIỂN THỊ GIỎ HÀNG ===================
        public IActionResult Index()
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            var vaiTro = HttpContext.Session.GetInt32("VaiTro");

            // Nếu chưa đăng nhập
            if (maAdmin == null)
            {
                TempData["ThongBao"] = "Vui lòng đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "GioHang") });
            }

            // Nếu là admin thì không có giỏ hàng
            if (vaiTro == 2)
            {
                TempData["ThongBao"] = "Admin không có giỏ hàng!";
                return RedirectToAction("Index", "Home");
            }

            // Lấy danh sách sản phẩm trong giỏ của user đang đăng nhập
            var gioHang = _context.GioHangs
                .Include(g => g.MaSanPhamNavigation)
                .Where(g => g.MaAdmin == maAdmin)
                .ToList();

            ViewBag.ThongBao = TempData["ThongBao"];
            return View(gioHang);
        }

        // =================== THÊM VÀO GIỎ ===================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemVaoGio(int maSanPham, int soLuong = 1, string returnUrl = null)
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            var vaiTro = HttpContext.Session.GetInt32("VaiTro");

            if (maAdmin == null)
            {
                TempData["ThongBao"] = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ!";
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl ?? Url.Action("Index", "Home") });
            }

            if (vaiTro == 2)
            {
                TempData["ThongBao"] = "Admin không thể thêm sản phẩm vào giỏ hàng!";
                return RedirectToAction("Index", "Home");
            }

            var sanPham = _context.SanPhams.FirstOrDefault(s => s.MaSanPham == maSanPham);
            if (sanPham == null)
                return NotFound("Không tìm thấy sản phẩm.");

            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            var gioHang = _context.GioHangs.FirstOrDefault(g => g.MaAdmin == maAdmin && g.MaSanPham == maSanPham);

            if (gioHang != null)
            {
                gioHang.SoLuong += soLuong;
                _context.Update(gioHang);
            }
            else
            {
                var item = new GioHang
                {
                    MaAdmin = maAdmin.Value,
                    MaSanPham = maSanPham,
                    SoLuong = soLuong,
                    NgayTao = DateTime.Now
                };
                _context.GioHangs.Add(item);
            }

            _context.SaveChanges();

            TempData["ThongBao"] = $"✅ Đã thêm {sanPham.TenSanPham} vào giỏ hàng!";
            return RedirectToAction("Index", "Home");
        }

        // =================== CẬP NHẬT SỐ LƯỢNG ===================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatSoLuong(int maSanPham, int soLuong)
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            if (maAdmin == null) return RedirectToAction("Login", "Account");

            var gioHang = _context.GioHangs.FirstOrDefault(g => g.MaAdmin == maAdmin && g.MaSanPham == maSanPham);
            if (gioHang != null)
            {
                gioHang.SoLuong = soLuong;
                _context.Update(gioHang);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // =================== XÓA KHỎI GIỎ ===================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaKhoiGio(int maSanPham)
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            if (maAdmin == null) return RedirectToAction("Login", "Account");

            var gioHang = _context.GioHangs.FirstOrDefault(g => g.MaAdmin == maAdmin && g.MaSanPham == maSanPham);
            if (gioHang != null)
            {
                _context.GioHangs.Remove(gioHang);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // =================== ĐẶT HÀNG ===================
        public IActionResult DatHang()
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            if (maAdmin == null)
            {
                TempData["ThongBao"] = "Vui lòng đăng nhập trước khi đặt hàng!";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "GioHang") });
            }

            var gioHang = _context.GioHangs
                .Include(g => g.MaSanPhamNavigation)
                .Where(g => g.MaAdmin == maAdmin)
                .ToList();

            if (!gioHang.Any())
            {
                TempData["ThongBao"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            // TODO: Thực hiện tạo hóa đơn (bảng DonHang hoặc tương tự)
            _context.GioHangs.RemoveRange(gioHang);
            _context.SaveChanges();

            TempData["ThongBao"] = "🎉 Đặt hàng thành công! Cảm ơn bạn đã mua sắm.";
            return RedirectToAction("Index", "Home");
        }
    }
}
