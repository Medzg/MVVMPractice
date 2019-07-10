using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    public class Friend
    {
        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
            Meetings = new Collection<Meeting>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }
        [StringLength(60)]
        public string LastName { get; set; }
        [StringLength(60)]
        [EmailAddress]
        public string Email { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int? FavoriteLangugeId { get; set; }

        public ProgrammingLanguage FavoriteLanguge { get; set; }

        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }

        public ICollection<Meeting> Meetings { get; set; }
    }
}
