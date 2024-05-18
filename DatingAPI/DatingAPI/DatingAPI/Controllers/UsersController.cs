using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatingAPI.Data;
using DatingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using DatingAPI.Interfaces;
using AutoMapper;
using DatingAPI.DTO;
using System.Security.Claims;
using DatingAPI.Extensions;
using DatingAPI.Helpers;

namespace DatingAPI.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUser = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }
            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

        // GET: api/Users/5

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            var user = await _userRepository.GetMemberByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // GET: api/Users/my
        [HttpGet("{name}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberByUsernameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult> PutUser(MemberUpdateDto memberUpdateDto )
        {
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();

            // update all of the properties in the DTO to the properties in that user
            _mapper.Map(memberUpdateDto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Fail to update user");
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _userRepository.Post(user);
            await _userRepository.SaveAllAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto (IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) { return BadRequest(result.Error); }
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0) photo.IsMain = true;
            //save to db
            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync() ) 
                return CreatedAtAction(nameof(GetUser),new {username = user.UserName}, 
                    _mapper.Map<PhotoDto>(photo));
            return BadRequest("Problem uploading photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto (int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);
            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This is already your main photo");
            var currentMain = user.Photos.FirstOrDefault(photo => photo.IsMain);
            if (currentMain != null) 
            {
                currentMain.IsMain = false;
            }
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync())
                return NoContent();
            return BadRequest("Problem setting main photo");
        }
        // DELETE: api/Users/5
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null)
            {
                return NotFound();
            }

            var photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) { return  BadRequest(result.Error.Message); }
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting photo");
        }

        private bool UserExists(int id)
        {
            return _userRepository.GetUserByIdAsync(id) != null;
        }
    }
}
