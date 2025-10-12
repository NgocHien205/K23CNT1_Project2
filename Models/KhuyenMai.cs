using System;
using System.Collections.Generic;

namespace WebDoDungNhaBep.Models;

public partial class KhuyenMai
{
    public int MaKm { get; set; }

    public string TenKm { get; set; } = null!;

    public decimal GiamGia { get; set; }

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }
}
