using Microsoft.EntityFrameworkCore;
using WebApiAcademy.DTOs;
using WebApiAcademy.Models;
using WebApiAcademy.Services;

namespace WebApiAcademy
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Guid personId = Guid.NewGuid();
            Person newPerson = new()
            {
                Id = personId,
                Name = "Jose",
                LastName = "Palomino",
                Phone = "3152560926",
                CardId = "123456789"
            };
            Guid rolId = Guid.NewGuid();
            List<Rol> newRoles= new()
            {
                new Rol
                {
                    Id = rolId,
                    Name = "Administrador"
                },
                new Rol
                {
                    Id = Guid.NewGuid(),
                    Name = "Estudiante"
                },
                new Rol
                {
                    Id = Guid.NewGuid(),
                    Name = "Profesor"
                }
            };

            User newUser = new()
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                RolId = rolId,
                Password = AuthService.HashPassword("123456jD"),
                Email = "jpalomino31@gmail.com"
            };

            modelBuilder.Entity<Person>(person =>
            {
                person.Property(p => p.Id)
                      .HasDefaultValueSql("NEWID()");
                person.Property(p => p.CardId)
                      .HasMaxLength(50)
                      .IsRequired();
                person.Property(p => p.Name)
                      .HasMaxLength(50)
                      .IsRequired();
                person.Property(p => p.LastName)
                      .HasMaxLength(50)
                      .IsRequired();
                person.Property(p => p.Phone)
                      .HasMaxLength(50)
                      .IsRequired();
                person.HasMany(p => p.Users)
                      .WithOne(u => u.Person)
                      .HasForeignKey(u => u.PersonId);
                person.HasData(newPerson);
            });
            modelBuilder.Entity<Rol>(rol =>
            {
                rol.Property(r => r.Id)
                   .HasDefaultValueSql("NEWID()");
                rol.Property(r => r.Name)
                   .HasMaxLength(50)
                   .IsRequired();
                rol.HasData(newRoles);
            });


            modelBuilder.Entity<User>(user =>
            {
                user.Property(u => u.Id)
                    .HasDefaultValueSql("NEWID()");
                user.Property(u => u.Password)
                    .HasMaxLength(200)
                    .IsRequired();
                user.Property(u => u.Email)
                    .HasMaxLength(50)
                    .IsRequired();
                user.HasOne(u => u.Person)
                    .WithMany(p => p.Users)
                    .HasForeignKey(u => u.PersonId);
                user.HasOne(u => u.Rol)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RolId);
                user.HasData(newUser);
            });

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
