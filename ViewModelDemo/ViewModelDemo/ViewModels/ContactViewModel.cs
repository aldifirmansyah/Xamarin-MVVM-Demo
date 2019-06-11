using ViewModelDemo.Models;
using Xamarin.Forms;

namespace ViewModelDemo.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        public int Id { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public Color TextColor
        {
            get { return IsBlocked ? Color.Red : Color.Green; }
        }

        public ContactViewModel() {}

        public ContactViewModel (Contact c)
        {
            Id = c.Id;
            _firstName = c.FirstName;
            _lastName = c.LastName;
            Phone = c.Phone;
            Email = c.Email;
            IsBlocked = c.IsBlocked;
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetValue(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetValue(ref _lastName, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        private bool _isBlocked;
        public bool IsBlocked
        {
            get { return _isBlocked; }
            set
            {
                SetValue(ref _isBlocked, value);
                OnPropertyChanged(nameof(TextColor));
            }
        }

    }
}
