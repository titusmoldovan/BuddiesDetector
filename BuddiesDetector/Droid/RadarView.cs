using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace BuddiesDetector.Droid
{
	public class RadarView : View
	{
		private static int POINT_ARRAY_SIZE = 25;

		private int fps = 100;
		private bool showCircles = true;

		float alpha = 0;
		Point[] latestPoints = new Point[POINT_ARRAY_SIZE];
		Paint[] latestPaint = new Paint[POINT_ARRAY_SIZE];

		Action Action;
		Runnable Tick;

		public RadarView(Context context) : base(context)
		{
			Paint localPaint = new Paint();
			localPaint.SetARGB(0, 0, 1, 0);
			localPaint.SetStyle(Paint.Style.Stroke);
			localPaint.StrokeWidth = 1.0F;
			//SetColor(Color.GREEN);
			//localPaintSetAntiAlias(true);
			//localPaint.setAlpha(0);

			int alpha_step = 255 / POINT_ARRAY_SIZE;
			for (int i = 0; i < latestPaint.Length; i++)
			{
				latestPaint[i] = new Paint(localPaint);
				latestPaint[i].Alpha = 255 - (i * alpha_step);
			}

			Android.OS.Handler Handler = new Android.OS.Handler();
			Action = new Action(HandleAction);
			Tick = new Runnable(Action);
		}

		public RadarView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Paint localPaint = new Paint();
			localPaint.SetARGB(0, 0, 1, 0);
			localPaint.SetStyle(Paint.Style.Stroke);
			localPaint.StrokeWidth = 1.0F;
			//SetColor(Color.GREEN);
			//localPaintSetAntiAlias(true);
			//localPaint.setAlpha(0);

			int alpha_step = 255 / POINT_ARRAY_SIZE;
			for (int i = 0; i < latestPaint.Length; i++)
			{
				latestPaint[i] = new Paint(localPaint);
				latestPaint[i].Alpha = 255 - (i * alpha_step);
			}

			Android.OS.Handler Handler = new Android.OS.Handler();
			Action = new Action(HandleAction);
			Tick = new Runnable(Action);
		}

		public RadarView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{

			Paint localPaint = new Paint();
			localPaint.SetARGB(0, 0, 1, 0);
			localPaint.SetStyle(Paint.Style.Stroke);
			localPaint.StrokeWidth = 1.0F;
			//SetColor(Color.GREEN);
			//localPaintSetAntiAlias(true);
			//localPaint.setAlpha(0);

			int alpha_step = 255 / POINT_ARRAY_SIZE;
			for (int i = 0; i < latestPaint.Length; i++)
			{
				latestPaint[i] = new Paint(localPaint);
				latestPaint[i].Alpha = 255 - (i * alpha_step);
			}

			Android.OS.Handler Handler = new Android.OS.Handler();
			Action = new Action(HandleAction);
			Tick = new Runnable(Action);
		}

		void HandleAction()
		{
			Invalidate();
			Handler.PostDelayed(new Runnable(HandleAction), 1000 / fps);
		}

		public void startAnimation()
		{
			Handler.RemoveCallbacks(Tick);
			Handler.Post(Tick);
		}

		public void stopAnimation()
		{
			Handler.RemoveCallbacks(Tick);
		}

		public void setFrameRate(int fps) { this.fps = fps; }
		public int getFrameRate() { return this.fps; }

		public void setShowCircles(bool showCircles) { this.showCircles = showCircles; }

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			var r = Java.Lang.Math.Min(Width, Height);

			//canvas.drawRect(0, 0, getWidth(), getHeight(), localPaint);

			int i = r / 2;
			int j = i - 1;
			Paint localPaint = latestPaint[0]; // GREEN

			if (showCircles)
			{
				canvas.DrawCircle(i, i, j, localPaint);
				canvas.DrawCircle(i, i, j, localPaint);
				canvas.DrawCircle(i, i, j * 3 / 4, localPaint);
				canvas.DrawCircle(i, i, j >> 1, localPaint);
				canvas.DrawCircle(i, i, j >> 2, localPaint);
			}

			alpha -= (float)0.5;

			if (alpha < -360) alpha = 0;
			double angle = Java.Lang.Math.ToRadians(alpha);
			int offsetX = (int)(i + (float)(i * Java.Lang.Math.Cos(angle)));
			int offsetY = (int)(i - (float)(i * Java.Lang.Math.Sin(angle)));

			latestPoints[0] = new Point(offsetX, offsetY);

			for (int x = POINT_ARRAY_SIZE - 1; x > 0; x--)
			{
				latestPoints[x] = latestPoints[x - 1];
			}

			int lines = 0;
			for (int x = 0; x < POINT_ARRAY_SIZE; x++)
			{
				Point point = latestPoints[x];
				if (point != null)
				{
					canvas.DrawLine(i, i, point.X, point.Y, latestPaint[x]);
				}
			}


			lines = 0;
			foreach (Point p in latestPoints) if (p != null) lines++;

			bool debug = false;
			if (debug)
			{
				StringBuilder sb = new StringBuilder(" >> ");
				foreach (Point p in latestPoints)
				{
					if (p != null) sb.Append(" (" + p.X + "x" + p.Y + ")");
				}

				//  " - R:" + r + ", i=" + i +
				//  " - Size: " + width + "x" + height +
				//  " - Angle: " + angle +
				//  " - Offset: " + offsetX + "," + offsetY);
			}

		}
	}
}