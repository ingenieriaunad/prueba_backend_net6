using Microsoft.EntityFrameworkCore;
using WebApiAcademy.Models;

namespace WebApiAcademy.Services
{
    public class RolService: IRolService
    {
        protected readonly ApplicationDbContext _context;

        public RolService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Save(Rol rol)
        {
            _context.Rols.Add(rol);
            await _context.SaveChangesAsync();
        }
        public async Task<Rol> GetByName(string name)
        {
            var rol = await _context.Rols.FirstOrDefaultAsync(x => x.Name == name);
            return rol;
        }
    }

    public interface IRolService
    {
        Task Save(Rol rol);
        Task<Rol> GetByName(string name);
    }
}
