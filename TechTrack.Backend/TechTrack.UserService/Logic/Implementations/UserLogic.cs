using Grpc.Core;
using System.Net;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;
using TechTrack.UserService.Data.Entities;
using TechTrack.UserService.Logic.Interfaces;

namespace TechTrack.UserService.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {

        private readonly UserServiceDbContext _dbContext;
        public UserLogic(UserServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
    }
}
