using System;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.Test.Sections.Controls
{
	class LogGridItem : GridItem
	{
		public int Row { get; set; }

		public LogGridItem (params object[] values)
			: base (values)
		{
		}

		public override void SetValue (int column, object value)
		{
			base.SetValue (column, value);
			Log.Write (this, "SetValue, Row: {0}, Column: {1}, Value: {2}", Row, column, value);
		}
	}

	public class GridViewSection : Panel
	{
		public GridViewSection ()
		{
			var layout = new DynamicLayout (this);
			
			layout.AddRow (new Label { Text = "Default" }, Default ());
			layout.AddRow (new Label { Text = "No Header,\nNon-Editable" }, NoHeader ());
			layout.AddRow (new Label { Text = "Context Menu\n& Multi-Select" }, WithContextMenu ());
		}
		
		ComboBoxCell MyDropDown ()
		{
			var combo = new ComboBoxCell ();
			var items = new ListItemCollection ();
			items.Add (new ListItem{ Text = "Item 1" });
			items.Add (new ListItem{ Text = "Item 2" });
			items.Add (new ListItem{ Text = "Item 3" });
			items.Add (new ListItem{ Text = "Item 4" });
			combo.DataStore = items;
			return combo;
		}

		GridView Default ()
		{
			var control = new GridView {
				Size = new Size (300, 100)
			};
			LogEvents (control);
			
			var dropDown = MyDropDown ();
			control.Columns.Add (new GridColumn{ DataCell = new CheckBoxCell (), Editable = true, AutoSize = true, Resizable = false});
			control.Columns.Add (new GridColumn{ HeaderText = "Image", DataCell = new ImageCell () });
			control.Columns.Add (new GridColumn{ HeaderText = "Text", Editable = true});
			control.Columns.Add (new GridColumn{ HeaderText = "Drop Down", DataCell = dropDown, Editable = true });
			
			var image1 = Bitmap.FromResource ("Eto.Test.TestImage.png");
			var image2 = Icon.FromResource ("Eto.Test.TestIcon.ico");
			var items = new GridItemCollection ();
			var rand = new Random ();
			for (int i = 0; i < 10000; i++) {
				var val = rand.Next (3);
				var boolVal = val == 0 ? (bool?)false : val == 1 ? (bool?)true : null;

				val = rand.Next (3);
				var image = val == 0 ? (Image)image1 : val == 1 ? (Image)image2 : null;

				var txt = string.Format ("Col 1 Row {0}", i);
				
				val = rand.Next (dropDown.DataStore.Count + 1);
				var combo = val == 0 ? null : dropDown.DataStore [val - 1].Key;
				
				items.Add (new LogGridItem (boolVal, image, txt, combo){ Row = i });
			}
			control.DataStore = items;
			
			return control;
		}
		
		GridView NoHeader ()
		{
			var control = Default ();
			foreach (var col in control.Columns) {
				col.Editable = false;
			}
			control.ShowHeader = false;
			return control;
		}

		GridView WithContextMenu ()
		{
			var control = Default ();
			control.AllowMultipleSelection = true;
			
			var menu = new ContextMenu ();
			var item = new ImageMenuItem{ Text = "Click Me!"};
			item.Click += delegate {
				if (control.SelectedRows.Count () > 0)
					Log.Write (item, "Click, Rows: {0}", SelectedRowsString (control));
				else
					Log.Write (item, "Click, no item selected");
			};
			menu.MenuItems.Add (item);
			
			control.ContextMenu = menu;
			return control;
		}

		void LogEvents (GridView control)
		{
			control.BeginCellEdit += (sender, e) => {
				Log.Write (control, "BeginCellEdit, Row: {0}, Column: {1}, Item: {2}, ColInfo: {3}", e.Row, e.Column, e.Item, e.GridColumn);
			};
			control.EndCellEdit += (sender, e) => {
				Log.Write (control, "EndCellEdit, Row: {0}, Column: {1}, Item: {2}, ColInfo: {3}", e.Row, e.Column, e.Item, e.GridColumn);
			};
			control.SelectionChanged += delegate {
				Log.Write (control, "Selection Changed, Rows: {0}", SelectedRowsString (control));
			};
		}

		string SelectedRowsString (GridView control)
		{
			return string.Join (",", control.SelectedRows.Select (r => r.ToString ()).OrderBy (r => r));
		}
	}
}

