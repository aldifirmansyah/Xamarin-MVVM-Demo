using SQLite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelDemo.Models;
using ViewModelDemo.Persistence;
using Xamarin.Forms;

namespace ViewModelDemo.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        private SQLiteAsyncConnection _connection;
        private readonly IPageService _pageService;
        private ContactViewModel _selectedContact;
        private bool _isDataLoaded;

        public ContactViewModel SelectedContact
        {
            get { return _selectedContact; }
            set { SetValue(ref _selectedContact, value); }
        }
        public ObservableCollection<ContactViewModel> Contacts { get; private set; } =
            new ObservableCollection<ContactViewModel>();
        
        public ICommand AddContactCommand { get; private set; }
        public ICommand LoadDataCommand { get; private set; }
        public ICommand SelectContactCommand { get; private set; }
        public ICommand DeleteContactCommand { get; private set; }

        public ContactsViewModel(IPageService pageService, SQLiteAsyncConnection connection)
        {
            _pageService = pageService;
            _connection = connection;

            AddContactCommand = new Command(async() => await AddContact());
            LoadDataCommand = new Command (async() => await LoadData());
            SelectContactCommand = new Command<ContactViewModel>(async c => await SelectContact(c));
            DeleteContactCommand = new Command<ContactViewModel>(async c => await DeleteContact(c));

        }

        private async Task AddContact()
        {
            var contactDetailViewModel = new ContactDetailViewModel (new ContactViewModel(), _pageService, _connection);
            contactDetailViewModel.ContactAdded += (source, contact) =>
            {
                Contacts.Add(new ContactViewModel(contact));
            };

            await _pageService.PushAsync(new ContactDetailPage(contactDetailViewModel));
        }

        private async Task SelectContact(ContactViewModel selectedContact)
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

        private async Task DeleteContact(ContactViewModel contactViewModel)
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
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;

            await _connection.CreateTableAsync<Contact>();

            var contacts = await _connection.Table<Contact>().ToListAsync();

            foreach (Contact c in contacts)
                Contacts.Add(new ContactViewModel(c));
        }
    }
}
