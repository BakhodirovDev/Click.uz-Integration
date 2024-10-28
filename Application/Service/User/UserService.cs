using Application.Interface.User;
namespace Application.Service.User
{
    public class UserService : IUserService
    {
        public Task<Domain.Entities.Users.User> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
