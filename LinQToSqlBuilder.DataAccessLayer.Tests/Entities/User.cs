using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinQToSqlBuilder.DataAccessLayer.Tests.Entities
{
    public abstract class ViewModel
    {
        public long Id { get; set; }

        public bool RecordDeleted { get; set; }
    }

    [Table("Users")]
    public class User: ViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<UserGroup> Groups { get; set; }

        public DateTimeOffset? LastChangePassword { get; set; }

        public DateTimeOffset? ModifiedDate       { get; set; }

        public int FailedLogIns { get; set; }
    }

    public class UserViewModel
    {
        public long   Id        { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}