using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Auth
{
    public static class Policies
    {
        public const string DevOnly = "DevOnly";
        public const string AdminAccess = "AdminAccess";
        public const string ManagementAccess = "ManagementAccess";
        public const string SupervisorAccess = "SupervisorAccess";
        public const string EmployeeAccess = "EmployeeAccess";
    }
}
