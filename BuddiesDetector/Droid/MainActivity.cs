using Android.App;
using Android.Widget;
using Android.OS;

namespace BuddiesDetector.Droid
{
	[Activity(Label = "BuddiesDetector", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var myMapFragment = MyMapFragment.NewInstance();
			FragmentTransaction tx = FragmentManager.BeginTransaction();
			tx.Add(Resource.Id.fragment_container, myMapFragment);
			tx.Commit();

			ImageButton localizationButton = (Android.Widget.ImageButton)FindViewById(Resource.Id.localizationButton);

			localizationButton.SetOnClickListener(myMapFragment);
		}
	}
}

