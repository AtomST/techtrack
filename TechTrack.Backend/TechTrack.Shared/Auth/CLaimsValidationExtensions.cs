using System.Security.Claims;
using TechTrack.Shared.Exceptions;

namespace TechTrack.Shared.Auth
{
    public static class ClaimsValidationExtensions
    {
        public static void AdditionalPolicyValidation(this ClaimsPrincipal principal, Guid companyId)
        {
            var info = GetPrincipalInfo(principal);
            ValidateUserPermission(info, companyId);
        }

        public static UserPermissionInfo GetPrincipalInfo(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var companyIdFromToken = user.FindFirstValue(CustomClaimTypes.CompanyId);
            var userRole = user.FindFirstValue(ClaimTypes.Role);

            return new UserPermissionInfo
            {
                UserId = Guid.Parse(userId),
                Role = userRole,
                CompanyId = companyIdFromToken,
            };
        }

        private static void ValidateUserPermission(UserPermissionInfo userPermissionInfo, Guid companyId)
        {
            if (companyId.ToString() != userPermissionInfo.CompanyId
                && userPermissionInfo.Role != Roles.Dev
                && userPermissionInfo.Role != Roles.PlatformAdmin)
            {
                throw new ForbiddenException("У вас нет доступа к информации этой компании.");
            }
        }
    }
}
