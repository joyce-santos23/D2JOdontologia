using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using UserEntity = Domain.Entities.User;

namespace Data.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public UserRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task<UserEntity> Authenticate(string email, string password)
        {
            var user = await _clinicaDbContext.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user; 
        }

    }


}
