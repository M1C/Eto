﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using swc = System.Windows.Controls;
using swm = System.Windows.Media;
using swi = System.Windows.Input;

namespace Eto.Platform.Wpf.Forms.Menu
{
	public class RadioMenuItemHandler : WpfMenuItem<swc.MenuItem, RadioMenuItem>, IRadioMenuItem
	{
		List<RadioMenuItem> group;

		public RadioMenuItemHandler ()
		{
			Control = new swc.MenuItem {
				IsCheckable = true
			};
			Setup ();
		}


		public bool Checked
		{
			get { return Control.IsChecked; }
			set { Control.IsChecked = value; }
		}

		public void Create (RadioMenuItem controller)
		{
			if (controller != null) {
				RadioMenuItemHandler controllerInner = (RadioMenuItemHandler)controller.Handler;
				if (controllerInner.group == null) {
					controllerInner.group = new List<RadioMenuItem> ();
					controllerInner.group.Add (controller);
					controllerInner.Control.Click += controllerInner.control_RadioSwitch;
				}
				controllerInner.group.Add (this.Widget);
				Control.Click += controllerInner.control_RadioSwitch;
			}
		}

		void control_RadioSwitch (object sender, EventArgs e)
		{
			if (group != null) {
				foreach (RadioMenuItem item in group) {
					item.Checked = (item.ControlObject == sender);
				}
			}
		}
	}
}
