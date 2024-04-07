namespace Application.Models
{
    public record CarModel(
     int Id,
     string Name,
     string PhotoBase64
    );

    public class PaginationModel<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
