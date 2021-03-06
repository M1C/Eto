using System;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.Platform.GtkSharp
{
	public class LabelHandler : GtkControl<Gtk.EventBox, Label>, ILabel
	{
		WrapLabel label;
		HorizontalAlign horizontalAlign = HorizontalAlign.Left;
		VerticalAlign verticalAlign = VerticalAlign.Top;
		
		class WrapLabel : Gtk.Label
		{
			int wrapWidth = 0;
			
			protected override void OnSizeRequested (ref Gtk.Requisition requisition)
			{
				//base.OnSizeRequested (ref requisition);
				int width, height;
				this.Layout.GetPixelSize(out width, out height);
				requisition.Width = width;
				requisition.Height = height;
			}
			
			protected override void OnSizeAllocated (Gdk.Rectangle allocation)
			{
				base.OnSizeAllocated (allocation);
				SetWrapWidth(allocation.Width);
			}
			
			void SetWrapWidth(int width)
			{
				if (width == 0) return;
				Layout.Width = (int)(width * Pango.Scale.PangoScale);
				if (wrapWidth != width) {
					wrapWidth = width;
					QueueResize ();
				}
			}
		}

		public LabelHandler ()
		{
			Control = new Gtk.EventBox();
			Control.VisibleWindow = false;
			label = new WrapLabel {
				SingleLineMode = false,
				LineWrap = true,
				LineWrapMode = Pango.WrapMode.Word
			};
			label.SetAlignment (0, 0);
			Control.Child = label;
		}
		
		public WrapMode Wrap {
			get {
				if (!label.LineWrap)
					return WrapMode.None;
				else if (label.LineWrapMode == Pango.WrapMode.Word)
					return WrapMode.Word;
				else if (label.LineWrapMode == Pango.WrapMode.Char)
					return WrapMode.Character;
				else 
					return WrapMode.Character;
			}
			set {
				switch (value) {
				case WrapMode.None: 
					label.Wrap = false;
					label.LineWrap = false;
					label.SingleLineMode = true;
					break;
				case WrapMode.Word:
					label.Wrap = true;
					label.LineWrapMode = Pango.WrapMode.Word;
					label.LineWrap = true;
					label.SingleLineMode = false;
					break;
				case WrapMode.Character:
					label.Wrap = true;
					label.LineWrapMode = Pango.WrapMode.Char;
					label.LineWrap = true;
					label.SingleLineMode = false;
					break;
				default:
					throw new NotSupportedException();
				}
			}
		}
		
		public virtual Color TextColor {
			get { return Generator.Convert (label.Style.Foreground (Gtk.StateType.Normal)); }
			set { label.ModifyFg (Gtk.StateType.Normal, Generator.Convert (value)); }
		}

		public override string Text {
			get { return MnuemonicToString (label.Text); }
			set { label.TextWithMnemonic = StringToMnuemonic (value); }
		}

		public HorizontalAlign HorizontalAlign {
			get { return horizontalAlign; }
			set {
				horizontalAlign = value;
				SetAlignment ();
			}
		}

		void SetAlignment ()
		{
			float xalignment = 0;
			float yalignment = 0;
			Gtk.Justification justify;
			switch (horizontalAlign) {
			default:
			case Eto.Forms.HorizontalAlign.Left:
				xalignment = 0F;
				justify = Gtk.Justification.Left;
				break;
			case Eto.Forms.HorizontalAlign.Center:
				xalignment = 0.5F;
				justify = Gtk.Justification.Center;
				break;
			case Eto.Forms.HorizontalAlign.Right:
				xalignment = 1F;
				justify = Gtk.Justification.Right;
				break;
			}
			switch (verticalAlign) {
			case Eto.Forms.VerticalAlign.Middle:
				yalignment = 0.5F;
				break;
			case Eto.Forms.VerticalAlign.Top:
				yalignment = 0F;
				break;
			case Eto.Forms.VerticalAlign.Bottom:
				yalignment = 1F;
				break;
			}
			label.SetAlignment(xalignment, yalignment);
			label.Justify = justify;
		}

		public VerticalAlign VerticalAlign {
			get { return verticalAlign; }
			set {
				verticalAlign = value;
				SetAlignment ();
				
			}
		}
	}
}
