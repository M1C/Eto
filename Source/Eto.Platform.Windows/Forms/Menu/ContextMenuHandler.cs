using System;
using SWF = System.Windows.Forms;
using Eto.Forms;

namespace Eto.Platform.Windows
{
	public class ContextMenuHandler : WidgetHandler<SWF.ContextMenuStrip, ContextMenu>, IContextMenu
	{
		
		public ContextMenuHandler ()
		{
			this.Control = new System.Windows.Forms.ContextMenuStrip ();
		}
		
		public void AddMenu (int index, MenuItem item)
		{
			Control.Items.Insert (index, (SWF.ToolStripItem)item.ControlObject);
		}

		public void RemoveMenu (MenuItem item)
		{
			Control.Items.Remove ((SWF.ToolStripItem)item.ControlObject);
		}

		public void Clear ()
		{
			Control.Items.Clear ();
		}
		
		public void Show (Control relativeTo)
		{
			if (relativeTo != null) {
				var control = relativeTo.ControlObject as SWF.Control;
				this.Control.Show (control, 0, 0);
			} else
				this.Control.Show ();
		}
	}
}
