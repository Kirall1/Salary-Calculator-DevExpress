using DevExpress.Mvvm;
using SqliteEncryptionApp.Data;
using SqliteEncryptionApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace SqliteEncryptionApp.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private string _newUserName;
        private User _selectedUser;
        private ObservableCollection<User> _users;
        private AppDbContext _dbContext;


        public string NewUserName
        {
            get => _newUserName;
            set
            {
                _newUserName = value;
                Set(ref _newUserName, value);
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                Set(ref _selectedUser, value);
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public MainViewModel()
        {
            try
            {
                _dbContext = new AppDbContext();
                DbInitializer.Initialize(_dbContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AddUserCommand = new DelegateCommand(() => AddUser(), () => !string.IsNullOrWhiteSpace(NewUserName));
            UpdateUserCommand = new DelegateCommand(() => UpdateUser(), () => SelectedUser != null);
            DeleteUserCommand = new DelegateCommand(() => DeleteUser(), () => SelectedUser != null);

            Users = new ObservableCollection<User>(_dbContext.Users.ToList());
        }


        private void AddUser()
        {
            var user = new User { Name = NewUserName };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            Users.Add(user);
            NewUserName = string.Empty;
        }

        private void UpdateUser()
        {
            var user = _dbContext.Users.Find(SelectedUser.Id);
            if (user != null)
            {
                user.Name = NewUserName;
                _dbContext.SaveChanges();
            }
        }

        private void DeleteUser()
        {
            var user = _dbContext.Users.Find(SelectedUser.Id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();

                Users.Remove(SelectedUser);
                SelectedUser = null;
            }
        }
    }
}
