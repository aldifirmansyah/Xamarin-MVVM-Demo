using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ViewModelDemo.ViewModels
{
    class PageService : IPageService
    {
        private Page _mainPage
        {
            get { return Application.Current.MainPage; }
        }

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await _mainPage.DisplayAlert(title, message, ok, cancel);
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await _mainPage.DisplayAlert(title, message, cancel);
        }

        public async Task PushAsync(Page page)
        {
            await _mainPage.Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await _mainPage.Navigation.PopAsync();
        }
    }
}
