using System;
using System.Collections.Generic;

namespace ComplaintBox.Models;

public partial class AreaInfo
{
    public string PinCode { get; set; } = null!;

    public string AreaName { get; set; } = null!;

    public string AdminId { get; set; } = null!;

    public virtual Admin Admin { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}
