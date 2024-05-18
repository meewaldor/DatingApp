using DatingAPI.DTO;
using DatingAPI.Helpers;
using DatingAPI.Models;

namespace DatingAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        void Post(User user);
        void Delete(User user);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberByIdAsync(int id);
        Task<MemberDto> GetMemberByUsernameAsync(string username);

    }
}
