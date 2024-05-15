using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseOffice.DAL.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? Users { get; set; }

        public string FamilyMember { get; set; }

        public string HousingConditions { get; set; }

        public string ImprovingConditions { get; set; }

        public bool LowIncomeFamily { get; set; }
    }
}
