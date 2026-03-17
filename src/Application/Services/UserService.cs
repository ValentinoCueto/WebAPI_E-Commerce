using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UserToResponse> GetAll()
        {
            var users = _userRepository.GetAll();

            var userToResponseList = new List<UserToResponse>();

            foreach (var user in users)
            {
                var userToResponse = new UserToResponse()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
                userToResponseList.Add(userToResponse);
            }
            return userToResponseList;
        }

        public UserToResponse? GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException("El usuario no fue encontrado");
            }
            var userToResponse = new UserToResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return userToResponse;
        }

        public int Create(UserToCreate userToCreate)
        {
            var existingUserName = _userRepository.GetByName(userToCreate.Name);
            var existingUserEmail = _userRepository.GetByEmail(userToCreate.Email);
            if (existingUserName is not null || existingUserEmail is not null)
            {
                throw new BadRequestException("Ya existe un usuario con ese nombre o email");
            }
            var user = new User()
            {
                Name = userToCreate.Name,
                Email = userToCreate.Email,
                Password = userToCreate.Password,
                Rol = RolEnum.Client,
                IsActive = true
            };

            _userRepository.Create(user);
            return user.Id;
        }

        public void DeactivateUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException($"El usuario con ID {id} no fue encontrado o ya está desactivado.");
            }
            user.IsActive = false; 
            _userRepository.Update(user);
        }

        public void Update(UserToUpdate userToUpdate, int id)
        {
            var existingUserName = _userRepository.GetByName(userToUpdate.Name);
            var existingUserEmail = _userRepository.GetByEmail(userToUpdate.Email);
            if (existingUserName is not null || existingUserEmail is not null)
            {
                throw new BadRequestException("Ya existe un usuario con ese nombre o email");
            }
            var user = _userRepository.GetById(id);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException($"El usuario con ID {id} no fue encontrado.");
            }

            user.Name = userToUpdate.Name;
            user.Email = userToUpdate.Email;
            user.Password = userToUpdate.Password;  
            _userRepository.Update(user);
        }

        public UserToResponse? GetByName(string name)
        {
            var user = _userRepository.GetByName(name);
            if (user == null || !user.IsActive)
            {
                throw new NotFoundException("El usuario no fue encontrado");
            }
            var userToResponse = new UserToResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return userToResponse;
        }
    }


}
