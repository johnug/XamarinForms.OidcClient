using XamarinForms.OidcClient.Services.Oidc;
using XamarinForms.OidcClient.Droid.Services.Oidc;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using IdentityModel.OidcClient;

[assembly: Dependency(typeof(OidcClientService))]
namespace XamarinForms.OidcClient.Droid.Services.Oidc
{
    public class OidcClientService : IOidcClientService
    {
        public IdentityModel.OidcClient.OidcClient PrepareClient(string authority, string clientId, string clientSecret, string scope, string returnUrl)
        {
            var options = new OidcClientOptions(
                authority, 
                clientId, 
                clientSecret, 
                scope, 
                returnUrl,
                new ChromeCustomTabsWebView(Forms.Context));
            return new IdentityModel.OidcClient.OidcClient(options);
        }
    }
}