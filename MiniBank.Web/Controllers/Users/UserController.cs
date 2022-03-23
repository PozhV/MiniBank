using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Web.Controllers.Users.Dto;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Data.Users;
namespace MiniBank.Web.Controllers.Users
{
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
        public User Create(UserDto model)
        {
            return _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            });

        }
        [HttpGet]
        public IEnumerable<User> List()
        {
            return _userService.GetAll();
        }
        [HttpPut]
        public void Edit(Guid Id, [FromQuery]UserDto model)
        {
            _userService.Edit(new User
            {
                UserId = Id,
                Login = model.Login,
                Email = model.Email
            });
        }
        [HttpDelete]
        public void Delete(Guid id)
        {
            _userService.Delete(id);
        }
    }
}
