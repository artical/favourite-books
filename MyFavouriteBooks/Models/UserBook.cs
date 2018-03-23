using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class UserBook
    {
        public string UserId { get; set; }
        public string ISBN { get; set; }

        [BindNever]
        [JsonIgnore]
        [ForeignKey("UserId")]
        [IgnoreDataMember]
        public Book Book { get; set; }
    }
}
