/* 
 * Copyright 2012 � Victor Chekalin
 * 
 * THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 * 
 */

using System;
using System.Drawing;
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
        protected string _description;
        private string _assemblyLocation;

		private readonly int sizeSmall = 16;
		private readonly int sizeLarge = 32;

		protected string _availabilityClassName;
		protected ContextualHelp _contextualHelp;

        public Button(string name, 
                      string text, 
                      Type externalCommandType)
        {
            _name = name;
            _text = text;

            if (externalCommandType != null)
            {
                _className = externalCommandType.FullName;
                _assemblyLocation =
                    externalCommandType.Assembly.Location;
            }
        }
       

        public Button SetLargeImage (ImageSource largeImage)
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

            if (_largeImage != null)
            {
                pushButtonData.LargeImage = _largeImage;
            }

            if (_smallImage != null)
            {
                pushButtonData.Image = _smallImage;
            }

            if (_description != null)
            {
                pushButtonData.LongDescription = _description;
            }

            if (_contextualHelp!=null)
            {
                pushButtonData.SetContextualHelp(_contextualHelp);
            }

			if(_availabilityClassName!=null)
			{
				pushButtonData.AvailabilityClassName = _availabilityClassName;
			}
            //_panel.Source.AddItem(pushButtonData);

            return pushButtonData;
        }

        public Button SetLongDescription(string description)
        {
            _description = description;

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

		public Button SetAvailability(string availabilityClassName)
		{
			_availabilityClassName = availabilityClassName;

			return this;
		}
	}
}