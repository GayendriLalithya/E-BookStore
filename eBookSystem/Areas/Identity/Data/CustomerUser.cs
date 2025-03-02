using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using eBookSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace eBookSystem.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CustomerUser class

public class CustomerUser : IdentityUser
{
    public ICollection<Order> Orders { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }


    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(10)")]
    public string MobileNumber { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Address { get; set; }
}

