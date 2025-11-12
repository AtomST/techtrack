using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Events
{
    public class CompanyRegisteredWithOwner
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
