using System;
using System.Collections.Generic;
using System.Text;

namespace BRM.DAO.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UsersPermissions> UsersPermissions { get; set; }

    }
}
