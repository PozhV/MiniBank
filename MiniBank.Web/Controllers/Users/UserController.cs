using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Web.Controllers.Users.Dto;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Data.Users;
namespace MiniBank.Web.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public User Create(UserDto model)
        {
            return _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            });

        }
        [HttpPut]
        public void Edit(string Id, [FromQuery]UserDto model)
        {
            _userService.Edit(new User
            {
                UserId = Id,
                Login = model.Login,
                Email = model.Email
            });
        }
        [HttpDelete]
        public void Delete(string id)
        {
            _userService.Delete(id);
        }
    }
}
