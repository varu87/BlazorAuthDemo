using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorAuthDemo.Views.Shared
{
    public partial class MainLayout
    {
        [Inject]
        protected AuthenticationStateProvider Provider { get; set; }

        private string username;

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState authenticationState =
                await Provider.GetAuthenticationStateAsync();

            ClaimsPrincipal user = authenticationState.User;
            username = user.Identity.Name;
        }
    }
}