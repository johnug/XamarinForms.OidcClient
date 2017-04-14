namespace XamarinForms.OidcClient.Services.Oidc
{
    public interface IOidcClientService
    {
        IdentityModel.OidcClient.OidcClient PrepareClient(string authority, string clientId, string clientSecret, string scope, string returnUrl);
    }
}
