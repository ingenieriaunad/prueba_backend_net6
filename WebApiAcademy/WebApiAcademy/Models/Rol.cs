namespace WebApiAcademy.Models
{
    public class Rol
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
