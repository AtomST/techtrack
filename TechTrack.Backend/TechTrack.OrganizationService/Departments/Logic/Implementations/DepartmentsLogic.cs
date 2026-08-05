using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Companies.Models;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.Departments.Entities;
using TechTrack.OrganizationService.Departments.Logic.Interfaces;
using TechTrack.OrganizationService.Departments.Models.Requests;
using TechTrack.OrganizationService.Departments.Models.Responses;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Protos;

namespace TechTrack.OrganizationService.Departments.Logic.Implementations
{
    public class DepartmentsLogic : IDepartmentsLogic
    {
        private readonly OrganizationServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<DepartmentsLogic> _logger;
        private readonly UserIdService.UserIdServiceClient _userGrpcClient;

        public DepartmentsLogic(OrganizationServiceDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<DepartmentsLogic> logger, UserIdService.UserIdServiceClient userGrpcClient)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _logger= logger;
            _userGrpcClient= userGrpcClient;
        }

        public async Task AddEmployeeByIdAsync(Guid departmentId, UserPermissionInfo permissionInfo, AddEmployeeByIdRequest request)
        {
            var department = await _dbContext.Departments
                .Where(d => d.Id == departmentId)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException("Отдел с таким Id не найден.");

            if (department.CompanyId != permissionInfo.CompanyId)
                throw new ForbiddenException("Вы можете иметь доступ только к отделам своей компании");

            var isUserExistsResponse = await _userGrpcClient.IsUserExistsAsync(new IsUserExistsRequest
            {
                UserId = request.Id.ToString()
            });

            if(!isUserExistsResponse.IsUserExists)
                throw new NotFoundException("Пользователь с таким ID не найден.");

            department.DepartmentEmployees.Add(new DepartmentUser
            {
                UserId = request.Id,
                JoinedAt = DateTime.UtcNow,
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<CreateDepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request, UserPermissionInfo userInfo)
        {
            var company = await _dbContext.Companies.FindAsync(userInfo.CompanyId)
                ?? throw new NotFoundException("Компания с таким Id не найдена.");

            var department = new Department()
            {
                Company = company,
                Name = request.Name,
                ResponsibleUserId = request.ResponsibleUserId,
            };

            var isUserInCompany = await _dbContext.CompanyUser.AnyAsync(cu => cu.UserId == request.ResponsibleUserId && cu.CompanyId == userInfo.CompanyId);
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

        public async Task<IList<EmployeeInfo>> GetAllDepartmentEmployees(Guid departmentId, UserPermissionInfo userInfo)
        {
            var department = await _dbContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == departmentId)
                ?? throw new NotFoundException("Отдел с таким ID не найден.");

            if (department.CompanyId != userInfo.CompanyId)
                throw new ForbiddenException("Вы не имеете доступа к этому отделу.");

            if(userInfo.Role == Roles.DepartmentHead || userInfo.Role == Roles.Manager)
            {
                var isUserInDepartment = await _dbContext.DepartmentUsers
                    .AsNoTracking()
                    .AnyAsync(d =>
                        d.DepartmentId == departmentId &&
                        d.UserId == userInfo.UserId);

                if (!isUserInDepartment)
                    throw new ForbiddenException("Вы не имеете доступа к этому отделу.");
            }

            var employees = await _dbContext.DepartmentUsers
                .AsNoTracking()
                .Where(d => d.DepartmentId == departmentId)
                .Join(_dbContext.UserProjections,
                d => d.UserId,
                u => u.UserId,
                (d, u) => new EmployeeInfo
                {
                    UserId = u.UserId,
                    FullName = u.FullName
                })
                .ToListAsync();

            return employees;
        }

        public async Task<GetAllDepartmentsResponse> GetAllDepartmentsAsync(UserPermissionInfo userInfo)
        {
            var departments = await _dbContext.Departments
                .AsNoTracking()
                .Where(d => d.CompanyId == userInfo.CompanyId)
                .ToListAsync();

            return new GetAllDepartmentsResponse
            {
                Departments = departments
            };
        }

        public async Task<GetAllDepartmentsResponse> GetAllUserDepartmentsAsync(UserPermissionInfo userInfo)
        {
            var userDepartments = await _dbContext.Departments
                .AsNoTracking()
                .Join(_dbContext.DepartmentUsers,
                    d => d.Id,
                    du => du.DepartmentId,
                    (d, du) => new {Department = d, DepartmentUser = du})
                .Where(
                    x => x.Department.CompanyId == userInfo.CompanyId &&
                    x.DepartmentUser.UserId == userInfo.UserId)
                .Select(x => x.Department)
                .ToListAsync();

            return new GetAllDepartmentsResponse
            {
                Departments = userDepartments
            };
        }

        public async Task<GetFullDepartmentInfoResponse> GetFullDepartmentInfoAsync(Guid departmentId, UserPermissionInfo permissionInfo)
        {
            var department =
                await _dbContext.Departments
                .AsNoTracking()
                .Include(d => d.Equipments)
                .Where(d => d.Id == departmentId)
                .FirstOrDefaultAsync()
                    ?? throw new NotFoundException("Отдел не найден.");

            if (department.CompanyId != permissionInfo.CompanyId)
                throw new ForbiddenException("Вы можете иметь доступ только к отделам своей компании");

            if (permissionInfo.Role == Roles.Employee || permissionInfo.Role == Roles.Manager)
            {
                var isUserDepartment =
                    await _dbContext.DepartmentUsers
                    .AsNoTracking()
                    .Where(d => d.UserId == permissionInfo.UserId && d.DepartmentId == departmentId)
                    .AnyAsync();

                if (!isUserDepartment)
                    throw new ForbiddenException("У вас нет доступа к этому отделу.");
            }

            return new GetFullDepartmentInfoResponse
            {
                Department = department,
            };
        }
    }
}
