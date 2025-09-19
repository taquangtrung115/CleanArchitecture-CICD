using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.Abstractions;

public interface ITokenCacheService
{
    Task<string?> GetRefreshTokenAsync(string userId);
    Task SetRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiration);
    Task RemoveRefreshTokenAsync(string userId);
    Task BlacklistTokenAsync(string tokenId, TimeSpan expiration);
    Task<bool> IsTokenBlacklistedAsync(string tokenId);
    Task RemoveAllUserTokensAsync(string userId);
}