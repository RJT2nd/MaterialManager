using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

// This creates an RFQDocument object
// Each RFQDocument has an id for the document, an id for which RFQ it belongs to, a file name, a file path to the saved location on the server, and a description
// Actions involving RFQDocuments are handled in the DataActions.cs file in App_Code
namespace MaterialManager.Models
{
    public class RFQDocument
    {
        [Key, ScaffoldColumn(false)]
        public int RFQDocID { get; set; }

        [Required, Display(Name = "RFQ ID")]
        public int RFQID { get; set; }

        [StringLength(255), Display(Name = "File Name")]
        public string FileName { get; set; }

        public string FilePath { get; set; }

        [StringLength(10000), Display(Name = "File Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}