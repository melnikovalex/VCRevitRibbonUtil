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
using System.Drawing;
using System.Reflection;
using System.Windows.Media;
using Autodesk.Revit.UI;
using VCRevitRibbonUtil.Helpers;

namespace VCRevitRibbonUtil
{
    public class Button : VCRibbonItem
    {
        protected readonly string _name;
        protected readonly string _text;
        private readonly string _className;
        protected ImageSource _largeImage;
        protected ImageSource _smallImage;
        protected string _longDescription;
        protected string _toolTip;
        private readonly string _assemblyLocation;

        private readonly int sizeSmall = 16;
        private readonly int sizeLarge = 32;
        protected ContextualHelp _contextualHelp;

        internal bool alwaysAvailable = false;
        private readonly Type _externalCommandType;

        public Button(string name,
                      string text,
                      Type externalCommandType)
        {
            _name = name;
            _text = text;
            _externalCommandType = externalCommandType;

            if (_externalCommandType != null)
            {
                _className = _externalCommandType.FullName;
                _assemblyLocation = _externalCommandType.Assembly.Location;

                if (_name == null)
                    _name = _className;

                if (_text == null)
                {
                    FieldInfo fieldTitle = _externalCommandType.GetField("Title");
                    if (fieldTitle != null) _text = fieldTitle.GetValue(_externalCommandType).ToString();
                }
                if (_text == null)
                    _text = _className;
            }
        }

        public Button SetLargeImage(ImageSource largeImage)
        {
            _largeImage = largeImage;
            return this;
        }

        public Button SetLargeImage(Bitmap largeImage, bool autoScale = false)
        {
            if (autoScale)
                _largeImage = BitmapSourceConverter.ConvertFromImage(Resizer.ResizeImage(largeImage, sizeLarge, sizeLarge));
            else
                _largeImage = BitmapSourceConverter.ConvertFromImage(largeImage);
            return this;
        }

        public Button SetSmallImage(ImageSource smallImage)
        {
            _smallImage = smallImage;
            return this;
        }

        public Button SetSmallImage(Bitmap smallImage, bool autoScale = false)
        {
            if (autoScale)
                _smallImage = BitmapSourceConverter.ConvertFromImage(Resizer.ResizeImage(smallImage, sizeSmall, sizeSmall));
            else
                _smallImage = BitmapSourceConverter.ConvertFromImage(smallImage);
            return this;
        }

        public Button SetImage(Bitmap image)
        {
            _smallImage = BitmapSourceConverter.ConvertFromImage(Resizer.ResizeImage(image, sizeSmall, sizeSmall));
            _largeImage = BitmapSourceConverter.ConvertFromImage(Resizer.ResizeImage(image, sizeLarge, sizeLarge));
            return this;
        }

        internal virtual ButtonData Finish()
        {
            PushButtonData pushButtonData =
                 new PushButtonData(_name,
                                    _text,
                                    _assemblyLocation,
                                    _className);

            if (_externalCommandType != null)
            {
                // if values were not filled, override them with field values of Command class (if it contains certain properties)
                if (_externalCommandType.IsSubclassOf(typeof(CommandDescription)))
                {
                    if (_longDescription == null)
                    {
                        FieldInfo fieldLongDescription = _externalCommandType.GetField("LongDescription");
                        if (fieldLongDescription != null) _longDescription = fieldLongDescription.GetValue(_externalCommandType).ToString();
                    }
                    if (_toolTip == null)
                    {
                        FieldInfo fieldTooltip = _externalCommandType.GetField("ToolTip");
                        if (fieldTooltip != null) _toolTip = fieldTooltip.GetValue(_externalCommandType).ToString();
                    }

                    if (_largeImage == null && _smallImage == null)
                    {
                        FieldInfo fieldImage = _externalCommandType.GetField("Image");
                        if (fieldImage != null)
                            SetImage(fieldImage.GetValue(_externalCommandType) as Bitmap);
                    }

                    if (_largeImage == null && _smallImage == null)
                    {
                        FieldInfo fieldLargeImage = _externalCommandType.GetField("LargeImage");
                        FieldInfo fieldSmallImage = _externalCommandType.GetField("SmallImage");
                        if (fieldLargeImage != null)
                            SetLargeImage(fieldLargeImage.GetValue(_externalCommandType) as Bitmap,
                                fieldSmallImage == null);
                        if (fieldSmallImage != null)
                            SetSmallImage(fieldSmallImage.GetValue(_externalCommandType) as Bitmap,
                                fieldLargeImage == null);
                    }

                    if (_contextualHelp == null)
                    {
                        FieldInfo fieldHelpUrl = _externalCommandType.GetField("HelpUrl");
                        if (fieldHelpUrl != null) SetHelpUrl(fieldHelpUrl.GetValue(_externalCommandType).ToString());
                    }

                    FieldInfo fieldAlwaysAvailable = _externalCommandType.GetField("AlwaysAvailable");
                    if (fieldAlwaysAvailable != null && (bool)fieldAlwaysAvailable.GetValue(_externalCommandType))
                        AlwaysAvailable();
                }
            }
            if (_largeImage != null)
            {
                pushButtonData.LargeImage = _largeImage;
            }

            if (_smallImage != null)
            {
                pushButtonData.Image = _smallImage;
            }

            if (_toolTip != null)
            {
                pushButtonData.ToolTip = _toolTip;
            }

            if (_longDescription != null)
            {
                pushButtonData.LongDescription = _longDescription;
            }

            if (_contextualHelp != null)
            {
                pushButtonData.SetContextualHelp(_contextualHelp);
            }

            return pushButtonData;
        }

        public Button SetLongDescription(string description)
        {
            _longDescription = description;

            return this;
        }

        public Button SetToolTip(string toolTip)
        {
            _toolTip = toolTip;

            return this;
        }

        public Button SetContextualHelp(ContextualHelpType contextualHelpType, string helpPath)
        {
            _contextualHelp = new ContextualHelp(contextualHelpType, helpPath);

            return this;
        }

        public Button SetHelpUrl(string url)
        {
            _contextualHelp = new ContextualHelp(ContextualHelpType.Url, url);

            return this;
        }

        public Button AlwaysAvailable()
        {
            alwaysAvailable = true;
            return this;
        }

        [Obsolete]
        public Button SetAvailability(string x)
        {
            AlwaysAvailable();
            return this;
        }
    }
}