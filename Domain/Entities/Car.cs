namespace Domain.Entities
{
    public class Car
    {
        public Car() { }
        public Car(int id, int photoId, string name, string status, Photo photo)
        {
            Id = id;
            PhotoId = photoId;
            Name = name;
            Status = status;
            Photo = photo;
        }
        public int Id { get; private set; }
        public int PhotoId { get; private set; }
        public string Name { get; private set; }
        public string Status { get; private set; }
        public Photo Photo { get; set; }

        public bool IsValid => Validate();

        private bool Validate()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Status);
        }
    }
}
