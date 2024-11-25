using UserEntity = Domain.Entities.User;

namespace Domain.Ports
{
    public interface IUserRepository
    {
        Task<UserEntity> Authenticate(string email, string password);
    }
}
