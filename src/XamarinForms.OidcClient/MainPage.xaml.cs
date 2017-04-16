using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinForms.OidcClient.Services.Oidc;

namespace XamarinForms.OidcClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.OidcClientService = DependencyService.Get<IOidcClientService>();

            BtnLogin.Clicked += async (sender, e) => await BtnLogin_Clicked(sender, e);
        }

        private IOidcClientService OidcClientService { get; set; }

        private IdentityModel.OidcClient.OidcClient oidcClient { get; set; }
        protected IdentityModel.OidcClient.OidcClient OidcClient => oidcClient ?? (oidcClient = OidcClientService.PrepareClient(
            authority: "http://demo.identityserver.io/",
            clientId: "native.hybrid",
            clientSecret: "some-ridiculously-longwinded-and-secure-secret",
            redirectUrl: "io.identityserver.native://callback",
            scope: "openid profile"));

        private async Task BtnLogin_Clicked(object sender, EventArgs e)
        {
            var client = OidcClient;

            var loginResult = await client.LoginAsync();

            if(loginResult.Success)
            {
                var claimsString = string.Join("\n", loginResult.Claims.Select(c => $"{c.Type}: {c.Value}"));
                EditorResult.Text = claimsString;
            }
        }
    }
}
