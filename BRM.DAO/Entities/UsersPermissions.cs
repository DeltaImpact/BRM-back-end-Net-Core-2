﻿namespace BRM.DAO.Entities
{
    public class UsersPermissions : BaseEntity
    {
        public Permission Permission { get; set; }
        public User User { get; set; }
    }
}
