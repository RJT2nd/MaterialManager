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
    public class Part
    {
        public Part()
        {

        }

        public Part(Part input)
        {
            this.JBMaterialID = input.JBMaterialID;
            this.NSN = input.NSN;
            this.PartNumber = input.PartNumber;
            this.Description = input.Description;
        }

        [Key, ScaffoldColumn(false)]
        public int PartID { get; set; }

        [StringLength(255), Display(Name = "JobBoss Material ID")]
        public string JBMaterialID { get; set; }

        [StringLength(255), Display(Name = "NSN")]
        public string NSN { get; set; }

        [StringLength(255), Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [StringLength(10000), Display(Name = "Part Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}