using WebApiAcademy.Models;

namespace WebApiAcademy.Services
{
    public class PersonService: IPersonService
    {
        protected readonly ApplicationDbContext _context;

        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Save(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }
        public async Task<Person> GetById(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            return person;
        }
        public async Task Update(Person person)
        {
            var currentPerson = await _context.Persons.FindAsync(person.Id) 
                                ?? throw new Exception("Person not found");
            currentPerson.Name     = person.Name;
            currentPerson.LastName = person.LastName;
            currentPerson.CardId   = person.CardId;
            currentPerson.Phone    = person.Phone;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var person = await _context.Persons.FindAsync(id) 
                        ?? throw new Exception("Person not found");
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
    public interface IPersonService
    {
        Task Save(Person person);
        Task<Person> GetById(Guid id);
        Task Update(Person person);
        Task Delete(Guid id);

    }
}
