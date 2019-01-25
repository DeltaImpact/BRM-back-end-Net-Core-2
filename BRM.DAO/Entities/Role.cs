using System;
using System.Collections.Generic;
using System.Text;

namespace BRM.DAO.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UsersRoles> UsersRoles { get; set; }
    }
}