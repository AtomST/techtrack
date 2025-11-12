using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Companies.Entities;
using TechTrack.OrganizationService.Companies.Logic.Interfaces;
using TechTrack.OrganizationService.Companies.Models.Requests;
using TechTrack.OrganizationService.Companies.Models.Responses;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;

namespace TechTrack.OrganizationService.Companies.Logic.Implementatios
{
    public class CompaniesLogic : ICompaniesLogic
    {
        private readonly OrganizationServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public CompaniesLogic(OrganizationServiceDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
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
