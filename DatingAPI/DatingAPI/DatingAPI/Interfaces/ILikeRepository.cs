using DatingAPI.DTO;
using DatingAPI.Helpers;
using DatingAPI.Models;

namespace DatingAPI.Interfaces
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);

        Task<User> GetUserWithLikes(int userId);

        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams); 
    }
}
