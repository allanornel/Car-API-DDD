namespace Domain.Entities
{
    public class Car
    {
        public Car() { }
        public Car(int id, int photoId, string name, int status, Photo photo)
        {
            Id = id;
            PhotoId = photoId;
            Name = name;
            Status = status;
            Photo = photo;
        }
        public int Id { get; private set; }
        public int PhotoId { get; private set; }
        public string Name { get; set; }
        public int Status { get; private set; } = 0;
        public Photo Photo { get; set; }
    }
}
