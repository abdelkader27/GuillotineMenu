using System;
using Android.Animation;

namespace Guillotine.Menu
{
	public class GuillotineInterpolator:Java.Lang.Object, ITimeInterpolator
	{
		public const float ROTATION_TIME = 0.46667f;
		public const float FIRST_BOUNCE_TIME = 0.26666f;
		public const float SECOND_BOUNCE_TIME = 0.26667f;

		public GuillotineInterpolator ()
		{
		}

		#region ITimeInterpolator implementation

		public float GetInterpolation (float input)
		{
			if (input < ROTATION_TIME)
				return rotation (input);
			else if (input < ROTATION_TIME + FIRST_BOUNCE_TIME)
				return firstBounce (input);
			else
				return secondBounce (input); 
		}

		#endregion

		private float rotation (float t)
		{ 
			return 4.592f * t * t; 
		}

		private float firstBounce (float t)
		{ 
			return 2.5f * t * t - 3f * t + 1.85556f; 
		}

		private float secondBounce (float t)
		{ 
			return 0.625f * t * t - 1.08f * t + 1.458f; 
		}
	}
}

