using System;
using MonoMac.AppKit;
using Eto.Forms;
using MonoMac.Foundation;
using Eto.Drawing;

namespace Eto.Platform.Mac.Forms.Controls
{
	public class DateTimePickerHandler : MacControl<NSDatePicker, DateTimePicker>, IDateTimePicker
	{
		DateTime? curValue;
		DateTimePickerMode mode;
		
		public class EtoDatePicker : NSDatePicker, IMacControl
		{
			public override void DrawRect (System.Drawing.RectangleF dirtyRect)
			{
				if (Handler.curValue != null)
					base.DrawRect (dirtyRect);
				else {
					// paint with no elements visible
					var old = this.DatePickerElements;
					this.DatePickerElements = 0;
					base.DrawRect (dirtyRect);
					this.DatePickerElements = old;
				}
			}
			
			public DateTimePickerHandler Handler { get; set; }
			
			object IMacControl.Handler { get { return Handler; } }
		}
		
		public DateTimePickerHandler ()
		{
			Control = new EtoDatePicker { Handler = this };
			Control.TimeZone = NSTimeZone.LocalTimeZone;
			Control.Calendar = NSCalendar.CurrentCalendar;
			Control.DateValue = Generator.Convert (DateTime.Now);
			this.Mode = DateTimePicker.DefaultMode;
		}
		
		protected override Size GetNaturalSize ()
		{
			return Size.Max (new Size (mode == DateTimePickerMode.DateTime ? 180 : 120, 10), base.GetNaturalSize ());
		}
		
		public override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			
			// apple+backspace clears the value
			Widget.KeyDown += delegate(object sender, KeyPressEventArgs ev) {
				if (!ev.Handled) {
					if (ev.KeyData == (Key.Application | Key.Backspace)) {
						curValue = null;
						Widget.OnValueChanged (EventArgs.Empty);
						Control.NeedsDisplay = true;
					}
				}
			};
			// when clicking, set the value if it is null
			Widget.MouseDown += delegate(object sender, MouseEventArgs ev) {
				if (ev.Buttons == MouseButtons.Primary) {
					if (curValue == null) {
						curValue = Generator.Convert (Control.DateValue);
						Widget.OnValueChanged (EventArgs.Empty);
						Control.NeedsDisplay = true;
					}
				}
			};
			Control.ValidateProposedDateValue += delegate(object sender, NSDatePickerValidatorEventArgs ev) {
				var date = Generator.Convert (ev.ProposedDateValue);
				if (date != Generator.Convert (Control.DateValue)) {
					curValue = date;
					Widget.OnValueChanged (EventArgs.Empty);
				}
			};
		}
		
		public DateTimePickerMode Mode {
			get { return mode; }
			set {
				mode = value;
				switch (mode) {
				case DateTimePickerMode.Date:
					Control.DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay;
					break;
				case DateTimePickerMode.Time:
					Control.DatePickerElements = NSDatePickerElementFlags.HourMinuteSecond;
					break;
				case DateTimePickerMode.DateTime:
					Control.DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay | NSDatePickerElementFlags.HourMinuteSecond;
					break;
				default:
					throw new NotSupportedException ();
				}
			}
		}
		
		public DateTime MinDate {
			get {
				return Generator.Convert (Control.MinDate) ?? DateTime.MinValue;
			}
			set {
				Control.MinDate = Generator.Convert (value);
			}
		}
		
		public DateTime MaxDate {
			get {
				return Generator.Convert (Control.MaxDate) ?? DateTime.MaxValue;
			}
			set {
				Control.MaxDate = Generator.Convert (value);
			}
		}

		public DateTime? Value {
			get {
				return curValue;
			}
			set {
				curValue = value;
				if (value != null)
					Control.DateValue = Generator.Convert (value);
				else
					Control.DateValue = Generator.Convert (DateTime.Now);
			}
		}

	}
}

