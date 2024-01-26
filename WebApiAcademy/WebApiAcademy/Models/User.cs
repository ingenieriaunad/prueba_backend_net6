using System;

namespace WebApiAcademy.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid RolId { get; set; }
        public Person Person { get; set; } 
        public string Password { get; set; } 
        public string Email { get; set; } 
        public Rol Rol { get; set; }
    }
}
