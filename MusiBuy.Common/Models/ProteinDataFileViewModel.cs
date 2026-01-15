using System;
using System.Collections.Generic;

namespace MusiBuy.Common.Models;

public partial class ProteinDataFileViewModel
{
    public int Id { get; set; }

    public int Createby { get; set; }

    public DateOnly Createdon { get; set; }

    public string FileName { get; set; } = null!;
}
