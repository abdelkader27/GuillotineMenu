using System;
using Android.Widget;
using Android.Content;
using Android.Util;

namespace Guillotine.Menu
{
	public class CanaroTextView : TextView
	{

		public CanaroTextView(Context context):this(context,null) {
	    }

		public CanaroTextView(Context context, IAttributeSet attrs):this(context, attrs, 0) {
		}

		public CanaroTextView(Context context, IAttributeSet attrs, int defStyleAttr):base(context,attrs,defStyleAttr) {

			Typeface = Android.Graphics.Typeface.CreateFromAsset (context.Assets, "Fonts/canaro_extra_bold.otf");
		}
	}
}

