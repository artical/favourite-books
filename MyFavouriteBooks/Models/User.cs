using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        [BindNever]
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<UserBook> UserBooks { get; set; }
    }
}
