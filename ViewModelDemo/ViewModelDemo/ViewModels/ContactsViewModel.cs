using SQLite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewModelDemo.Models;
using ViewModelDemo.Persistence;
using Xamarin.Forms;

namespace ViewModelDemo.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        public ObservableCollection<ContactViewModel> Contacts { get; private set; } = new ObservableCollection<ContactViewModel>();
        
        private SQLiteAsyncConnection _connection;
        private readonly IPageService _pageService;

        public ContactsViewModel(IPageService pageService, SQLiteAsyncConnection connection)
        {
            _pageService = pageService;
            _connection = connection;

            LoadData();
        }

        public async Task AddContact()
        {
            var contactDetailViewModel = new ContactDetailViewModel (new ContactViewModel(), _pageService, _connection);
            contactDetailViewModel.ContactAdded += (source, contact) =>
            {
                Contacts.Add(new ContactViewModel(contact));
            };

            await _pageService.PushAsync(new ContactDetailPage(contactDetailViewModel));
        }

        public async Task SelectContact(ContactViewModel selectedContact)
        {
            if (selectedContact == null)
                return;
            
            SelectedContact = null;

            var contactDetailViewModel = new ContactDetailViewModel(selectedContact, _pageService, _connection);
            contactDetailViewModel.ContactUpdated += (source, contact) =>
            {
                selectedContact.Id = contact.Id;
                selectedContact.FirstName = contact.FirstName;
                selectedContact.LastName = contact.LastName;
                selectedContact.Phone = contact.Phone;
                selectedContact.Email = contact.Email;
                selectedContact.IsBlocked = contact.IsBlocked;
            };
            await _pageService.PushAsync(new ContactDetailPage(contactDetailViewModel));
        }

        public async Task DeleteContact(ContactViewModel contactViewModel)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure want to delete {contactViewModel.FullName}?", "Yes", "No"))
            {
                Contacts.Remove(contactViewModel);

                await _connection.DeleteAsync(
                    new Contact
                    {
                        Id = contactViewModel.Id
                    });
            }
        }

        private async Task LoadData()
        {
            await _connection.CreateTableAsync<Contact>();

            var contacts = await _connection.Table<Contact>().ToListAsync();

            foreach (Contact c in contacts)
                Contacts.Add(new ContactViewModel(c));
        }

        private ContactViewModel _selectedContact;
        public ContactViewModel SelectedContact
        {
            get { return _selectedContact; }
            set { SetValue(ref _selectedContact, value); }
        }
    }
}
