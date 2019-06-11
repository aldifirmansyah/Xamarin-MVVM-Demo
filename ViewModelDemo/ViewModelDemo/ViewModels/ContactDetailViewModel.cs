using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModelDemo.Models;
using ViewModelDemo.Persistence;
using Xamarin.Forms;

namespace ViewModelDemo.ViewModels
{
    public class ContactDetailViewModel : BaseViewModel
    {
        public Contact Contact { get; private set; }
        public event EventHandler<Contact> ContactAdded;
        public event EventHandler<Contact> ContactUpdated;

        private SQLiteAsyncConnection _connection;
        private IPageService _pageService;

        public ContactDetailViewModel(ContactViewModel contact, IPageService pageService, SQLiteAsyncConnection connection)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            _pageService = pageService;
            _connection = connection;

            Contact = new Contact
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Phone = contact.Phone,
                Email = contact.Email,
                IsBlocked = contact.IsBlocked
            };
        }

        public async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Contact.FirstName) || string.IsNullOrWhiteSpace(Contact.LastName))
            {
                await _pageService.DisplayAlert("Error", "Please enter the name.", "OK");
                return;
            }

            if (Contact.Id == 0)
            {
                await _connection.InsertAsync(Contact);

                ContactAdded?.Invoke(this, Contact);
            }
            else
            {
                await _connection.UpdateAsync(Contact);

                ContactUpdated?.Invoke(this, Contact);
            }

            await _pageService.PopAsync();
        }
    }
}
