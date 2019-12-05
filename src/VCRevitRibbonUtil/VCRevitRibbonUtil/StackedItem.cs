/*
 * Copyright 2012 © Victor Chekalin
 *
 * THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 *
 */

using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace VCRevitRibbonUtil
{
    public class StackedItem : VCRibbonItem
    {
        private readonly Panel _panel;
        private readonly IList<Button> _buttons;
        internal readonly string _classNameAvailable;
        internal readonly string _classNameDisabled;
        internal List<string> commandNamesTaken;
        internal bool _autoLineBreaks = false;

        public StackedItem(Panel panel)
        {
            _panel = panel;
            _buttons = new List<Button>(3);
            _classNameAvailable = panel.Tab.Ribbon._classNameAvailable;
            _classNameDisabled = panel.Tab.Ribbon._classNameDisabled;
            _autoLineBreaks = panel.Tab.Ribbon._autoLineBreaks;
            commandNamesTaken = panel.Tab.Ribbon.commandNamesTaken;
        }

        public StackedItem CreateButton<TExternalCommandClass>(Action<Button> action = null)
            where TExternalCommandClass : CommandDescription, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(null, null, commandClassType, action);
        }

        public StackedItem CreateButton<TExternalCommandClass>(string name,
                                  string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, null);
        }

        public StackedItem CreateButton<TExternalCommandClass>(string name,
                                  string text,
                                  Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        public StackedItem CreateButton(string name,
                                  string text,
                                  Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        public StackedItem CreateButton(string name,
                                   string text,
                                   Type externalCommandType,
                                   Action<Button> action)
        {
            if (Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            var button = new Button(name,
                              text,
                              externalCommandType);
            if (action != null)
            {
                action.Invoke(button);
            }

            Buttons.Add(button);

            return this;
        }

        public StackedItem CreatePullDownButton(string name,
                                   string text,
                                   Action<PulldownButton> action)
        {
            if (Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            PulldownButton button = new PulldownButton(this, name, text);

            if (action != null)
            {
                action.Invoke(button);
            }

            var buttonData = button.Finish();

            while (_panel.Tab.Ribbon.commandNamesTaken.Contains(buttonData.Name))
            {
                buttonData.Name = buttonData.Name + "_";
            }
            _panel.Tab.Ribbon.commandNamesTaken.Add(buttonData.Name);

            Buttons.Add(button);

            return this;
        }

        public StackedItem CreateSplitButton(string name,
                           string text,
                           Action<SplitButton> action)
        {
            if (Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            SplitButton button = new SplitButton(this, name, text);

            if (action != null)
            {
                action.Invoke(button);
            }

            var buttonData = button.Finish();

            while (_panel.Tab.Ribbon.commandNamesTaken.Contains(buttonData.Name))
            {
                buttonData.Name = buttonData.Name + "_";
            }
            _panel.Tab.Ribbon.commandNamesTaken.Add(buttonData.Name);

            Buttons.Add(button);

            return this;
        }

        public int ItemsCount
        {
            get { return Buttons.Count; }
        }

        public IList<Button> Buttons
        {
            get { return _buttons; }
        }
    }
}