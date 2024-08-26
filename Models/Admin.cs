using System;
using System.Collections.Generic;

namespace ComplaintBox.Models;

public partial class Admin
{
    public string AdminId { get; set; } = null!;

    public string AdminPass { get; set; } = null!;

    public string AdminStatus { get; set; } = null!;

    public virtual ICollection<AreaInfo> AreaInfos { get; set; } = new List<AreaInfo>();

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}
