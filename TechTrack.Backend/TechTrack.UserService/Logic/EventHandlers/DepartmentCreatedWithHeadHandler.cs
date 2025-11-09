using MassTransit;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.UserService.Data;
using TechTrack.UserService.Logic.Interfaces;

namespace TechTrack.UserService.Logic.EventHandlers
{
    public class DepartmentCreatedWithHeadHandler : IConsumer<DepartmentCreatedWithHead>
    {
        private readonly IUserLogic _userLogic;

        public DepartmentCreatedWithHeadHandler(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }
        public async Task Consume(ConsumeContext<DepartmentCreatedWithHead> context)
        {
            var message = context.Message;
            await _userLogic.ChangeUserRoleFromEvent(message.UserId, Roles.DepartmentHead);
        }
    }
}
