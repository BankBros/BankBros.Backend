using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.DataAccess.Abstract;

namespace BankBros.Backend.Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;
        
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }
        public void Add(User user)
        {
            _userDal.Add(user);
        }
        [CacheAspect(duration: 20)]
        public User GetByUsername(string username)
        {
            return _userDal.GetSingle(x => x.Username.Equals(username));
        }
    }
}
