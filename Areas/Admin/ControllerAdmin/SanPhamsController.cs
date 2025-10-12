using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDoDungNhaBep.Models;

namespace WebDoDungNhaBep.Areas.Admin.ControllerAdmin
{
    [Area("Admin")]
    public class SanPhamsController : Controller
    {
        private readonly ShopDoDungNhaBep02Context _context;

        public SanPhamsController(ShopDoDungNhaBep02Context context)
        {
            _context = context;
        }

       





        // Hàm dùng chung để load danh mục + ảnh
        private void LoadDanhMucVaAnh(object? selectedDanhMuc = null, string? selectedImage = null)
        {
            // Danh mục
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", selectedDanhMuc);

            // Hình ảnh
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
            var images = Directory.Exists(imageFolder)
                ? Directory.GetFiles(imageFolder).Select(Path.GetFileName).ToList()
                : new List<string>();

            // Nếu trong DB lưu "/img/abc.jpg" thì ta cắt thành "abc.jpg" để select đúng option
            if (!string.IsNullOrEmpty(selectedImage))
            {
                selectedImage = selectedImage.Replace("/img/", "");
            }

            ViewData["HinhAnh"] = new SelectList(images, selectedImage);
        }

        //// GET: Admin/SanPhams
        //public async Task<IActionResult> Index()
        //{
        //    var shopDoDungNhaBep02Context = _context.SanPhams.Include(s => s.MaDanhMucNavigation);
        //    return View(await shopDoDungNhaBep02Context.ToListAsync());
        //}


        // GET: Admin/SanPhams
        public async Task<IActionResult> Index(string? keyword)
        {
            var query = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.TenSanPham.Contains(keyword));
            }

            var list = await query.ToListAsync();
            ViewData["Keyword"] = keyword;
            return View(list);
        }

        // GET: Admin/SanPhams/Search
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            return RedirectToAction(nameof(Index), new { keyword });
        }




           



        // GET: Admin/SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();  

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // GET: Admin/SanPhams/Create
        public IActionResult Create()
        {
            LoadDanhMucVaAnh();
            return View();
        }

        // POST: Admin/SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,Gia,SoLuong,MoTa,MaDanhMuc,HinhAnh")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                {
                    sanPham.HinhAnh = "/img/" + sanPham.HinhAnh;
                }

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDanhMucVaAnh(sanPham.MaDanhMuc, sanPham.HinhAnh);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null) return NotFound();

            LoadDanhMucVaAnh(sanPham.MaDanhMuc, sanPham.HinhAnh);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSanPham,TenSanPham,Gia,SoLuong,MoTa,MaDanhMuc,HinhAnh")] SanPham sanPham)
        {
            if (id != sanPham.MaSanPham) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                    {
                        sanPham.HinhAnh = "/img/" + sanPham.HinhAnh;
                    }

                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SanPhams.Any(e => e.MaSanPham == sanPham.MaSanPham))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            LoadDanhMucVaAnh(sanPham.MaDanhMuc, sanPham.HinhAnh);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSanPham == id);
        }
    }
}
