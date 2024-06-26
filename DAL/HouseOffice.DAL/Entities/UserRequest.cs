﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HouseOffice.DAL.Entities
{
    public class UserRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? Users { get; set; }

        public int RequestId { get; set; }
        public Request? Requests { get; set; }

        public int StatusId { get; set; }
        public Status? Status { get; set; }

        public string? FamilyMember { get; set; }

        public string? HousingConditions { get; set; }

        public string? ImprovingConditions { get; set; }

        public bool? LowIncomeFamily { get; set; }

        public string? LandRegistrationFile { get; set; }
        public string? PassportFile { get; set; }
        public string? ProxyFile { get; set; }

        public DateTime? RequestDate { get; set; }
        
        [NotMapped]
        public bool IsOverdue { get; set; }
    }
}
