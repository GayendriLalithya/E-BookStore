using System.Collections.Generic;

namespace eBookSystem.Models
{
    public class Admin
    {
        public int ID { get; set; } // Primary key, auto-incremental

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; } // Unique field
        public string Email { get; set; } // Unique field
        public string PasswordHash { get; set; } // For storing hashed passwords
        public ICollection<Admin> Admins { get; set; } = new List<Admin>();
    }
}
