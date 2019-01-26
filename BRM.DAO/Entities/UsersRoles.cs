namespace BRM.DAO.Entities
{
    public class UsersRoles : BaseEntity
    {
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
