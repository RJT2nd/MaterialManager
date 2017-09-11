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
    public class ProjectKit
    {
        [Key, ScaffoldColumn(false)]
        public int ProjectKitID { get; set; }

        [Required, Display(Name = "Project ID")]
        public int ProjectID { get; set; }

        [StringLength(10000), Display(Name = "Project Kit Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}