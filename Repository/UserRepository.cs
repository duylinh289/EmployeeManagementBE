using AutoMapper;
using EmployeeManagementBE.DTO.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RepositoryCodeFirstCore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserRepository(MyDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> Login(UserLoginDTO req)
        {
            var users = await _context.Users.Where(x => x.UserName == req.UserName).ToListAsync();
            var user = users.FirstOrDefault(x => VerifyPassword(req.Password, x.PasswordHash, x.PasswordSalt) && x.UserName == req.UserName);


            if (user == null)
            {
                return "Not found";
            }
            else
            {
                return CreateToken(req, user.Role);
            }    
        }

        public async Task<User> Register(UserRegisterDTO req, int isEmployee = 0)
        {
            var checkUserName = await _context.Users.FirstOrDefaultAsync(x => x.UserName == req.UserName);

            if (checkUserName != null)
            {
                throw new Exception("User name is existed!");
            }
            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User{
                UserName = req.UserName,
                Email = req.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = (isEmployee == 0 ? "ManagerLV2" : "Employee")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(UserLoginDTO user, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["AppSettings:Issuer"],
                _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: cred) ;

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<User>, List<UserDTO>>(users);
        }

        public async Task<string> Update(UserDTO req)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == req.UserName);
            user.Role = req.Role;
            user.Email = req.Email;

            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> Delete(string username)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return "Success";
        }

        public async Task<List<UserDTO>> GetByUsername(string username)
        {
            var user = await _context.Users.Where(x => x.UserName == username).ToListAsync();
            return _mapper.Map<List<User>, List<UserDTO>>(user);
        }
    }
}
