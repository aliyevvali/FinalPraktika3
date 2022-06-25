using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalPraktika3.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Posission { get; set; }
        public string FeedBack { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
