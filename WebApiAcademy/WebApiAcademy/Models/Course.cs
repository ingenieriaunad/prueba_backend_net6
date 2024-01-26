namespace WebApiAcademy.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
