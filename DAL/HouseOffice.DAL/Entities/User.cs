using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace HouseOffice.DAL.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role? Roles { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "SNILS is required")]
        [RegularExpression(@"\d{3}-\d{3}-\d{3} \d{2}", ErrorMessage = "Invalid SNILS format")]
        public string SNILS { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Passport Series is required")]
        public string PassportSeries { get; set; }

        [Required(ErrorMessage = "Passport Number is required")]
        public string PassportNumber { get; set; }

        [Required(ErrorMessage = "Passport Issued is required")]
        public string PassportIssued { get; set; }

        [Required(ErrorMessage = "Passport Date is required")]
        public DateTime PassportDate { get; set; }
        
        public string ShortDateWithYear => PassportDate.ToString("d MMMM, yyyy", new CultureInfo("ru-RU"));

        public List<UserRequest> UserRequests { get; set; } = new();

        [NotMapped]
        public string FIO => $"{LastName} {FirstName} {MiddleName}";
    }
}
