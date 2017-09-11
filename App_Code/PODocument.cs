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
    public class PODocument
    {
        [Key, ScaffoldColumn(false)]
        public int PODocID { get; set; }

        [Required, Display(Name = "Purchase Order ID")]
        public int POID { get; set; }

        [StringLength(255), Display(Name = "File Name")]
        public string FileName { get; set; }

        public string FilePath { get; set; }

        [StringLength(10000), Display(Name = "File Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}