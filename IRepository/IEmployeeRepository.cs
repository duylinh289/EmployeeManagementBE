using EmployeeManagementBE.DTO.Employee;

namespace EmployeeManagementBE.IRepository
{
    public interface IEmployeeRepository
    {
        public Task<List<EmployeeDTO>> GetAll();
        public Task<EmployeeDTO> GetById(Guid req);
        public Task<Guid> Create(CreateUpdateEmployeeDTO req, string userCreate);
        public Task<string> Update(CreateUpdateEmployeeDTO req, string userModify);
        public Task<string> Delete(Guid req, string userDelete);
        public Task<List<EmployeeDTO>> Search(string search);
        public Task<List<ImportEmployeeDTO>> CheckDataImport(List<ImportEmployeeDTO> req);
        public Task<string> SaveImport (List<ImportEmployeeDTO> req, string userName);
    }
}
