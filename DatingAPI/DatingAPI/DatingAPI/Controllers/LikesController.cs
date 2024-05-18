using DatingAPI.DTO;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using DatingAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;
        public LikesController(IUserRepository userRepository, ILikeRepository likesRepository) {
            _userRepository = userRepository;
            _likeRepository = likesRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike (string username)
        {
            var sourceUserId = int.Parse(User.GetUserId());
            var likedUser = await _userRepository.GetUserByUsernameAsync (username);
            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likeRepository.GetUserLike (sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id,
            };
            sourceUser.LikedUsers.Add (userLike);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Fail to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes ([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = int.Parse (User.GetUserId());
            var users = await _likeRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    }
}
