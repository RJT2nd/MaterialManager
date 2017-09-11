using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for ExtendedProjectKitItem
/// </summary>

namespace MaterialManager.Models
{

    public class ExtendedProjectKitItem
    {
        private ProjectKitItem item;
        private Part part;

        public ExtendedProjectKitItem(ProjectKitItem item, Part part)
        {
            this.item = item;
            this.part = part;
        }


        public int PartID { get { return item.PartID; } }

        public string JBMaterialID { get { return part.JBMaterialID; } }

        public string NSN { get { return part.NSN; } }

        public string PartNumber { get { return part.PartNumber; } }

        public string Description { get { return part.Description; } }

        public int ProjectKitItemID { get { return item.ProjectKitItemID; } }

        public int ProjectKitID { get { return item.ProjectKitID; } }

        public double Quantity { get { return item.Quantity; } }

        public Part ItemPart { get { return part; } }
    }
}