using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Companies.Entities;
using TechTrack.OrganizationService.Companies.Logic.Interfaces;
using TechTrack.OrganizationService.Companies.Models.Requests;
using TechTrack.OrganizationService.Companies.Models.Responses;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Protos;

namespace TechTrack.OrganizationService.Companies.Logic.Implementatios
{
    public class CompaniesLogic : ICompaniesLogic
    {
        private readonly OrganizationServiceDbContext _dbContext;
        private readonly UserIdService.UserIdServiceClient _userIdClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public CompaniesLogic(OrganizationServiceDbContext dbContext, IPublishEndpoint publishEndpoint, UserIdService.UserIdServiceClient userIdClient)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _userIdClient = userIdClient;
        }

        public async Task AddEmployeeByEmailAsync(AddEmployeeByEmailRequest request, Guid companyId)
        {
            var message = await _userIdClient.GetUserIdByEmailAsync(new GetUserIdByEmailRequest 
            {
                UserEmail = request.Email
            });
            var userId = message.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new NotFoundException("Пользователь с таким Email не найден.");

            var guidUserId = Guid.Parse(userId);

            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId)
                ?? throw new NotFoundException("Компания с таким Id не найдена.");

            company.UsersInfo.Add(new CompanyUser
            {
                UserId = guidUserId,
                JoinedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
            await _publishEndpoint.Publish(new UserCompanyChanged
            {
                UserId = guidUserId,
                CompanyId = companyId
            });
            await _publishEndpoint.Publish(new UserRoleChanged())
            return;

        }

        public async Task<RegisterCompanyResponse> RegisterAsync(RegisterCompanyRequest companyDto)
        {
            if (await _dbContext.Companies.AnyAsync(c => c.Name == companyDto.Name))
                throw new RecordExistsException("Компания с таким именем уже зарегистрирована.");

            var dateTime = DateTime.UtcNow;

            Company company = new Company()
            {
                Name = companyDto.Name,
                Address = companyDto.Address,
                ConnectedAt = dateTime,
                CompanyHeadId = companyDto.CompanyHeadId
            };
            await _dbContext.AddAsync(company);

            if (companyDto.CompanyHeadId != null)
            {
                CompanyUser bind = new CompanyUser()
                {
                    UserId = companyDto.CompanyHeadId.Value,
                    Company = company,
                    JoinedAt = dateTime,
                };
                await _dbContext.CompanyUser.AddAsync(bind);
            }

            await _dbContext.SaveChangesAsync();

            if (companyDto.CompanyHeadId != null)
                await _publishEndpoint.Publish(
                new CompanyRegisteredWithOwner
                {
                    UserId = companyDto.CompanyHeadId.Value,
                    CompanyId = company.Id
                });

            return new RegisterCompanyResponse
            (
                CompanyId: company.Id
            );
        }
    }
}
