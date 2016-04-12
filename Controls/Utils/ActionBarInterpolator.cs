using System;
using Android.Animation;

namespace Guillotine.Menu
{
	public class ActionBarInterpolator:Java.Lang.Object, ITimeInterpolator
	{
		private const float FIRST_BOUNCE_PART = 0.375f;
		private const float SECOND_BOUNCE_PART = 0.625f;

		#region ITimeInterpolator implementation

		public float GetInterpolation (float input)
		{
			if (input < FIRST_BOUNCE_PART) {
				return (-28.4444f) * input * input + 10.66667f * input;
			} else if (input < SECOND_BOUNCE_PART) {
				return (21.33312f) * input * input - 21.33312f * input + 4.999950f;
			} else { 
				return (-9.481481f) * input * input + 15.40741f * input - 5.925926f; 
			} 
		}

		#endregion
	}
}

