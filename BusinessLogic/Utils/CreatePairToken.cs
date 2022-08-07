using System.Security.Claims;
using DAL.Model;
using Data;

namespace BusinessLogic.Utils
{
    public class CreatePairToken
    {
        private readonly GenerateToken _jwtHandler;

        public CreatePairToken(GenerateToken jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        public (PairTokenDto, double) GeneratePairToken(UserModel user)
        {
            var claims = new List<Claim>
           {
               new(ClaimTypes.Name, user.Id.ToString()),
               new("Email", user.Email),
               new(ClaimTypes.Role, user.Role.ToString())
           };

            return (new PairTokenDto
            {
                AccessToken = _jwtHandler.CreateAccessToken(claims),
                RefreshToken = _jwtHandler.CreateRefreshToken()
            }
                , _jwtHandler.GetRefreshTokenExTime());
        }
    }

}

