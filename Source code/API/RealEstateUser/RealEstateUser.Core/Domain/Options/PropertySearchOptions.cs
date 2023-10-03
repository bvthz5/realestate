namespace RealEstateUser.Core.Domain.Options
{
    public class PropertySearchOptions
    {
        public int[] CategoryIds { get; set; } = Array.Empty<int>();
        public float StartPrice { get; set; }
        public float EndPrice { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool SortByDesc { get; set; }
        public string? Zipcode { get; set; }
        public byte[] Status { get; set; } = Array.Empty<byte>();
        public byte CategoryType { get; set; }
        public int TotalBedrooms { get; set; }
        public int TotalBathrooms { get; set; }
    }
}
