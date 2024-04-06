namespace Domain.Entities
{
    public class Photo
    {
        public Photo() { }
        public Photo(int id, string base64)
        {
            Id = id;
            Base64 = base64;
        }
        public int Id { get; private set; }
        public string Base64 { get; set; }
    }
}
