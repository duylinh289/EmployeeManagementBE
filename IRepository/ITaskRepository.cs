using EmployeeManagementBE.DTO.Task;

namespace EmployeeManagementBE.IRepository
{
    public interface ITaskRepository
    {
        public Task<List<TaskAssignDTO>> GetAll();

        public Task<string> AssignToTask(TaskAssignDTO req);
    }
}
