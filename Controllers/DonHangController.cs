using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDoDungNhaBep.Models;

namespace WebDoDungNhaBep.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ShopDoDungNhaBep02Context _context;

        public DonHangController(ShopDoDungNhaBep02Context context)
        {
            _context = context;
        }

        // ===================== DANH SÁCH ĐƠN HÀNG =====================
        public IActionResult Index()
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            if (maAdmin == null)
                return RedirectToAction("Login", "Account");

            var donHangs = _context.DonHangs
                .Where(d => d.MaAdmin == maAdmin) // ✅ chỉ lấy đơn hàng của admin hiện tại
                .Include(d => d.MaAdminNavigation)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSanPhamNavigation)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MuaNgay(int maSanPham, int soLuong)
        {
            var maAdmin = HttpContext.Session.GetInt32("MaAdmin");
            if (maAdmin == null)
                return RedirectToAction("Login", "Account");

            // Lấy sản phẩm
            var sanPham = _context.SanPhams.FirstOrDefault(sp => sp.MaSanPham == maSanPham);
            if (sanPham == null)
                return NotFound();

            // Tạo đơn hàng mới
            var donHang = new DonHang
            {
                MaAdmin = maAdmin.Value,
                NgayDat = DateTime.Now,
                TongTien = sanPham.Gia * soLuong
            };
            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            // Tạo chi tiết đơn hàng
            var chiTiet = new ChiTietDonHang
            {
                MaDonHang = donHang.MaDonHang,
                MaSanPham = maSanPham,
                SoLuong = soLuong,
                Gia = sanPham.Gia
            };
            _context.ChiTietDonHangs.Add(chiTiet);
            _context.SaveChanges();

            TempData["ThongBao"] = "Mua hàng thành công!";
            return RedirectToAction("Index");
        }





        // ===================== CHI TIẾT =====================
        public IActionResult ChiTietDonHang(int id)
        {
            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSanPhamNavigation) // load sản phẩm trong chi tiết
                .Include(d => d.MaAdminNavigation) // load thông tin admin quản lý đơn
                .FirstOrDefault(d => d.MaDonHang == id);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }


        // ===================== TẠO MỚI =====================
        public IActionResult Create()
        {
            ViewBag.Admins = _context.Admins.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                donHang.NgayDat = DateTime.Now;
                _context.Add(donHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Admins = _context.Admins.ToList();
            return View(donHang);
        }

        // ===================== SỬA =====================
        public IActionResult Edit(int id)
        {
            var donHang = _context.DonHangs.Find(id);
            if (donHang == null)
                return NotFound();

            ViewBag.Admins = _context.Admins.ToList();
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DonHang donHang)
        {
            if (id != donHang.MaDonHang)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(donHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Admins = _context.Admins.ToList();
            return View(donHang);
        }

        // ===================== XÓA =====================
        public IActionResult Delete(int id)
        {
            var donHang = _context.DonHangs
                .Include(d => d.MaAdminNavigation)
                .FirstOrDefault(d => d.MaDonHang == id);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var donHang = _context.DonHangs.Find(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
