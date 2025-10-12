using System;
using System.Collections.Generic;

namespace WebDoDungNhaBep.Models;

public partial class ChiTietDonHang
{
    public int MaCtdh { get; set; }

    public int MaDonHang { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public decimal Gia { get; set; }

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
