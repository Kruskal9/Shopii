using Domain.BaseEntity;
using Domain.Entity;
using Domain.ViewEntity.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abtract
{
    public interface IUserServices
    {
        Task<bool> CheckExist(string username);
        Task<VUser> GetUser(string id);
        Task<string> GetUserID(string username);
        Task<ResultData<Profile>> Login(string userName, string password);
        Task<ResultData<VUser>> RegisterUser(UserRegister register);
    }
}
