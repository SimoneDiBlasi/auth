using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Handlers.Model
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [StringLength(255)]
        public required string UserId { get; set; }

        [StringLength(255)]
        public string? Street { get; set; }

        [StringLength(255)]
        public string? City { get; set; }

        [StringLength(128)]
        public string? State { get; set; }

        [StringLength(32)]
        public string? PostalCode { get; set; }

        [StringLength(128)]
        public string? Country { get; set; }

        public virtual required IdentityUser User { get; set; }

    }
}
