using System.Collections.Generic;
using BRM.DAO.Entities;

namespace BRM.BL.Models.RoleDto
{
    public class RoleReturnDto
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<Role> Roles { get; set; }
    }
}
