namespace WebApiAcademy.Models
{
    public class Score
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public float ScoreValue { get; set; }
        public Person Student { get; set; }
        public Course Course { get; set; }
    }
}
