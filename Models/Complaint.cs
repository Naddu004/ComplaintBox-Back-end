using System;
using System.Collections.Generic;

namespace ComplaintBox.Models;

public partial class Complaint
{
    public string ComplaintId { get; set; } = null!;

    public string ComplaintType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DateTimeLodged { get; set; }

    public string? ComplaintStatus { get; set; }

    public string? StreetNo { get; set; }

    public string? BuildingNo { get; set; }

    public string PinCode { get; set; } = null!;

    public string? VictimId { get; set; }

    public string AdminAllotedId { get; set; } = null!;

    public byte[]? Images { get; set; }

    public virtual Admin AdminAlloted { get; set; } = null!;

    public virtual AreaInfo PinCodeNavigation { get; set; } = null!;

    public virtual ICollection<UserComplaint> UserComplaints { get; set; } = new List<UserComplaint>();

    public virtual VictimInfo? Victim { get; set; }
}
