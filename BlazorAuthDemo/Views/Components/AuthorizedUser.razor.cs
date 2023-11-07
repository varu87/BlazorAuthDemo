using BlazorAuthDemo.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BlazorAuthDemo.Views.Components
{
    public partial class AuthorizedUser
    {
        [Inject]
        public ILogger<AuthorizedUser> Logger { get; set; }

        [CascadingParameter]
        public User User { get; set; }

        [Parameter]
        public string AuthorizedGroups { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Verbose { get; set; } = true;

        public AuthorizedUserComponentState State { get; set; }

        protected override void OnInitialized()
        {
            try
            {
                this.State = AuthorizedUserComponentState.Loading;

                bool isAuthorized = User.Groups.Any(group => 
                                        AuthorizedGroups.Split(',').ToList()
                                        .Select(group => group.Trim())
                                        .Contains(group));

                this.State = isAuthorized ?
                                AuthorizedUserComponentState.Content :
                                AuthorizedUserComponentState.Unauthorized;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                this.State = AuthorizedUserComponentState.Error;
            }
        }
    }

    public enum AuthorizedUserComponentState
    {
        Loading,
        Content,
        Unauthorized,
        Error
    }
}