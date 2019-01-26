using System.Collections.Generic;

namespace BRM.DAO.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }

        public ICollection<UsersRoles> Roles { get; set; }
        public ICollection<UsersPermissions> Permissions { get; set; }
    }
}
