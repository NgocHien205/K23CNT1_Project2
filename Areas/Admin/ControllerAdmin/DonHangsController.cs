using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDoDungNhaBep.Areas.Admin.DTOs;
using WebDoDungNhaBep.Models;

namespace WebDoDungNhaBep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonHangsController : Controller
    {
        private readonly ShopDoDungNhaBep02Context _context;

        public DonHangsController(ShopDoDungNhaBep02Context context)
        {
            _context = context;
        }

        // GET: Admin/DonHangs
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/DonHangs/GetAllDonHangs - API for AJAX
        [HttpGet]
        public async Task<IActionResult> GetAllDonHangs()
        {
            try
            {
                var donHangs = await _context.DonHangs
                    .Include(d => d.MaAdminNavigation)
                    .Select(d => new DonHangDTO
                    {
                        MaDonHang = d.MaDonHang,
                        MaAdmin = d.MaAdmin,
                        NgayDat = d.NgayDat,
                        TongTien = d.TongTien,
                        TrangThai = d.TrangThai,
                        TenAdmin = d.MaAdminNavigation.HoTen
                    })
                    .ToListAsync();

                return Json(new ApiResponse<List<DonHangDTO>>
                {
                    Success = true,
                    Data = donHangs
                });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse<List<DonHangDTO>>
                {
                    Success = false,
                    Message = $"Lỗi khi tải dữ liệu: {ex.Message}"
                });
            }
        }

        // GET: Admin/DonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Json(new ApiResponse { Success = false, Message = "ID không hợp lệ" });
            }

            try
            {
                var donHang = await _context.DonHangs
                    .Include(d => d.MaAdminNavigation)
                    .FirstOrDefaultAsync(m => m.MaDonHang == id);

                if (donHang == null)
                {
                    return Json(new ApiResponse { Success = false, Message = "Không tìm thấy đơn hàng" });
                }

                var donHangDTO = new DonHangDTO
                {
                    MaDonHang = donHang.MaDonHang,
                    MaAdmin = donHang.MaAdmin,
                    NgayDat = donHang.NgayDat,
                    TongTien = donHang.TongTien,
                    TrangThai = donHang.TrangThai,
                    TenAdmin = donHang.MaAdminNavigation.HoTen
                };

                return Json(new ApiResponse<DonHangDTO> { Success = true, Data = donHangDTO });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        // GET: Admin/DonHangs/Create
        public IActionResult Create()
        {
            try
            {
                var admins = _context.Admins
                    .Select(a => new { a.MaAdmin, a.HoTen })
                    .ToList();

                return Json(new ApiResponse { Success = true, Data = admins });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: Admin/DonHangs/Create - AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateDonHangDTO donHangDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new ApiResponse { Success = false, Message = "Dữ liệu không hợp lệ", Data = errors });
            }

            try
            {
                var donHang = new DonHang
                {
                    MaAdmin = donHangDTO.MaAdmin,
                    NgayDat = donHangDTO.NgayDat,
                    TongTien = donHangDTO.TongTien,
                    TrangThai = donHangDTO.TrangThai
                };

                _context.Add(donHang);
                await _context.SaveChangesAsync();

                return Json(new ApiResponse { Success = true, Message = "Tạo đơn hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi khi tạo đơn hàng: {ex.Message}" });
            }
        }

        // GET: Admin/DonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Json(new ApiResponse { Success = false, Message = "ID không hợp lệ" });
            }

            try
            {
                var donHang = await _context.DonHangs.FindAsync(id);
                if (donHang == null)
                {
                    return Json(new ApiResponse { Success = false, Message = "Không tìm thấy đơn hàng" });
                }

                var donHangDTO = new UpdateDonHangDTO
                {
                    MaDonHang = donHang.MaDonHang,
                    MaAdmin = donHang.MaAdmin,
                    NgayDat = donHang.NgayDat,
                    TongTien = donHang.TongTien,
                    TrangThai = donHang.TrangThai
                };

                var admins = _context.Admins
                    .Select(a => new { a.MaAdmin, a.HoTen })
                    .ToList();

                return Json(new ApiResponse
                {
                    Success = true,
                    Data = new { DonHang = donHangDTO, Admins = admins }
                });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: Admin/DonHangs/Edit/5 - AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] UpdateDonHangDTO donHangDTO)
        {
            if (id != donHangDTO.MaDonHang)
            {
                return Json(new ApiResponse { Success = false, Message = "ID không khớp" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new ApiResponse { Success = false, Message = "Dữ liệu không hợp lệ", Data = errors });
            }

            try
            {
                var donHang = await _context.DonHangs.FindAsync(id);
                if (donHang == null)
                {
                    return Json(new ApiResponse { Success = false, Message = "Không tìm thấy đơn hàng" });
                }

                donHang.MaAdmin = donHangDTO.MaAdmin;
                donHang.NgayDat = donHangDTO.NgayDat;
                donHang.TongTien = donHangDTO.TongTien;
                donHang.TrangThai = donHangDTO.TrangThai;

                _context.Update(donHang);
                await _context.SaveChangesAsync();

                return Json(new ApiResponse { Success = true, Message = "Cập nhật đơn hàng thành công" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonHangExists(donHangDTO.MaDonHang))
                {
                    return Json(new ApiResponse { Success = false, Message = "Đơn hàng không tồn tại" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi khi cập nhật: {ex.Message}" });
            }
        }

        // GET: Admin/DonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new ApiResponse { Success = false, Message = "ID không hợp lệ" });
            }

            try
            {
                var donHang = await _context.DonHangs
                    .Include(d => d.MaAdminNavigation)
                    .FirstOrDefaultAsync(m => m.MaDonHang == id);

                if (donHang == null)
                {
                    return Json(new ApiResponse { Success = false, Message = "Không tìm thấy đơn hàng" });
                }

                var donHangDTO = new DonHangDTO
                {
                    MaDonHang = donHang.MaDonHang,
                    MaAdmin = donHang.MaAdmin,
                    NgayDat = donHang.NgayDat,
                    TongTien = donHang.TongTien,
                    TrangThai = donHang.TrangThai,
                    TenAdmin = donHang.MaAdminNavigation.HoTen
                };

                return Json(new ApiResponse<DonHangDTO> { Success = true, Data = donHangDTO });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: Admin/DonHangs/Delete/5 - AJAX
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var donHang = await _context.DonHangs.FindAsync(id);
                if (donHang != null)
                {
                    // Kiểm tra khóa ngoại trước khi xóa
                    var hasChiTiet = await _context.ChiTietDonHangs
                        .AnyAsync(ct => ct.MaDonHang == id);

                    if (hasChiTiet)
                    {
                        return Json(new ApiResponse
                        {
                            Success = false,
                            Message = "Không thể xóa đơn hàng vì có chi tiết đơn hàng liên quan"
                        });
                    }

                    _context.DonHangs.Remove(donHang);
                    await _context.SaveChangesAsync();
                }

                return Json(new ApiResponse { Success = true, Message = "Xóa đơn hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = $"Lỗi khi xóa: {ex.Message}" });
            }
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}