namespace ComplaintBox.Models
{
    public class ComplaintInfo
    {
        public string ComplaintType { get; set; } = null!;

        public string? Description { get; set; }

        public string? StreetNo { get; set; }

        public string? BuildingNo { get; set; }

        public string PinCode { get; set; } = null!;

        public string? VictimName { get; set; }

        public int? VictimAge { get; set; }

        public string? VictimGender { get; set; }

        public byte[]? Images { get; set; }
    }
}
