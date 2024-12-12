namespace SqliteEncryptionApp.Models
{
    public class User : ViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
    }
}
