using System;
using Gdk;

namespace Eto.Platform.GtkSharp.CustomControls
{
	internal class AnalogClock : Gtk.DrawingArea
	{
		double radius;
		double hourLength, minuteLength, secondLength;
		Cairo.Color hourColor = new Cairo.Color(0.23, 0.23, 0.23, 0.9);
		Cairo.Color minuteColor = new Cairo.Color(0.23, 0.23, 0.23);
		Cairo.Color secondColor = new Cairo.Color(0.56, 0.54, 0.4, 0.5);
		Cairo.Color ticksColor = new Cairo.Color(0.56, 0.54, 0.4);
		Cairo.PointD center;
		DateTime time;
		
		public AnalogClock ()
		{
			time = DateTime.Now;
			QueueResize ();
		}

		public DateTime Time {
			get {
				return time;
			}
			set {
				if (time != value) {
					time = value;
					QueueDraw ();
				}
			}
		}
		
		void DrawFace (Cairo.PointD center, double radius, Cairo.Context e)
		{
			e.Arc (center.X, center.Y, radius, 0, 360);
			Cairo.Gradient pat = new Cairo.LinearGradient (100, 200, 200, 100);
			pat.AddColorStop (0, Generator.ConvertC (new Eto.Drawing.Color (240, 240, 230, 75)));
			pat.AddColorStop (1, Generator.ConvertC (new Eto.Drawing.Color (0, 0, 0, 50)));
			e.LineWidth = 0.1;
			e.Pattern = pat;
			e.FillPreserve ();
			e.Stroke ();
		}
		
		void DrawHand (double fThickness, double length, Cairo.Color color, double radians, Cairo.Context e)
		{
			e.MoveTo (new Cairo.PointD (center.X - (length / 9 * Math.Sin (radians)), center.Y + (length / 9 * Math.Cos (radians))));
			e.LineTo (new Cairo.PointD (center.X + (length * Math.Sin (radians)), center.Y - (length * Math.Cos (radians))));
			e.ClosePath ();
			e.LineCap = Cairo.LineCap.Round;
			e.LineJoin = Cairo.LineJoin.Round;
			e.Color = color;
			e.LineWidth = fThickness;
			e.Stroke ();
		}
		
		protected override void OnSizeAllocated (Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			radius = allocation.Height / 2;
			center = new Cairo.PointD(allocation.Width / 2, allocation.Height / 2);
			hourLength = allocation.Height / 3 / 1.65F;
			minuteLength = allocation.Height / 3 / 1.20F;
			secondLength = allocation.Height / 3 / 1.15F;
		}
		
		protected override bool OnExposeEvent (EventExpose evnt)
		{
			base.OnExposeEvent (evnt);
			using (var e = CairoHelper.Create (evnt.Window))
			{
				var hourRadians = (time.Hour % 12 + time.Minute / 60F) * 30 * Math.PI / 180;
				DrawHand (3, hourLength, hourColor, hourRadians, e);

				var minuteRadians = (time.Minute) * 6 * Math.PI / 180;
				DrawHand (2, minuteLength, minuteColor, minuteRadians, e);

				var secondRadians = (time.Second) * 6 * Math.PI / 180;
				DrawHand (1, secondLength, secondColor, secondRadians, e);
				
				for (int i = 0; i < 60; i++) {
					if (i % 5 == 0) {
						var p1 = new Cairo.PointD (center.X + (radius / 1.50 * Math.Sin (i * 6 * Math.PI / 180)), center.Y - (radius / 1.50 * Math.Cos (i * 6 * Math.PI / 180)));
						var p2 = new Cairo.PointD (center.X + (radius / 1.65 * Math.Sin (i * 6 * Math.PI / 180)), center.Y - (radius / 1.65 * Math.Cos (i * 6 * Math.PI / 180)));
						e.LineWidth = 1;
						e.Color = ticksColor;
						e.MoveTo (p1);
						e.LineTo (p2);
						e.ClosePath ();
						e.Stroke ();
					} else {
						
						var p1 = new Cairo.PointD (center.X + (radius / 1.50 * Math.Sin (i * 6 * Math.PI / 180)), center.Y - (radius / 1.50 * Math.Cos (i * 6 * Math.PI / 180)));
						var p2 = new Cairo.PointD ( center.X + (radius / 1.55 * Math.Sin (i * 6 * Math.PI / 180)), center.Y - (radius / 1.55 * Math.Cos (i * 6 * Math.PI / 180)));
						e.LineWidth = 1;
						e.Color = ticksColor;
						e.MoveTo (p1);
						e.LineTo (p2);
						e.ClosePath ();
						e.Stroke ();
						
					}
				}
				DrawFace (center, (radius / 2) + 17, e);
				DrawFace (center, 8, e);
				
			}
			return true;			
		}
	}
}

