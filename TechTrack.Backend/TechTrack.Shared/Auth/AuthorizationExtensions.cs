using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Auth
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddTechTrackAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PlatformAdminAccess", policy =>
                    policy.RequireRole(
                        Roles.PlatformAdmin,
                        Roles.Dev));

                options.AddPolicy("DevOnly", policy =>
                    policy.RequireRole(Roles.Dev));

                options.AddPolicy("CompanyHeadAccess", policy =>
                    policy.RequireRole(
                        Roles.CompanyHead,
                        Roles.Admin,
                        Roles.Dev));

                options.AddPolicy("AdminAccess", policy =>
                    policy.RequireRole(
                        Roles.DepartmentHead,
                        Roles.CompanyHead,
                        Roles.Admin,
                        Roles.Dev));

                options.AddPolicy("ManagementAccess", policy =>
                    policy.RequireRole(
                        Roles.Manager,
                        Roles.DepartmentHead,
                        Roles.CompanyHead,
                        Roles.Admin,
                        Roles.Dev));

                options.AddPolicy("EmployeeAccess", policy =>
                    policy.RequireRole(
                        Roles.Employee,
                        Roles.Manager,
                        Roles.DepartmentHead,
                        Roles.CompanyHead,
                        Roles.Admin,
                        Roles.Dev));
            });

            return services;
        }
    }
}
