using System;
using Android.Views;
using Android.Animation;

namespace Guillotine.Menu
{
	public class GuillotineAnimation
	{
		private const string ROTATION = "rotation";
		private const float GUILLOTINE_CLOSED_ANGLE = -90f;
		private const float GUILLOTINE_OPENED_ANGLE = 0f;
		private const long DEFAULT_DURATION = 625;
		private const float ACTION_BAR_ROTATION_ANGLE = 3f;

		private View _guillotineView;
		private long _duration;
		private ObjectAnimator _openingAnimation;
		private ObjectAnimator _closingAnimation;
		private IGuillotineListener _listner;
		private bool _isRightToLeftLayout;
		private ITimeInterpolator _interpolator;
		private View _actionBarView;
		private long _delay;

		private bool _isOpening;
		private bool _isClosing;

		private View _openingView;
		private View _closingView;

		public GuillotineAnimation (GuillotineBuilder builder)
		{
			_actionBarView = builder.ActionBarView;
			_listner = builder.GuillotineListener; 

			_guillotineView = builder.GuillotineView; 
			_duration = builder.Duration > 0 ? builder.Duration : DEFAULT_DURATION; 
			_delay = builder.StartDelay; 
			_isRightToLeftLayout = builder.IsRightToLeftLayout; 
			_interpolator = builder.Interpolator == null ? new GuillotineInterpolator() : builder.Interpolator; 
			setUpOpeningView(builder.OpeningView); 
			setUpClosingView(builder.ClosingView); 
			_openingAnimation = buildOpeningAnimation(); 
			_closingAnimation = buildClosingAnimation(); 
			if (builder.IsClosedOnStart) { 
				_guillotineView.Rotation = GUILLOTINE_CLOSED_ANGLE;
				_guillotineView.Visibility = ViewStates.Gone;
			}
		}

		public void Open ()
		{ 
			if (!_isOpening) { 
				_openingAnimation.Start (); 
			} 
		}


		public void Close ()
		{ 
			if (!_isClosing) { 
				_closingAnimation.Start (); 
			} 
		}

		private void setUpOpeningView (View openingView)
		{
			_openingView = openingView;
			if (_actionBarView != null) {
				_actionBarView.ViewTreeObserver.GlobalLayout += ActionBarView_ViewTreeObserver_GlobalLayout;
			}
			_openingView.Click += delegate {
				Open ();
			};
		}

		void ActionBarView_ViewTreeObserver_GlobalLayout (object sender, EventArgs e)
		{
			_actionBarView.ViewTreeObserver.GlobalLayout -= ActionBarView_ViewTreeObserver_GlobalLayout;
			_actionBarView.PivotX = calculatePivotX (_openingView);
			_actionBarView.PivotY = calculatePivotY (_openingView);
		}

		private void setUpClosingView (View closingView)
		{
			_closingView = closingView;
			_guillotineView.ViewTreeObserver.GlobalLayout += GuillotineView_ViewTreeObserver_GlobalLayout;

			_closingView.Click += delegate {
				Close ();
			};
		}

		void GuillotineView_ViewTreeObserver_GlobalLayout (object sender, EventArgs e)
		{
			_guillotineView.ViewTreeObserver.GlobalLayout -= GuillotineView_ViewTreeObserver_GlobalLayout;
			_guillotineView.PivotX = calculatePivotX (_closingView);
			_guillotineView.PivotY = calculatePivotY (_closingView);
		}

		private ObjectAnimator buildOpeningAnimation ()
		{
			ObjectAnimator rotationAnimator = initAnimator (ObjectAnimator.OfFloat (_guillotineView, ROTATION, GUILLOTINE_CLOSED_ANGLE, GUILLOTINE_OPENED_ANGLE)); 
			rotationAnimator.SetInterpolator (_interpolator); 
			rotationAnimator.SetDuration (_duration);
			rotationAnimator.AnimationStart+= delegate {
				_guillotineView.Visibility = ViewStates.Visible;
				_isOpening = true;
			};
			rotationAnimator.AnimationEnd+= delegate {
				_isOpening = false;
				if(_listner!=null)
					_listner.OnGuillotineOpened();
			};

			return rotationAnimator;
		}

		private ObjectAnimator buildClosingAnimation ()
		{
			ObjectAnimator rotationAnimator = initAnimator(ObjectAnimator.OfFloat(_guillotineView, ROTATION, GUILLOTINE_OPENED_ANGLE, GUILLOTINE_CLOSED_ANGLE)); 
			rotationAnimator.SetDuration((long) (_duration * GuillotineInterpolator.ROTATION_TIME)); 
			rotationAnimator.AnimationStart+= delegate {
				_isClosing = true;
				_guillotineView.Visibility = ViewStates.Visible;

			};
			rotationAnimator.AnimationEnd+= delegate {
				_isClosing = false;
				_guillotineView.Visibility = ViewStates.Gone;
				startActionBarAnimation();
				if(_listner!=null)
					_listner.OnGuillotineClosed();
			};

			return rotationAnimator;
		}

		private void startActionBarAnimation ()
		{ 
			ObjectAnimator actionBarAnimation = ObjectAnimator.OfFloat (_actionBarView, ROTATION, GUILLOTINE_OPENED_ANGLE, ACTION_BAR_ROTATION_ANGLE); 
			actionBarAnimation.SetDuration ((long)(_duration * (GuillotineInterpolator.FIRST_BOUNCE_TIME + GuillotineInterpolator.SECOND_BOUNCE_TIME))); 
			actionBarAnimation.SetInterpolator (new ActionBarInterpolator ()); 
			actionBarAnimation.Start (); 
		}


		private ObjectAnimator initAnimator (ObjectAnimator animator)
		{ 
			animator.StartDelay = _delay;
			return animator; 
		}


		private float calculatePivotY (View burger)
		{ 
			return burger.Top + burger.Height / 2; 
		}


		private float calculatePivotX (View burger)
		{ 
			return burger.Left + burger.Width / 2; 
		}
	}

	public class GuillotineBuilder
	{
		public  View GuillotineView;
		public  View OpeningView;
		public  View ClosingView;
		public View ActionBarView;
		public IGuillotineListener GuillotineListener;
		public long Duration;
		public long StartDelay;
		public bool IsRightToLeftLayout;
		public ITimeInterpolator Interpolator;
		public bool IsClosedOnStart;


		public GuillotineBuilder (View guillotineView, View closingView, View openingView)
		{ 
			GuillotineView = guillotineView; 
			OpeningView = openingView; 
			ClosingView = closingView; 
		}

		public GuillotineBuilder SetActionBarViewForAnimation (View view)
		{ 
			ActionBarView = view; 
			return this; 
		}

		public GuillotineBuilder SetGuillotineListener (IGuillotineListener guillotineListener)
		{ 
			GuillotineListener = guillotineListener; 
			return this; 
		}

		public GuillotineBuilder SetDuration (long duration)
		{ 
			Duration = duration; 
			return this; 
		}


		public GuillotineBuilder SetStartDelay (long startDelay)
		{ 
			StartDelay = startDelay; 
			return this; 
		}


		public GuillotineBuilder SetRightToLeftLayout (bool isRightToLeftLayout)
		{ 
			IsRightToLeftLayout = isRightToLeftLayout; 
			return this; 
		}


		public GuillotineBuilder SetInterpolator (ITimeInterpolator interpolator)
		{ 
			Interpolator = interpolator; 
			return this; 
		}


		public GuillotineBuilder SetClosedOnStart (bool isClosedOnStart)
		{ 
			IsClosedOnStart = isClosedOnStart; 
			return this; 
		}

		public GuillotineAnimation Build ()
		{ 
			return new GuillotineAnimation (this); 
		}
	}
	
}

