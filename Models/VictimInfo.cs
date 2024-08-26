using System;
using System.Collections.Generic;

namespace ComplaintBox.Models;

public partial class VictimInfo
{
    public string VictimId { get; set; } = null!;

    public string? VictimName { get; set; }

    public int? VictimAge { get; set; }

    public string? VictimGender { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}
