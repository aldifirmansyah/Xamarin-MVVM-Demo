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
            ViewModel = new ContactsViewModel(new PageService(), DependencyService.Get<ISQLiteDb>().GetConnection());

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        public ContactsViewModel ViewModel
        {
            get { return BindingContext as ContactsViewModel; }
            set { BindingContext = value; }
        }

        private void ContactsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectContactCommand.Execute(e.SelectedItem);
        }
    }
}