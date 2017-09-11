using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for ProjectKitItem
/// </summary>
namespace MaterialManager.Models
{
    public class ProjectKitItem
    {
        [Key, ScaffoldColumn(false)]
        public int ProjectKitItemID { get; set; }

        [Required, Display(Name = "Kit ID")]
        public int ProjectKitID { get; set; }

        [Required, Display(Name = "Part ID")]
        public int PartID { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}