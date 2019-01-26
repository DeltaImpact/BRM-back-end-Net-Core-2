using System.Collections.Generic;

namespace BRM.DAO.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UsersPermissions> UsersPermissions { get; set; }

    }
}
