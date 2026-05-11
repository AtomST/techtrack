using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.Equipments.Entities;
using TechTrack.OrganizationService.Equipments.Logic.Interfaces;
using TechTrack.OrganizationService.Equipments.Models.Requests;
using TechTrack.OrganizationService.Equipments.Models.Responses;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Equipment;
using TechTrack.Shared.Exceptions;

namespace TechTrack.OrganizationService.Equipments.Logic.Implementations
{
    public class EquipmentsLogic : IEquipmentsLogic
    {
        private readonly OrganizationServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public EquipmentsLogic(OrganizationServiceDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<AddEquipmentResponse> AddEquipmentAsync(Guid departmentId, AddEquipmentRequest request)
        {
            var defaultStatus = (int)EquipmentStatusCode.Green;

            var existingNames =
                await _dbContext.Equipments
                .AsNoTracking()
                .Where(e => 
                    (EF.Functions.ILike(e.Name, $"{request.Name} (%)") 
                    || EF.Functions.ILike(e.Name, request.Name))
                    && e.DepartmentId == departmentId)
                .Select(e => e.Name)
                .Order()
                .ToListAsync();

            var postFix = GenerateEquipmentPostfix(existingNames, request.Name);
            var equipment = new Equipment
            {
                Name = string.IsNullOrEmpty(postFix) ? request.Name : $"{request.Name} {postFix}",
                SerialNumber = request.SerialNumber,
                DepartmentId = departmentId,
                Description = request.Desctiption,
                ResponsibleUserId = request.ResponsibleUserId,
                CurrentStatusId = defaultStatus
            };

            await _dbContext.AddAsync(equipment);
            await _dbContext.SaveChangesAsync();

            

            return new AddEquipmentResponse { EquipmentId = equipment.Id };
        }

        public async Task<GetAllEquipmentsResponse> GetAllEquipmentsAsync(Guid departmentId, UserPermissionInfo userPermissionInfo)
        {
            if(userPermissionInfo.Role == Roles.Employee || userPermissionInfo.Role == null)
            {
                var isDepartmentEmployee = await _dbContext.DepartmentUsers
                    .Where(ud => ud.DepartmentId == departmentId && ud.UserId == userPermissionInfo.UserId)
                    .AnyAsync();

                if (!isDepartmentEmployee)
                    throw new ForbiddenException("Вы не сотрудник этого отдела");
            }
            var equipments = await _dbContext.Equipments
                .AsNoTracking()
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();

            return new GetAllEquipmentsResponse { Equipments = equipments };
        }

        private string GenerateEquipmentPostfix(IList<string> names, string baseName)
        {
            if(names.Count == 0)
                return string.Empty;

            int newIndex = 2;
            while (names.Contains($"{baseName} ({newIndex})"))
                newIndex++;

            return $"({newIndex})";
        }
    }
}
