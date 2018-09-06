using Android.App;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;

namespace SimpleMapDemo
{
    [Activity(Label = "@string/activity_label_mylocation")]
    public class MyLocationActivity : AppCompatActivity, IOnMapReadyCallback
    {
        static readonly string TAG = "MyLocationActivity";

        static readonly int REQUEST_PERMISSIONS_LOCATION = 1000;


        MapFragment mapFrag;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MapLayout);

            this.AddMapFragmentToLayout(Resource.Id.map_container);
        }


        void InitializeMapFragment()
        {
            var options = new GoogleMapOptions();
            options.InvokeMapType(GoogleMap.MapTypeHybrid)
                   .InvokeCompassEnabled(true);

            mapFrag = MapFragment.NewInstance(options);

            FragmentManager.BeginTransaction()
                           .Add(Resource.Id.map_container, mapFrag, "map_frag")
                           .Commit();

            mapFrag.GetMapAsync(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSIONS_LOCATION) 
            {
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted) ) {
                    // Permissions granted, nothing to do.
                }
                else {
                    // Permissions not granted!
                    Log.Info(TAG, "The app does not have location permissions");

                    var layout = FindViewById(Android.Resource.Id.Content);
                    Snackbar.Make(layout, Resource.String.location_permission_missing, Snackbar.LengthShort).Show();
                    Finish();
                }
            }
            else 
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            this.PerformRuntimePermissionCheckForLocation(REQUEST_PERMISSIONS_LOCATION);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.UiSettings.MyLocationButtonEnabled = true;
        }
    }
}
