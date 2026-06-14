using NexLIMS.BLL.DTO.PermissionDto;
using NextLIMS.DAL.Repositories;

namespace NextLIMS.BLL.Services.Permissions
{
    public class PermissionService
    {
        private readonly PermissionRepository _repository;

        public PermissionService(PermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PermissionDTO>> GetAllPermissions()
        {
            var permissions = await _repository.GetAllPermissionsAsync();

            return permissions.Select(x => new PermissionDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
        }
    }
}