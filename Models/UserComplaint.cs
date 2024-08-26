using System;
using System.Collections.Generic;

namespace ComplaintBox.Models;

public partial class UserComplaint
{
    public string UserId { get; set; } = null!;

    public string ComplaintId { get; set; } = null!;

    public virtual Complaint Complaint { get; set; } = null!;
}
