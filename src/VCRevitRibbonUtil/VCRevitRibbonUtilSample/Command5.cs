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
    public class Command5 : VCRevitRibbonUtil.CommandDescription, IExternalCommand
    {
        new public static string Title = "Command 5 long long long Title";

        new public static string LongDescription = "Description 5";

        new public static string ToolTip = "ToolTip";

        new public static System.Drawing.Bitmap Image = Properties.Resources._1348119615_internet_web_browser_32;

        new public static string HelpUrl = "https://google.com";

        new public static bool AlwaysAvailable = true;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("VC", "Command5");

            return Result.Succeeded;
        }
    }
}