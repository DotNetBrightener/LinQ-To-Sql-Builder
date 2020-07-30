using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinQToSqlBuilder.DataAccessLayer.Tests.Entities
{
    [Table("UsersGroup")]
    public class UserGroup
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsUndeletable { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}