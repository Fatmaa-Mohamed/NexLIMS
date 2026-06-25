using NexLIMS.BLL.DTO.RoleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.RoleDto
{
    public class RoleDtoWithStatus:RoleDTO
    {
        public bool status {  get; set; }
    }
}
