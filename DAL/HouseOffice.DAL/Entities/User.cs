using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseOffice.DAL.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role? Roles { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string SNILS { get; set; }

        public string Password { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string PassportSeries { get; set; }

        public string PassportNumber { get; set; }

        public string PassportIssued { get; set; }

        public DateTime PassportDate { get; set; }

        public List<UserRequest> UserRequests { get; set; } = new();
    }
}
