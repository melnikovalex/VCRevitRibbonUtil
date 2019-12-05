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
    public class Panel : VCRibbonItem
    {
        private readonly Tab _tab;
        private readonly RibbonPanel _panel;
        internal readonly string _classNameAvailable;
        internal readonly string _classNameDisabled;
        internal List<string> commandNamesTaken;
        internal bool _autoLineBreaks = false;

        public Panel(Tab tab, RibbonPanel panel)
        {
            _tab = tab;
            _panel = panel;
            _classNameAvailable = tab.Ribbon._classNameAvailable;
            _classNameDisabled = tab.Ribbon._classNameDisabled;
            _autoLineBreaks = tab.Ribbon._autoLineBreaks;
            commandNamesTaken = tab.Ribbon.commandNamesTaken;
        }

        internal RibbonPanel Source
        {
            get { return _panel; }
        }

        internal Tab Tab
        {
            get { return _tab; }
        }

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        public Panel CreateStackedItems(Action<StackedItem> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            StackedItem stackedItem = new StackedItem(this);

            action.Invoke(stackedItem);

            if (stackedItem.ItemsCount < 2 ||
                stackedItem.ItemsCount > 3)
            {
                throw new InvalidOperationException("You must create 2 or three items in the StackedItems");
            }

            List<RibbonItem> ribbonItems;
            var item1 = stackedItem.Buttons[0].Finish();
            if (item1 is PushButtonData && stackedItem.Buttons[0].alwaysAvailable != null)
            {
                if (stackedItem.Buttons[0].alwaysAvailable == true && _classNameAvailable != null)
                    (item1 as PushButtonData).AvailabilityClassName = _classNameAvailable;
                else if (stackedItem.Buttons[0].alwaysAvailable == false && _classNameDisabled != null)
                    (item1 as PushButtonData).AvailabilityClassName = _classNameDisabled;
            }
            while (_tab.Ribbon.commandNamesTaken.Contains(item1.Name))
            {
                item1.Name = item1.Name + "_";
            }
            _tab.Ribbon.commandNamesTaken.Add(item1.Name);

            var item2 = stackedItem.Buttons[1].Finish();
            if (stackedItem.Buttons[1].alwaysAvailable == true && _classNameAvailable != null)
                (item2 as PushButtonData).AvailabilityClassName = _classNameAvailable;
            else if (stackedItem.Buttons[1].alwaysAvailable == false && _classNameDisabled != null)
                (item2 as PushButtonData).AvailabilityClassName = _classNameDisabled;

            while (_tab.Ribbon.commandNamesTaken.Contains(item2.Name))
            {
                item2.Name = item2.Name + "_";
            }
            _tab.Ribbon.commandNamesTaken.Add(item2.Name);

            if (stackedItem.ItemsCount == 3)
            {
                var item3 =
                    stackedItem.Buttons[2].Finish();
                if (stackedItem.Buttons[2].alwaysAvailable == true && _classNameAvailable != null)
                    (item3 as PushButtonData).AvailabilityClassName = _classNameAvailable;
                else if (stackedItem.Buttons[2].alwaysAvailable == false && _classNameDisabled != null)
                    (item3 as PushButtonData).AvailabilityClassName = _classNameDisabled;

                while (_tab.Ribbon.commandNamesTaken.Contains(item3.Name))
                {
                    item3.Name = item3.Name + "_";
                }
                _tab.Ribbon.commandNamesTaken.Add(item3.Name);
                ribbonItems = _panel.AddStackedItems(item1, item2, item3) as List<RibbonItem>;
            }
            else
            {
                ribbonItems = _panel.AddStackedItems(item1, item2) as List<RibbonItem>;
            }

            foreach (RibbonItem ri in ribbonItems)
            {
                if (ri.GetType() == typeof(Autodesk.Revit.UI.PulldownButton))
                {
                    int i = ribbonItems.IndexOf(ri);
                    (stackedItem.Buttons[i] as PulldownButton).BuildButtons(ri as Autodesk.Revit.UI.PulldownButton);
                    (stackedItem.Buttons[i] as PulldownButton).RibbonItem = ri;
                }
                else if (ri.GetType() == typeof(Autodesk.Revit.UI.SplitButton))
                {
                    int i = ribbonItems.IndexOf(ri);
                    (stackedItem.Buttons[i] as SplitButton).BuildButtons(ri as Autodesk.Revit.UI.SplitButton);
                    (stackedItem.Buttons[i] as SplitButton).RibbonItem = ri;
                }
            }
            return this;
        }

        public Panel CreateButton<TExternalCommandClass>(Action<Button> action = null)
            where TExternalCommandClass : CommandDescription, IExternalCommand
        {
            return CreateButton<TExternalCommandClass>(null, null, action);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <returns>Panel where button were created</returns>
        public Panel CreateButton<TExternalCommandClass>(string name,
                                  string text) where TExternalCommandClass : class, IExternalCommand
        {
            return CreateButton<TExternalCommandClass>(name, text, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public Panel CreateButton<TExternalCommandClass>(string name,
                                  string text,
                                  Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        public Panel CreateButton(string name,
                                  string text,
                                  Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public Panel CreateButton(string name,
                                  string text,
                                  Type externalCommandType,
                                  Action<Button> action)
        {
            Button button = new Button(name,
                text,
                externalCommandType);
            if (action != null)
            {
                action.Invoke(button);
            }

            var buttonData = button.Finish();
            if (button.alwaysAvailable != null)
            {
                if (button.alwaysAvailable == true && this._classNameAvailable != null)
                    (buttonData as PushButtonData).AvailabilityClassName = this._classNameAvailable;
                if (button.alwaysAvailable == false && this._classNameDisabled != null)
                    (buttonData as PushButtonData).AvailabilityClassName = this._classNameDisabled;
            }

            while (Tab.Ribbon.commandNamesTaken.Contains(buttonData.Name))
            {
                buttonData.Name = buttonData.Name + "_";
            }
            Tab.Ribbon.commandNamesTaken.Add(buttonData.Name);

            // Add break lines
            if (Tab.Ribbon._autoLineBreaks)
            {
                buttonData.Text = LineBreaks.Format(buttonData.Text);
            }

            _panel.AddItem(buttonData);

            return this;
        }

        public Panel CreatePullDownButton(string name,
                                  string text,
                                  Action<PulldownButton> action)
        {
            PulldownButton button = new PulldownButton(this, name, text);

            if (action != null)
            {
                action.Invoke(button);
            }

            var buttonData = button.Finish();

            while (Tab.Ribbon.commandNamesTaken.Contains(buttonData.Name))
            {
                buttonData.Name = buttonData.Name + "_";
            }
            Tab.Ribbon.commandNamesTaken.Add(buttonData.Name);

            var ribbonItem = _panel.AddItem(buttonData) as Autodesk.Revit.UI.PulldownButton;

            button.BuildButtons(ribbonItem);

            button.RibbonItem = ribbonItem;

            return this;
        }

        public Panel CreateSplitButton(string name,
                          string text,
                          Action<SplitButton> action)
        {
            SplitButton button = new SplitButton(this, name, text);

            if (action != null)
            {
                action.Invoke(button);
            }

            var buttonData = button.Finish();

            while (Tab.Ribbon.commandNamesTaken.Contains(buttonData.Name))
            {
                buttonData.Name = buttonData.Name + "_";
            }
            Tab.Ribbon.commandNamesTaken.Add(buttonData.Name);

            var ribbonItem = _panel.AddItem(buttonData) as Autodesk.Revit.UI.SplitButton;

            button.BuildButtons(ribbonItem);

            button.RibbonItem = ribbonItem;

            return this;
        }

        //--

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        public Panel CreateSeparator()
        {
            _panel.AddSeparator();
            return this;
        }
    }
}