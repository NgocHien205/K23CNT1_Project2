using System;
using System.Collections.Generic;

namespace WebDoDungNhaBep.Models;

public partial class DanhGium
{
    public int MaDanhGia { get; set; }

    public int MaAdmin { get; set; }

    public int MaSanPham { get; set; }

    public int? SoSao { get; set; }

    public string? BinhLuan { get; set; }

    public DateTime? NgayDanhGia { get; set; }

    public virtual Admin MaAdminNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
