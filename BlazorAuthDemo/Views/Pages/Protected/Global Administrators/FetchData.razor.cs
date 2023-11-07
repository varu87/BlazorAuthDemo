using BlazorAuthDemo.Data;
using System;
using System.Threading.Tasks;

namespace BlazorAuthDemo.Views.Pages.Protected.Global_Administrators
{
    public partial class FetchData
    {
        private WeatherForecast[] forecasts;
        protected override async Task OnInitializedAsync()
        {
            forecasts = await ForecastService.GetForecastAsync(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}