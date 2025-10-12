using Microsoft.AspNetCore.Mvc;
using WebDoDungNhaBep.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace WebDoDungNhaBep.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopDoDungNhaBep02Context _context;

        public AccountController(ShopDoDungNhaBep02Context context)
        {
            _context = context;
        }

        // ========================= LOGIN (GET) =========================
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // ========================= LOGIN (POST) =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập và mật khẩu.";
                return View();
            }

            // Tìm tài khoản theo Email hoặc Tên đăng nhập
            var user = _context.Admins.FirstOrDefault(a =>
                (a.Email == username || a.TenDangNhap == username) &&
                a.MatKhau == password);

            if (user == null)
            {
                ViewBag.Error = "Thông tin đăng nhập không hợp lệ.";
                return View();
            }

            // ---------------- Lưu SESSION ----------------
            HttpContext.Session.SetInt32("MaAdmin", user.MaAdmin);
            HttpContext.Session.SetString("HoTen", user.HoTen);
            HttpContext.Session.SetString("TenDangNhap", user.TenDangNhap);
            HttpContext.Session.SetInt32("VaiTro", user.VaiTro);

            TempData["ThongBao"] = $"Xin chào {user.HoTen}! Đăng nhập thành công.";

            // ---------------- Điều hướng theo vai trò ----------------
            if (user.VaiTro == 2) // ✅ ADMIN
            {
                // Chuyển sang trang quản trị (Area Admin)
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if (user.VaiTro == 1) // ✅ USER
            {
                // Nếu có returnUrl (ví dụ đang ở giỏ hàng mà chưa đăng nhập) → quay lại
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                // Mặc định về trang người dùng
                return RedirectToAction("Index", "Home");
            }

            // Nếu không khớp vai trò nào, về trang chủ
            return RedirectToAction("Index", "Home");
        }

        // ========================= LOGOUT =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["ThongBao"] = "Bạn đã đăng xuất thành công!";
            return RedirectToAction("Index", "Home");
        }

        // ========================= REGISTER (GET) =========================
        public IActionResult Register()
        {
            return View();
        }

        // ========================= REGISTER (POST) =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string hoTen, string diaChi, string soDienThoai,
            string tenDangNhap, string email, string matKhau)
        {
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(tenDangNhap)
                || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                return View();
            }

            // Kiểm tra trùng tên đăng nhập hoặc email
            var exists = await _context.Admins.AnyAsync(a => a.TenDangNhap == tenDangNhap || a.Email == email);
            if (exists)
            {
                ViewBag.Error = "Tên đăng nhập hoặc email đã tồn tại.";
                return View();
            }

            var user = new Admin
            {
                HoTen = hoTen,
                DiaChi = diaChi,
                SoDienThoai = soDienThoai,
                TenDangNhap = tenDangNhap,
                Email = email,
                MatKhau = matKhau,
                VaiTro = 1, // ✅ User mặc định là 1
                TrangThai = "Hoạt động",
                NgayTao = DateTime.Now
            };

            _context.Admins.Add(user);
            await _context.SaveChangesAsync();

            // Lưu Session sau khi đăng ký
            HttpContext.Session.SetInt32("MaAdmin", user.MaAdmin);
            HttpContext.Session.SetString("HoTen", user.HoTen);
            HttpContext.Session.SetString("TenDangNhap", user.TenDangNhap);
            HttpContext.Session.SetInt32("VaiTro", user.VaiTro);

            TempData["ThongBao"] = "Đăng ký thành công! Chào mừng bạn đến với cửa hàng.";

            return RedirectToAction("Index", "Home");
        }

        // ========================= ACCESS DENIED =========================
        public IActionResult AccessDenied()
        {
            TempData["ThongBao"] = "Bạn không có quyền truy cập vào trang này!";
            return RedirectToAction("Index", "Home");
        }
    }
}
