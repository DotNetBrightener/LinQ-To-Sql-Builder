using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinQToSqlBuilder.DataAccessLayer.Tests.Entities
{
    [Table("Users")]
    public class User
    {
        public long   Id        { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<UserGroup> Groups { get; set; }

        public DateTimeOffset? LastChangePassword { get; set; }

        public DateTimeOffset? ModifiedDate       { get; set; }

        public int FailedLogIns { get; set; }
    }
}