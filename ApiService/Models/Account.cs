using System.Collections.Generic;

#nullable disable

namespace ApiService.Models
{
    public partial class Account
    {
        public Account()
        {
            PhoneNumbers = new HashSet<PhoneNumber>();
        }

        public int Id { get; set; }
        public string AuthId { get; set; }
        public string Username { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
