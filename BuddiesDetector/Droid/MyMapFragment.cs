
using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Transitions;
using Android.Util;
using System.Drawing;

namespace BuddiesDetector.Droid
{
	public class MyMapFragment : Fragment, IOnMapReadyCallback, View.IOnClickListener, ILocationListener
	{
		private GoogleMap _map;
		private MapFragment _mapFragment;
		private RadarView _radarView;
		private int radarRadius = 200;

		LocationManager localizationManager;

		public static MyMapFragment NewInstance()
		{
			return new MyMapFragment();
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			return inflater.Inflate(Resource.Layout.MainFragment, container, false);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			_radarView = new RadarView(Activity);
			_radarView.Visibility = ViewStates.Gone;

			RelativeLayout layout = (Android.Widget.RelativeLayout)Activity.FindViewById(Resource.Id.mainLayout);
			layout.AddView(_radarView);

			_radarView.LayoutParameters.Height = PixelsToDp(radarRadius);
			_radarView.LayoutParameters.Width = PixelsToDp(radarRadius);

			_mapFragment = new MapFragment();
			FragmentTransaction transaction = Activity.FragmentManager.BeginTransaction();
			transaction.Add(Resource.Id.map, _mapFragment).Commit();

			_mapFragment.GetMapAsync(this);
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			_map = googleMap;
		}

		public void OnClick(View v)
		{

			localizationManager = (LocationManager)Activity.GetSystemService(Activity.LocationService);

			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = localizationManager.GetProviders(criteriaForLocationService, true);

			String locationProvider;

			if (acceptableLocationProviders.Any())
			{
				locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				locationProvider = string.Empty;
			}

			localizationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
		}

		public void OnLocationChanged(Location location)
		{
			MarkerOptions markerOpt1 = new MarkerOptions();
			markerOpt1.SetPosition(new LatLng(location.Latitude, location.Longitude));
			markerOpt1.SetTitle("Vimy Ridge");
			_map.AddMarker(markerOpt1);

			LatLng latLng = new LatLng(location.Latitude, location.Longitude);
			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(latLng);
			builder.Zoom(18);
			//builder.Bearing(155);
			//builder.Tilt(65);
			CameraPosition cameraPosition = builder.Build();
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

			_map.MoveCamera(cameraUpdate);

			Projection projection = _map.Projection;
			var screenPosition = projection.ToScreenLocation(markerOpt1.Position);

			_radarView.Visibility = ViewStates.Visible;
			_radarView.SetX(screenPosition.X - radarRadius);
			_radarView.SetY(screenPosition.Y - radarRadius);

			_radarView.startAnimation();
		}

		public void OnProviderDisabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnProviderEnabled(string provider)
		{
			
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			throw new NotImplementedException();
		}

		private int PixelsToDp(int pixels)
		{
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
		}
	}
}
