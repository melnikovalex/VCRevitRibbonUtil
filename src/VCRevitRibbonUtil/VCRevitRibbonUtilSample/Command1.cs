/*
 * Copyright 2012 © Victor Chekalin
 *
 * THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 *
 */

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace VCRevitRibbonUtilSample
{
    [Transaction(TransactionMode.ReadOnly)]
    public class Command1 : VCRevitRibbonUtil.CommandDescription, IExternalCommand
    {
        new public static bool Available = false;

        new public static string Title = "not available";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("VC", "Command1");

            return Result.Succeeded;
        }
    }
}