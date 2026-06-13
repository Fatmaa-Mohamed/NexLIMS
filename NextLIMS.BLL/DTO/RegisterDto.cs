using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexLIMS.BLL.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Tenant name is required.")]
        [StringLength(100, ErrorMessage = "Tenant name cannot exceed 100 characters.")]
        public string TenantName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Subscription tier is required.")]
        public string SubscriptionTier { get; set; }

        [Required(ErrorMessage = "Admin name is required.")]
        [StringLength(50, ErrorMessage = "Admin name cannot exceed 50 characters.")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int NumberofSampleInMonth { get; set; }
        public int PaymentMethodId { get; set; }
        public int Amount { get; set; }
    }
}
