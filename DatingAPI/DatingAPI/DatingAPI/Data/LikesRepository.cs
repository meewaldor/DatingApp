using DatingAPI.DTO;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using DatingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class LikesRepository : ILikeRepository {
        private readonly DataContext _context;
        public LikesRepository(DataContext context) {
            _context = context; 
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            // a query, nothing happen to the DB
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked") {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);
            }
            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain).Url,
                City = user.City,
                Id = user.Id,
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<User> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include (user => user.LikedUsers)
                .FirstOrDefaultAsync(user => user.Id == userId);
        }
    }
}
