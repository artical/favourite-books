using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string CoverThumb { get; set; }
        public string LanguageCode { get; set; }
        public string Subjects { get; set; }
        public string Authors { get; set; }
    }
}
