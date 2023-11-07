using Microsoft.AspNetCore.Components;

namespace BlazorAuthDemo.Views.Components
{
    public partial class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            string returnUri = NavigationManager
                .ToBaseRelativePath(NavigationManager.Uri);

            string redirectUri =
                string.IsNullOrWhiteSpace(returnUri) ?
                    string.Empty : $"?redirectUri=/{returnUri}";

            NavigationManager.NavigateTo(
                uri: $"MicrosoftIdentity/Account/SignIn{redirectUri}",
                forceLoad: true);
        }
    }
}