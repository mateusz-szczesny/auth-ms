using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Auth.Models
{

    public class User : IdentityUser<long>
    {
        public User() : base()
        {
            IsActive = true;
        }

        [Column("id")]
        override public long Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}