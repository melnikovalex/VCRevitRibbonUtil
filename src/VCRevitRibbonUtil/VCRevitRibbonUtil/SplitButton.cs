using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace VCRevitRibbonUtil
{
    public class SplitButton : Button
    {
        //new protected readonly string _name;
        //protected readonly string _text;
        //private readonly string _className;
        //protected string _description;
        //private string _assemblyLocation;

        //protected ContextualHelp _contextualHelp;

        private readonly IList<VCRibbonItem> _items = new List<VCRibbonItem>();
        private readonly dynamic _parentElement;

        public SplitButton(dynamic parentElement, string name, string text) :
            base(name, text, null)
        {
            _parentElement = parentElement;
        }

        internal override ButtonData Finish()
        {
            SplitButtonData splitButtonData = new SplitButtonData(_name, _text);

            if (_largeImage != null)
            {
                splitButtonData.LargeImage = _largeImage;
            }

            if (_smallImage != null)
            {
                splitButtonData.Image = _smallImage;
            }

            if (_longDescription != null)
            {
                splitButtonData.LongDescription = _longDescription;
            }

            if (_contextualHelp != null)
            {
                splitButtonData.SetContextualHelp(_contextualHelp);
            }

            return splitButtonData;
        }

        public SplitButton CreateButton<TExternalCommandClass>(Action<Button> action = null)
                        where TExternalCommandClass : CommandDescription, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(null, null, commandClassType, action);
        }

        public SplitButton CreateButton<TExternalCommandClass>(string name,
                          string text)
                        where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, null);
        }

        public SplitButton CreateButton<TExternalCommandClass>(string name,
                                  string text,
                                  Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        public SplitButton CreateButton(string name,
                                  string text,
                                  Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        public SplitButton CreateButton(string name,
                                   string text,
                                   Type externalCommandType,
                                   Action<Button> action)
        {
            var button = new Button(name,
                              text,
                              externalCommandType);
            if (action != null)
            {
                action.Invoke(button);
            }

            Items.Add(button);

            return this;
        }
        public SplitButton CreateSeparator()
        {
            Items.Add(new Separator());
            return this;
        }
        public IList<VCRibbonItem> Items
        {
            get { return _items; }
        }

        internal void BuildButtons(Autodesk.Revit.UI.SplitButton splitButton)
        {
            foreach (var item in Items)
            {
                if (item is Button)
                {
                    var button = item as Button; var buttonData = button.Finish();
                    if (buttonData is PushButtonData && button.alwaysAvailable != null)
                    {
                        if (button.alwaysAvailable == true && _parentElement._classNameAvailable != null)
                            (buttonData as PushButtonData).AvailabilityClassName = _parentElement._classNameAvailable;
                        if (button.alwaysAvailable == false && _parentElement._classNameDisabled != null)
                            (buttonData as PushButtonData).AvailabilityClassName = _parentElement._classNameDisabled;
                    }
                    while (_parentElement.commandNamesTaken.Contains(buttonData.Name))
                    {
                        buttonData.Name += "_";
                    }
                    _parentElement.commandNamesTaken.Add(buttonData.Name);

                    // Add break lines
                    if (_parentElement._autoLineBreaks)
                    {
                        buttonData.Text = LineBreaks.Format(buttonData.Text);
                    }

                    splitButton.AddPushButton(buttonData as PushButtonData);
                } else if (item is Separator)
                {
                    splitButton.AddSeparator();
                }
            }
        }
    }
}