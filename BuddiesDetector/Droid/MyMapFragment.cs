
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BuddiesDetector.Droid
{
	public class MyMapFragment : MapFragment, IOnMapReadyCallback, View.IOnClickListener, ILocationListener
	{
		private GoogleMap _map;

		LocationManager localizationManager;


		public static MyMapFragment NewInstance()
		{
			return new MyMapFragment();
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			return base.OnCreateView(inflater, container, savedInstanceState);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);

			GetMapAsync(this);

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
		}

		public void OnProviderDisabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnProviderEnabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			throw new NotImplementedException();
		}
	}
}
