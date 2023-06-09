using EmployeeManagementBE.DTO.User;
using RepositoryCodeFirstCore.Data;

namespace RepositoryCodeFirstCore.IRepository
{
    public interface IUserRepository
    {
        public Task<User> Register(UserRegisterDTO req, int isEmployee = 0);
        public Task<string> Login (UserLoginDTO req);
        public Task<List<UserDTO>> GetAll();
        public Task<string> Update(UserDTO req);
        public Task<string> Delete(string username);
        public Task<List<UserDTO>> GetByUsername(string username);

    }
}
