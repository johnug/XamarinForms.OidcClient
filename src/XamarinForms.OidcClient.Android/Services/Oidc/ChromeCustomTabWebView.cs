using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IdentityModel.OidcClient.WebView;
using System.Threading.Tasks;
using Android.Support.CustomTabs;
using Android.Graphics;
using Xamarin.Forms;

namespace XamarinForms.OidcClient.Droid.Services.Oidc
{
    public class ChromeCustomTabsWebView : IWebView
    {
        public event EventHandler<HiddenModeFailedEventArgs> HiddenModeFailed;

        private CustomTabsActivityManager _customTabs;

        public ChromeCustomTabsWebView()
        {
        }

        public Task<InvokeResult> InvokeAsync(InvokeOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.StartUrl))
            {
                throw new ArgumentException("Missing StartUrl", nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.EndUrl))
            {
                throw new ArgumentException("Missing EndUrl", nameof(options));
            }

            var _context = Forms.Context;

            // must be able to wait for the intent to be finished to continue
            // with setting the task result
            var _tcs = new TaskCompletionSource<InvokeResult>();

            // create & open chrome custom tab
            _customTabs = new CustomTabsActivityManager((Activity)_context);

            // build custom tab
            var builder = new CustomTabsIntent.Builder(_customTabs.Session)
               .SetToolbarColor(Android.Graphics.Color.Argb(255, 52, 152, 219))
               .SetShowTitle(true)
               .EnableUrlBarHiding();

            var customTabsIntent = builder.Build();

            // ensures the intent is not kept in the history stack, which makes
            // sure navigating away from it will close it
            customTabsIntent.Intent.AddFlags(ActivityFlags.NoHistory);

            MessagingCenter.Unsubscribe<Activity, string>(this, "OIDC.Login.Success");
            MessagingCenter.Subscribe<Activity, string>(this, "OIDC.Login.Success", (sender, dataString) => {
                //_context.StartActivity(typeof(MainActivity));
                Toast.MakeText(_context, "Login success. Please close the browser if it doesn't automatically close", ToastLength.Short).Show();

                _tcs.SetResult(new InvokeResult {
                    Response = dataString,
                    ResultType = InvokeResultType.Success
                });
            });

            // launch
            customTabsIntent.LaunchUrl((Activity)_context, Android.Net.Uri.Parse(options.StartUrl));
            // need an intent to be triggered when browsing to the "io.identityserver.native://callback"
            // scheme/URI => OidcCallbackInterceptorActivity
            return _tcs.Task;
        }
    }
}