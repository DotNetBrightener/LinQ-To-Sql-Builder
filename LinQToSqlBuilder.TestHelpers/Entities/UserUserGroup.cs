using System.ComponentModel.DataAnnotations.Schema;

namespace LinQToSqlBuilder.TestHelpers.Entities
{
    [Table("UsersUserGroup")]
    public class UserUserGroup
    {
        public long UserId { get; set; }

        public long UserGroupId { get; set; }
    }
}