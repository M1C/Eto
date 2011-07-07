﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;

namespace Eto.Platform.Wpf.Forms.Controls
{
	public class ButtonHandler : WpfControl<System.Windows.Controls.Button, Button>, IButton
	{
		public ButtonHandler ()
		{
			Control = new System.Windows.Controls.Button ();
			Control.Click += delegate {
				Widget.OnClick (EventArgs.Empty);
			};
		}

		public string Text
		{
			get { return Control.Content as string; }
			set { Control.Content = value; }
		}
	}
}