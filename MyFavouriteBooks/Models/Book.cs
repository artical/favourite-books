using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class Book
    {
        [Key]
        [Required]
        [JsonProperty("ISBN")]
        public string ISBN { get; set; }

        [Required]
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("CoverThumb")]
        public string CoverThumb { get; set; }

        [JsonProperty("LanguageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("Subjects")]
        public string Subjects { get; set; }

        [JsonProperty("Authors")]
        public string Authors { get; set; }
    }
}
