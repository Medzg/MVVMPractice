using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Model
{
    public class Friend
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }
        [StringLength(60)]
        public string LastName { get; set; }
        [StringLength(60)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
