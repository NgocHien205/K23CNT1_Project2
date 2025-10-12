using System;
using System.Collections.Generic;

namespace WebDoDungNhaBep.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public decimal Gia { get; set; }

    public int SoLuong { get; set; }

    public string? MoTa { get; set; }

    public int? MaDanhMuc { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual DanhMuc? MaDanhMucNavigation { get; set; }
}
