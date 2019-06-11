using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ViewModelDemo.ViewModels
{
    public interface IPageService
    {
        Task PushAsync(Page page);
        Task PopAsync();
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
    }
}
