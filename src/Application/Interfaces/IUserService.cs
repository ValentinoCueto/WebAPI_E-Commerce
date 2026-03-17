using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        List<UserToResponse> GetAll();
        UserToResponse? GetById(int id);
        int Create(UserToCreate userToCreate);
        void DeactivateUser(int id);
        void Update(UserToUpdate userToUpdate, int id);
        UserToResponse? GetByName(string name);
    }
}
