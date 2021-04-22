using System;
using System.Collections.Generic;
using System.ComponentModel;
using Autodesk.Revit.UI;

namespace VCRevitRibbonUtil
{
    public class PulldownButton : Button
    {
        private readonly IList<VCRibbonItem> _items = new List<VCRibbonItem>();
        private readonly dynamic _parentElement;

        public PulldownButton(dynamic parentElement, string name, string text) :
            base(name, text, null)
        {
            _parentElement = parentElement;
        }

        internal override ButtonData Finish()
        {
            PulldownButtonData pulldownButtonData =
                new PulldownButtonData(_name,
                    _text);

            if (_largeImage != null)
            {
                pulldownButtonData.LargeImage = _largeImage;
            }

            if (_smallImage != null)
            {
                pulldownButtonData.Image = _smallImage;
            }

            if (_longDescription != null)
            {
                pulldownButtonData.LongDescription = _longDescription;
            }

            if (_contextualHelp != null)
            {
                pulldownButtonData.SetContextualHelp(_contextualHelp);
            }

            //pulldownButtonData.

            //_panel.Source.AddItem(pushButtonData);

            return pulldownButtonData;
        }

        public PulldownButton CreateButton<TExternalCommandClass>(Action<Button> action = null)
                        where TExternalCommandClass : CommandDescription, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(null, null, commandClassType, action);
        }

        public PulldownButton CreateButton<TExternalCommandClass>(string name,
                          string text)
                        where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, null);
        }

        public PulldownButton CreateButton<TExternalCommandClass>(string name,
                                  string text,
                                  Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        public PulldownButton CreateButton(string name,
                                  string text,
                                  Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        public PulldownButton CreateButton(string name,
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

        public PulldownButton CreateSeparator()
        {
            Items.Add(new Separator());
            return this;
        }

        public int ItemsCount
        {
            get { return Items.Count; }
        }

        public IList<VCRibbonItem> Items
        {
            get { return _items; }
        }

        internal void BuildButtons(Autodesk.Revit.UI.PulldownButton pulldownButton)
        {
            foreach (var item in Items)
            {
                if (item is Button)
                {
                    var button = item as Button;
                    var buttonData = button.Finish();
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

                    pulldownButton.AddPushButton(buttonData as PushButtonData);
                } else if (item is Separator)
                {
                    pulldownButton.AddSeparator();
                }
                
            }
        }
    }
}