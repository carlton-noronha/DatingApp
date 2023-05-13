using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            this._mapper = mapper;
            this._dataContext = dataContext;
        }

        public async Task<MemberDTO> GetMemberByUserNameAsync(string username)
        {
            return await this._dataContext.Users.Where(user => user.UserName == username)
            .ProjectTo<MemberDTO>(this._mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            IQueryable<AppUser> query = this._dataContext.Users.AsQueryable();

            query = query.Where(user => user.UserName != userParams.CurrentUsername);
            query = query.Where(user => user.Gender == userParams.Gender);

            DateOnly minDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            DateOnly maxDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(user => user.DateOfBirth >= minDOB && user.DateOfBirth <= maxDOB);

            switch (userParams.OrderBy)
            {
                case "created":
                    query = query.OrderByDescending(user => user.Created);
                    break;
                default:
                    query = query.OrderByDescending(user => user.LastActive);
                    break;
            }

            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(this._mapper.ConfigurationProvider)
             .AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this._dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await this._dataContext.Users.Include(user => user.Photos).FirstOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this._dataContext.Users.Include(user => user.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this._dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            this._dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}