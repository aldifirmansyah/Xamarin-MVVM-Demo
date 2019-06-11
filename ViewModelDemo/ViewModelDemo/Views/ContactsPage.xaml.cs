using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelDemo.Models;
using ViewModelDemo.Persistence;
using ViewModelDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViewModelDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            BindingContext = new ContactsViewModel(new PageService(), DependencyService.Get<ISQLiteDb>().GetConnection());

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }        

        async void OnAddContact(object sender, System.EventArgs e)
        {
            await (BindingContext as ContactsViewModel).AddContact();
        }

        async void OnDeleteContact(object sender, System.EventArgs e)
        {
            await (BindingContext as ContactsViewModel).DeleteContact((sender as MenuItem).CommandParameter as ContactViewModel);
        }

        async void OnContactSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            await (BindingContext as ContactsViewModel).SelectContact(e.SelectedItem as ContactViewModel);
        }
    }
}