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
    public class Project
    {
        [ScaffoldColumn(false)]
        public int ProjectID { get; set; }

        [Required, StringLength(255), Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [StringLength(255), Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [Display(Name = "Funds Allocated")]
        public decimal FundsAllocated { get; set; }
    }
}