using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Domains.Users.Validators;
using FluentValidation;
namespace MiniBank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _userValidator;

        public UserService(IValidator<User> userValidator, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        public async Task<List<User>> GetAll()
        {
           return await _userRepository.GetAll();
        }

        public async Task<User> Create(User user)
        {
            _userValidator.ValidateAndThrow(user);
            var _user = await _userRepository.Create(user);
            await _unitOfWork.SaveChangesAsync();
            return _user;
        }
        public async Task Edit(User user)
        {
            _userValidator.ValidateAndThrow(user);
            await _userRepository.Edit(user);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task Delete(Guid id)
        {
            await _userRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
