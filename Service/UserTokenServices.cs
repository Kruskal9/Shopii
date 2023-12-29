using Data.Abtract;
using Domain.BaseEntity;
using Domain.Entity;
using Service.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserTokenServices:IUserTokenServices
    {
        IRepository<UserToken> _repository;
        IDapperHelper _dapperHelper;

        public UserTokenServices(IRepository<UserToken> repository, IDapperHelper dapperHelper)
        {
            _repository = repository;
            _dapperHelper = dapperHelper;
        }

        public async Task CreateToken(UserToken userToken)
        {
            await _repository.Insert(userToken);
            await _repository.Commit();
        }

        public async Task<UserToken> GetUserToken(string serialNumber)
        {
            string sql = $"select * from AspNetUserTokens where name = '{serialNumber}'";
            var rs = await _dapperHelper.ExecuteReturnFirst<UserToken>(sql);
            return rs;
        }
    }
}
