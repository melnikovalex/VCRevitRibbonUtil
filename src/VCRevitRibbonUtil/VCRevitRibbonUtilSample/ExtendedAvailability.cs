using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace VCRevitRibbonUtilSample
{
    public class ExtendAvailabilityZeroDocuments : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(
          UIApplication a,
          Autodesk.Revit.DB.CategorySet b)
        {
            return true;
        }
    }

    public class ExtendAvailabilityDisabled : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(
          UIApplication a,
          Autodesk.Revit.DB.CategorySet b)
        {
            return false;
        }
    }
}