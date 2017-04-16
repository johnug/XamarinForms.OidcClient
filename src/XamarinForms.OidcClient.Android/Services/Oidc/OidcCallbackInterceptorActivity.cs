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
using Xamarin.Forms;

namespace XamarinForms.OidcClient.Droid.Services.Oidc
{
    [Activity(Label = "OIDC Callback interceptor")]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "io.identityserver.native",
        DataHost = "callback")] 
    public class OidcCallbackInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Finish();

            MessagingCenter.Send<Activity, string>(this, "OIDC.Login.Success", Intent.DataString);
        }

    }
}