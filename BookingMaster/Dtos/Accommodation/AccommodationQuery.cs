namespace BookingMaster.Dtos.Accommodation
{
    public class AccommodationQuery
    {
        public string? Name { get; set; }

        public string? City { get; set; }

        public int? PropertyTypeId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; } = false;
    }
}
