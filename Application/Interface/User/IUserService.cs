namespace Application.Interface.User
{
    public interface IUserService
    {
        Task<Domain.Entities.Users.User> GetUserByIdAsync(Guid id);
    }
}

