using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public class UserRoleChanged
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
