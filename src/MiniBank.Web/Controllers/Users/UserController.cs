using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Web.Controllers.Users.Dto;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Data.Users;
using MiniBank.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MiniBank.Web.Controllers.Users
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]", Name = "[controller]_[action]")]
    public class UserController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<User> Create(UserDto model)
        {
            return await _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            });

        }
        [HttpGet]
        public async Task<List<User>> List()
        {
            return await _userService.GetAll();
        }
        [HttpPut]
        public async Task Edit(Guid Id, [FromQuery]UserDto model)
        {
            await _userService.Edit(new User
            {
                UserId = Id,
                Login = model.Login,
                Email = model.Email
            });
        }
        [HttpDelete]
        public async Task Delete(Guid id)
        {
            await _userService.Delete(id);
        }
    }
}
