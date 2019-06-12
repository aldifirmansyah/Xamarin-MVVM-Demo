using SQLite;
using System;
using ViewModelDemo.Persistence;
using ViewModelDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViewModelDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactDetailPage : ContentPage
	{
		public ContactDetailPage (ContactDetailViewModel contactDetailViewModel)
        {
            BindingContext = contactDetailViewModel;

            InitializeComponent ();
		}
	}
}