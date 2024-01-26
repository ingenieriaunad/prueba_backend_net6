namespace WebApiAcademy.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string CardId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public List<User> Users { get; set; }
        public List<Score> Scores { get; set; }
    }
}
