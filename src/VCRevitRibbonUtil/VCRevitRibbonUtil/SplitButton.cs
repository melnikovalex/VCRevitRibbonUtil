using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace VCRevitRibbonUtil
{
	class SplitButton : Button
	{
		//new protected readonly string _name;
		//protected readonly string _text;
		//private readonly string _className;
		//protected string _description;
		//private string _assemblyLocation;

		//protected ContextualHelp _contextualHelp;

		public SplitButton(string name, string text) :
			base(name, text, null)
		{
		}
	}
}
