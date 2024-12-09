using Microsoft.AspNetCore.Identity;

namespace Innoverse.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user,List<string> roles);
    }
}
