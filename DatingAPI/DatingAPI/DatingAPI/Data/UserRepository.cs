using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAPI.DTO;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using DatingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            // FindAsync(PK): Find an entity with a given PK
            return await _context.Users
                .FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(
                user => user.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            // SaveChangesAsync(): return a number of rows
            return await _context.SaveChangesAsync() > 0;
        }

        public async void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async void Post(User user)
        {
            _context.Users.Add(user);          
        }

        public async void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(user => user.UserName != userParams.CurrentUsername);
            query = query.Where(user => user.Gender == userParams.Gender);

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(user => user.DateOfBirth >= minDob && user.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), 
                userParams.PageNumber, 
                userParams.PageSize);
        }

        public async Task<MemberDto> GetMemberByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MemberDto> GetMemberByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(user => user.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}
