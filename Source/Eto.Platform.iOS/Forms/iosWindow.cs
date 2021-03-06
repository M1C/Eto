using System;
using MonoTouch.UIKit;
using Eto.Forms;
using Eto.Platform.iOS.Forms.Controls;
using Eto.Drawing;

namespace Eto.Platform.iOS.Forms
{
	public abstract class iosWindow<T, W> : iosContainer<T, W>, IWindow
		where T: UIView
		where W: Window
	{

		public iosWindow ()
		{
		}
		
		public Point Location {
			get {
				return Generator.ConvertF(this.Control.Frame.Location);
			}
			set {
				var frame = this.Control.Frame;
				frame.Location = Generator.ConvertF (value);
				this.Control.Frame = frame;
			}
		}
		
		#region IWindow implementation
		
		public virtual void Close ()
		{
		}

		public ToolBar ToolBar {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region ITextControl implementation
		
		public abstract string Text { get; set; }

		#endregion


	}
}

