namespace WebApiAcademy.DTOs
{
    public class Student
    {
        public Guid Id { get; set; }
        public Guid RolId { get; set; }
        public Guid PersonId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string CardId { get; set; }
    }
}
