using System;
using System.ComponentModel.DataAnnotations;

namespace BRM.DAO.Entities
{
    public class BaseEntity
    {
        [Key] public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}