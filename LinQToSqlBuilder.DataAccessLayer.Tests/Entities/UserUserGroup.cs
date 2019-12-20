using System.ComponentModel.DataAnnotations.Schema;

namespace LinQToSqlBuilder.DataAccessLayer.Tests.Entities
{
    [Table("UsersUserGroup")]
    public class UserUserGroup
    {
        public long UserId { get; set; }

        public long UserGroupId { get; set; }
    }
}