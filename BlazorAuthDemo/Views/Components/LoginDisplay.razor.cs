using BlazorAuthDemo.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthDemo.Views.Components
{
    public partial class LoginDisplay
    {
        [CascadingParameter]
        public User User { get; set; }

        protected override void OnInitialized()
        {
            var user = User;
        }
    }
}