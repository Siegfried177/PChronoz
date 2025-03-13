using PChronoz.Models;
using PChronoz.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PChronoz.ViewModels
{
    class ViewModelControl : INotifyPropertyChanged
    {
        private string _cardimage;
        public string CardImage
        {
            get { return _cardimage; }
            set
            {
                _cardimage = value;
            }
        }

        public string _searchbartext;
        public string SearchBarText
        {
            get { return _searchbartext; }
            set
            {
                _searchbartext = value;
                OnPropertyChanged(nameof(SearchBarText));
            }
        }

        private string _selectedcard_level;
        public string SelectedCardLevel
        {
            get { return _selectedcard_level; }
            set
            {
                string method = SelectedCard.Type3 == "Xyz" ? "Rango" : SelectedCard.Type3 == "Link" ? "Link": "Nivel";
                _selectedcard_level = $"{method}:";
            }
        }

        private Card _selectedcard;
        public Card SelectedCard
        {
            get { return _selectedcard; }
            set
            {
                if (_selectedcard != value)
                {
                    _selectedcard = value;
                    string method = SelectedCard.Type3 == "Xyz" ? "Rango" : SelectedCard.Type3 == "Link" ? "Link": "Nivel";
                    SelectedCardLevel = $"{method}:";
                    CardImage = $@"C:\Users\yamiy\OneDrive\Escritorio\Proyectos\4 -- C#\PChronoz\PChronoz\Images\Cards\{SelectedCard.Id}.png";
                    OnPropertyChanged(nameof(SelectedCard));
                    OnPropertyChanged(nameof(CardImage));
                    OnPropertyChanged(nameof(SelectedCardLevel));
                }
            }
        }

        private string _selecteduser;
        public string SelectedUser
        {
            get { return _selecteduser; }
            set
            {
                if (_selecteduser != value)
                {
                    _selecteduser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                    LoadCards();

                    if (_selecteduser == "Siegfried")
                    {
                        ProfilePhoto = UsersImage[0];
                        SelectedUserWins = "Victorias 17";
                        SelectedUserLoses = "Derrotas 07";
                    }
                    else if (_selecteduser == "Paka")
                    {
                        ProfilePhoto = UsersImage[2];
                        SelectedUserWins = "Victorias 0";
                        SelectedUserLoses = "Derrotas 69";
                    }
                    else if (_selecteduser == "Walko")
                    {
                        ProfilePhoto = UsersImage[1];
                        SelectedUserWins = "Victorias ?";
                        SelectedUserLoses = "Derrotas ?";
                    }

                    OnPropertyChanged(nameof(UserCards));
                    OnPropertyChanged(nameof(ProfilePhoto));
                    OnPropertyChanged(nameof(SelectedUserWins));
                }
            }
        }

        private string _selecteduserwins;
        public string SelectedUserWins
        {
            get { return _selecteduserwins; }
            set
            {
                if (_selecteduserwins != value)
                {
                    _selecteduserwins = value;
                    OnPropertyChanged(nameof(SelectedUserWins));
                }
            }
        }

        private string _selecteduserloses;
        public string SelectedUserLoses
        {
            get { return _selecteduserloses; }
            set
            {
                if (_selecteduserloses != value)
                {
                    _selecteduserloses = value;
                    OnPropertyChanged(nameof(SelectedUserLoses));
                }
            }
        }

        private string _profilephoto;
        public string ProfilePhoto
        {
            get { return _profilephoto; }
            private set
            {
                _profilephoto = value;
                OnPropertyChanged(nameof(ProfilePhoto));
            }
        }

        private object _actualView;
        public object ActualView
        {
            get { return _actualView; }
            set
            {
                if (_actualView != value)
                {
                    _actualView = value;
                    OnPropertyChanged(nameof(ActualView));
                }
            }
        }

        private ObservableCollection<string> _users;
        public ObservableCollection<string> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private ObservableCollection<string> _usersimage;
        public ObservableCollection<string> UsersImage
        {
            get { return _usersimage; }
            set
            {
                _usersimage = value;
                OnPropertyChanged(nameof(UsersImage));
            }
        }
        
        private ObservableCollection<Card> _usercards;
        public ObservableCollection<Card> UserCards
        {
            get { return _usercards; }
            set
            {
                _usercards = value;
                OnPropertyChanged(nameof(UserCards));
            }
        }

        public ICommand ChangeViewCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand FilterOffCommand { get; }
        public ICommand SearchCommand { get; }

        public string MainViewSettingsBarVisibility => ActualView is MainView ? "Collapsed" : "Visible";
        public string ShopVisibility => ActualView is ShopView ? "Collapsed" : "Visible";

        public ViewModelControl()
        {
            ActualView = new MainView();
            ChangeViewCommand = new RelayCommand(ChangeView);
            FilterCommand = new RelayCommand(OpenWindow);
            FilterOffCommand = new RelayCommand(FilterOff);
            SearchCommand = new RelayCommand(SearchFilter);

            Users = new ObservableCollection<string> { "Siegfried", "Walko", "Paka" };
            UsersImage = new ObservableCollection<string> { "C:\\Users\\yamiy\\OneDrive\\Escritorio\\Proyectos\\4 -- C#\\PChronoz\\PChronoz\\Images\\Avatars\\Siegfried.png", "C:\\Users\\yamiy\\OneDrive\\Escritorio\\Proyectos\\4 -- C#\\PChronoz\\PChronoz\\Images\\Avatars\\Walko.png", "D:\\Descargas\\Paka.webp" };
            CardImage = "C:\\Users\\yamiy\\OneDrive\\Escritorio\\Proyectos\\4 -- C#\\PChronoz\\PChronoz\\Images\\Icons\\normal.png";

            UserCards = new ObservableCollection<Card>();
        }

        private void ChangeView(object parameter)
        {
            if (parameter.ToString() == "Main")
                ActualView = new MainView();
            else if (parameter.ToString() == "UserData")
                ActualView = new UserDataView();
            else if (parameter.ToString() == "Cards")
                ActualView = new UserCardsView();
            else if (parameter.ToString() == "Shop")
                ActualView = new ShopView();
            else if (Users.Contains(parameter.ToString()))
            {
                SelectedUser = parameter.ToString();
                ActualView = new UserDataView();
            }

            OnPropertyChanged(nameof(MainViewSettingsBarVisibility));
            OnPropertyChanged(nameof(ShopVisibility));
        }

        private void OpenWindow(object parameter)
        {
            var inputWindow = new FilterWindow();
            if (inputWindow.ShowDialog() == true)
            {
                string input = inputWindow.InputText;
                input = input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
                if (input != null) FilterCards(parameter.ToString(), input);
            }
        }

        public void FilterCards(string parameter, string input)
        {
            UserCards = new ObservableCollection<Card>();
            LoadCards();
            var filteredItems = UserCards.Where(item =>
            {
                var property = item.GetType().GetProperty(parameter);
                if (property != null)
                {
                    var value = property.GetValue(item)?.ToString();
                    if (parameter != "Name")
                        return value != null && value == input;
                    else
                        return value != null && value.Contains(input);
                }
                return false;
            }).ToList();

            UserCards.Clear();
            foreach (var item in filteredItems)
                UserCards.Add(item);
        }

        private void FilterOff(object parameter)
        {
            UserCards = new ObservableCollection<Card>();
            LoadCards();
        }

        private void SearchFilter(object parameter)
        {
            FilterCards("Name", SearchBarText);
            SearchBarText = null;
        }

        public void LoadCards()
        {
            string path = $"C:\\Users\\yamiy\\OneDrive\\Escritorio\\Proyectos\\4 -- C#\\PChronoz\\PChronoz\\{SelectedUser}.txt";
            if (!File.Exists(path))
                throw new FileNotFoundException("El archivo no existe.");

            UserCards = new ObservableCollection<Card>();
            var lineas = File.ReadAllLines(path);
            foreach (var linea in lineas)
            {
                var carddata = linea.Split('|');
                Card Carta = new Card
                {
                    Id = carddata[0],
                    Arquetype = carddata[1],
                    Name = carddata[2],
                    Atribute = carddata[3],
                    Level = carddata[4],
                    Type1 = carddata[5],
                    Type2 = carddata[6],
                    Type3 = carddata[7],
                    Type4 = carddata[8],
                    Atk = carddata[9],
                    Def = carddata[10]
                };
                UserCards.Add(Carta);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
