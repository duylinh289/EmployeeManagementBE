using AutoMapper;
using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Task;
using EmployeeManagementBE.IRepository;
using Microsoft.EntityFrameworkCore;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.Migrations;
using System.Threading.Tasks;

namespace EmployeeManagementBE.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public TaskRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> AssignToTask(TaskAssignDTO req)
        {
            TaskList task = await _context.TaskList.Where(x => x.Id == req.Id).FirstOrDefaultAsync();
            if (task != null)
            {
                task.Assignee = req.Assignee;
                task.Reporter = req.Reporter;

                await _context.SaveChangesAsync();
                return "Success";
            }
            else
            {
                return "Not found";
            }
        }

        public async Task<List<TaskAssignDTO>> GetAll()
        {
            List<TaskAssignDTO> listTask = await _context.TaskList
                            .GroupJoin(_context.Employees
                                        , task => task.Assignee
                                        , emp => emp.EmployeeCode
                                        , (t, e) => new { Tk = t, Emp = e })
                            .SelectMany(
                               a => a.Emp.DefaultIfEmpty(),
                               (x,y) => new TaskAssignDTO
                               {
                                  Id = x.Tk.Id,
                                  TaskName = x.Tk.TaskName,
                                  TaskDescription = x.Tk.TaskDescription,
                                  Assignee = x.Tk.Assignee,
                                  AssigneeName = y != null ? y.EmployeeName : null,
                                  Reporter = x.Tk.Reporter
                               }
                            ).ToListAsync();

            return listTask;
        }
    }
}
