using Microsoft.EntityFrameworkCore;
using WebApiAcademy.Models;


namespace WebApiAcademy.Services
{
    public class UserService: IUserService
    {
        protected readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email)
        {
            var user = _context.Users
                        .Include(u => u.Person)
                        .FirstOrDefault(u => u.Email == email);
            return user;
        }
        public Task<User> GetByPersonId(Guid personId)
        {
            var user = _context.Users
                        .Include(u => u.Person)
                        .FirstOrDefaultAsync(u => u.PersonId == personId);
            return user;
        }
        public async Task Save(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task Update(User user)
        {
            var currentUser = _context.Users.Find(user.Id) 
                              ?? throw new Exception("User not found");
            if(user.Password != null)
            {
                currentUser.Password = user.Password;
            }
            currentUser.Email    = user.Email;
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Guid id)
        {
            var user = _context.Users.Find(id) 
                       ?? throw new Exception("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
    }
    public interface IUserService
    {
        User? GetByEmail(string email);
        Task<User> GetByPersonId(Guid personId);
        Task Save(User user);
        Task Update(User user);
        Task Delete(Guid id);
        Task<User> GetById(Guid id);
    }
}
