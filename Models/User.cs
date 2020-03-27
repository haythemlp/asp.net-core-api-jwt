using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JwtApplication.Validations;
using Newtonsoft.Json;

namespace JwtApplication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [EmailUserUnique]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]

        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Book> Books { get; set; }


    }
}
