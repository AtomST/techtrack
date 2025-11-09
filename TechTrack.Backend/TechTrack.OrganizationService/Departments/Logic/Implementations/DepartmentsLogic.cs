using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.Departments.Entities;
using TechTrack.OrganizationService.Departments.Logic.Interfaces;
using TechTrack.OrganizationService.Departments.Models.Requests;
using TechTrack.OrganizationService.Departments.Models.Responses;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;

namespace TechTrack.OrganizationService.Departments.Logic.Implementations
{
    public class DepartmentsLogic : IDepartmentsLogic
    {
        private readonly OrganizationServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<DepartmentsLogic> _logger;

        public DepartmentsLogic(OrganizationServiceDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<DepartmentsLogic> logger)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _logger= logger;
        }

        public async Task<CreateDepartmentResponse> CreateDepartmentAsync(Guid companyId, CreateDepartmentRequest request)
        {
            var company = await _dbContext.Companies.FindAsync(companyId)
                ?? throw new NotFoundException("Компания с таким Id не найдена.");

            var department = new Department()
            {
                Company = company,
                Name = request.Name,
                ResponsibleUserId = request.ResponsibleUserId,
            };

            var isUserInCompany = await _dbContext.CompanyUser.AnyAsync(cu => cu.UserId == request.ResponsibleUserId && cu.CompanyId == companyId);
            if (request.ResponsibleUserId.HasValue && !isUserInCompany
                )
            {
                    throw new InvalidInputException("Вы не можете назначить этого пользователя главой отдела, он не в вашей компании");
            }

            await _dbContext.AddAsync(department);
            await _dbContext.SaveChangesAsync();
            if (request.ResponsibleUserId.HasValue)
            {
                await _publishEndpoint.Publish(new DepartmentCreatedWithHead()
                {
                    UserId = request.ResponsibleUserId.Value,
                });
            }

            return new CreateDepartmentResponse(department.Id);
        }

        public async Task<GetAllDepartmentsResponse> GetAllDepartmentsAsync(Guid companyId)
        {
            var departments = await _dbContext.Departments.AsNoTracking().Where(d => d.CompanyId == companyId).ToListAsync();

            return new GetAllDepartmentsResponse
            {
                Departments = departments
            };
        }
    }
}
