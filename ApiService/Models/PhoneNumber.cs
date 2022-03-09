using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ApiService.Models
{
    public partial class PhoneNumber
    {
        public int Id { get; set; }
        public int MessageCount { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [MinLength(6, ErrorMessage = "Phone Number should be minimum 6 characters")]
        [MaxLength(16, ErrorMessage = "Phone Number Should not be More than 16 characters")]
        public string Number { get; set; }
        public int? AccountId { get; set; }
        [MinLength(1, ErrorMessage = "Text should be minimum 1 characters")]
        [MaxLength(120, ErrorMessage = "Text Should not be More than 120 characters")]
        public string Text { get; set; }
        public string To{ get; set; }
        public string From { get; set; }
        public virtual Account Account { get; set; }
    }
}
