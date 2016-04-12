using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Com.Lilarcor.Cheeseknife;

namespace Guillotine.Menu
{
	[Activity (Theme="@style/AppTheme", Label = "Guillotine.Menu", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{
		const long RIPPLE_DURATION = 250;

		[InjectView(Resource.Id.toolbar)]  Toolbar _toolbar;
		[InjectView(Resource.Id.root)] FrameLayout _root;
		[InjectView(Resource.Id.content_hamburger)] View _contentHamburger;

		protected override void OnCreate (Bundle bundle)
		{
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate (bundle);
           
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			Cheeseknife.Inject(this);

			if (_toolbar != null) {
				SetSupportActionBar (_toolbar);
                SupportActionBar.SetDisplayShowTitleEnabled(false);
				//SupportActionBar.SetTitle (Resource.String.app_name);
			}

			View guillotineMenu = LayoutInflater.From(this).Inflate(Resource.Layout.Guillotine, null);
			_root.AddView(guillotineMenu);

			new GuillotineBuilder(guillotineMenu, guillotineMenu.FindViewById(Resource.Id.guillotine_hamburger), _contentHamburger)
				.SetStartDelay(RIPPLE_DURATION)
				.SetActionBarViewForAnimation(_toolbar)
				.Build();
		}
	}
}


