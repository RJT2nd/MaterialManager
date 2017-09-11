using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for RequestForQuote
/// </summary>
namespace MaterialManager.Models
{
    public class PartsCatalog
    {
        [Key, ScaffoldColumn(false)]
        public int PartsCatalogID { get; set; }

        [Required, Display(Name = "Project ID")]
        public int ProjectID { get; set; }

        [Required, Display(Name = "Part ID")]
        public int PartID { get; set; }
    }
}