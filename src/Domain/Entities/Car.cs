using Domain.Enums;

namespace Domain.Entities
{
    public class Car
    {
        public Car() { }
        public Car(int id, int photoId, string name, int status, Photo photo)
        {
            Id = id;
            Name = name;
            Status = status;
            Photo = photo;
        }
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Status { get; private set; } = 0;
        public string? StatusDescription => Enum.GetName(typeof(EnumStatus), Status);
        public Photo Photo { get; set; }
    }
}
