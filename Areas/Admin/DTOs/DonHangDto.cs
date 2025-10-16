using System;
using System.ComponentModel.DataAnnotations;

namespace WebDoDungNhaBep.Areas.Admin.DTOs
{
    public class DonHangDTO
    {
        public int MaDonHang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Admin")]
        [Display(Name = "Admin")]
        public int MaAdmin { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày đặt")]
        [Display(Name = "Ngày đặt")]
        public DateTime? NgayDat { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tổng tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 0")]
        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        [Display(Name = "Trạng thái")]
        public string? TrangThai { get; set; }

        // Thông tin thêm từ navigation properties
        public string? TenAdmin { get; set; }
    }

    public class CreateDonHangDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn Admin")]
        public int MaAdmin { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày đặt")]
        public DateTime? NgayDat { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tổng tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 0")]
        public decimal TongTien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string? TrangThai { get; set; }
    }

    public class UpdateDonHangDTO
    {
        public int MaDonHang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Admin")]
        public int MaAdmin { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày đặt")]
        public DateTime? NgayDat { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tổng tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 0")]
        public decimal TongTien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string? TrangThai { get; set; }
    }
}