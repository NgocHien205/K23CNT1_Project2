using System;
using System.Collections.Generic;

namespace WebDoDungNhaBep.Models;

public partial class GioHang
{
    public int MaGioHang { get; set; }

    public int MaAdmin { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual Admin MaAdminNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
