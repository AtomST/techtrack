using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.Equipments.Entities;
using TechTrack.OrganizationService.Equipments.Logic.Interfaces;
using TechTrack.OrganizationService.Equipments.Models.Requests;
using TechTrack.OrganizationService.Equipments.Models.Responses;

namespace TechTrack.OrganizationService.Equipments.Logic.Implementations
{
    public class EquipmentsLogic : IEquipmentsLogic
    {
        private readonly OrganizationServiceDbContext _dbContext;

        public EquipmentsLogic(OrganizationServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AddEquipmentResponse> AddEquipmentAsync(Guid departmentId, AddEquipmentRequest request)
        {
            var defaultStatus = await _dbContext.EquipmentStatuses
                .Where(s => s.Name == EquipmentStatusConstants.Green)
                .FirstOrDefaultAsync();

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
                EquipmentStatus = defaultStatus
            };

            await _dbContext.AddAsync(equipment);
            await _dbContext.SaveChangesAsync();

            return new AddEquipmentResponse { EquipmentId = equipment.Id };
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
