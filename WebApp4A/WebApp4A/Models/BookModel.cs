using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApp4A.Models
{
    public class BookModel
    {       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int Year { get; set; }
        public XmlDocument Content { get; set; }
        public DataTable listy { get; set; }

    }
}