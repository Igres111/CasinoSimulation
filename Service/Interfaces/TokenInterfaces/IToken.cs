using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Service.Interfaces.TokenInterfaces
{
    public interface IToken
    {
        public string CreateAccessToken(User user);
        public Task<RefreshToken> CreateRefreshTokenAsync(User user);
        public Task<string> RefreshAccessTokenAsync(string refreshTokenInput);
    }
}
