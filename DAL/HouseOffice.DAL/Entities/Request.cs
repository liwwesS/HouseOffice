using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace HouseOffice.DAL.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public ObservableCollection<User>? Users { get; set; } = [];

        [Required]
        public string FamilyMember { get; set; }

        [Required]
        public string HousingConditions { get; set; }

        [Required]
        public string ImprovingConditions { get; set; }

        [Required]
        public bool LowIncomeFamily { get; set; }
    }
}
