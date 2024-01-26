using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiAcademy.DTOs;
using WebApiAcademy.Models;
using WebApiAcademy.Utils;

namespace WebApiAcademy.Services
{
    public class StudentService: IStudentService
    {
        protected readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StudentService(ApplicationDbContext context,IMapper mapper )
        {
            _context = context;
            _mapper  = mapper;
        }
        public async Task<ListTable> GetPaginated(Pagination pagination)
        {
            var queryable = _context.Users.AsQueryable();
            if(!string.IsNullOrEmpty(pagination.Search))
            {
                queryable = queryable.Where(x => x.Person.Name.Contains(pagination.Search) || x.Person.CardId.Contains(pagination.Search));
            }
            queryable = queryable.Include(x => x.Rol)
                                 .Include(x => x.Person);
            queryable = queryable.Where(x => x.Person.Users
                                 .Any(x => x.Rol.Name == "Estudiante"));
            var students     = await queryable.GetPaged(pagination).ToListAsync();
            var totalRecords = await queryable.CountAsync();
            var studentDTO  = _mapper.Map<List<Student>>(students);
            return new ListTable
            {
                Items = studentDTO.Cast<object>().ToList(),
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pagination.PerPage)

            };
        }
        public async Task<Student> GetById(Guid id)
        {
            var student = await _context.Persons.Include(x => x.Users)
                                                .ThenInclude(x => x.Rol)
                                                .FirstOrDefaultAsync(x => x.Id == id);
            if (student == null) return null;
            var studentDTO = _mapper.Map<Student>(student);
            return studentDTO;
        }
    }

    public interface IStudentService
    {
        Task<ListTable> GetPaginated(Pagination pagination);
        Task<Student> GetById(Guid id);
    }
}
