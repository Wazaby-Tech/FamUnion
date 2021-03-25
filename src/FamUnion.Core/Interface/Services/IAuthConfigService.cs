using FamUnion.Auth;

namespace FamUnion.Core.Interface.Services
{
    public interface IAuthConfigService
    {
        AuthConfig GetConfig(string configKey);
    }
}
