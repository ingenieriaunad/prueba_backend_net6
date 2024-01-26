namespace WebApiAcademy.DTOs
{
    public class ListTable
    {
        public int TotalPages { get; set; } = 1;
        public int TotalRecords { get; set; } = 10;
        public List<object> Items { get; set; } = new List<object>();
    }
}
