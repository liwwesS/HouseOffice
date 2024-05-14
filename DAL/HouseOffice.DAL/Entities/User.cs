using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace HouseOffice.DAL.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public ObservableCollection<Role>? Roles { get; set; } = [];

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string SNILS { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string PassportSeries { get; set; }

        [Required]
        public string PassportNumber { get; set; }

        [Required]
        public string PassportIssued { get; set; }

        [Required]
        public DateTime PassportDate { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
