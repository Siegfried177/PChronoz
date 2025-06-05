using PChronoz.Models;
using PChronoz.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PChronoz.ViewModels
{
    class ViewModelControl : INotifyPropertyChanged
    {
        public string DuelistPath { get; set; }
        public string ImagesPath { get; set; }
        public string ResourcesPath { get; set; }
        public string EDOProPath { get; set; }

        public ObservableCollection<string> Sets { get; set; }
        public ObservableCollection<string> SetCards { get; set; }

        private string _selectedset;
        public string SelectedSet
        {
            get => _selectedset;
            set 
            { 
                _selectedset = value;
                LoadSetCards();
                OnPropertyChanged(nameof(SelectedSet));
                OnPropertyChanged(nameof(SetCards));
            }
        }

        private string _craftselectedcard;
        public string CraftSelectedCard
        {
            get => _craftselectedcard;
            set
            {
                _craftselectedcard = value;
                CraftSelectedImage = $@"{ImagesPath}\Cards\{CraftSelectedCard}.png";
                OnPropertyChanged(nameof(CraftSelectedCard));
                OnPropertyChanged(nameof(CraftSelectedImage));
            }
        }

        private string _craftselectedimage;
        public string CraftSelectedImage
        {
            get => _craftselectedimage;
            set
            {
                _craftselectedimage = value;
                OnPropertyChanged(nameof(CraftSelectedImage));
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

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

        private Card _selectedcard;
        public Card SelectedCard
        {
            get { return _selectedcard; }
            set
            {
                if (_selectedcard != value)
                {
                    _selectedcard = value;
                    string aux;

                    if (SelectedCard.Attribute != "Spell" && SelectedCard.Attribute != "Trap")
                    {
                        SelectedCardText1 = $"Monster {SelectedCard.Type2} {SelectedCard.Type3} {SelectedCard.Attribute} {SelectedCard.Type1}";
                        aux = SelectedCard.Type3 == "Xyz" ? "Rango" : SelectedCard.Type3 == "Link" ? "Link" : "Level";
                        SelectedCardText2 = $"{aux} {SelectedCard.Level} ATK {SelectedCard.Atk} DEF {SelectedCard.Def}";
                    }
                    else
                    {
                        aux = SelectedCard.Type2 == "-" ? "" : SelectedCard.Type2;
                        SelectedCardText1 = $"{SelectedCard.Attribute} {aux}";
                        SelectedCardText2 = "";
                    }
                    SelectedCardDescription = "blablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablabla";
                    CardImage = $@"{ImagesPath}\Cards\{SelectedCard.Id}.jpg";
                    OnPropertyChanged(nameof(SelectedCard));
                    OnPropertyChanged(nameof(SelectedCardText1));
                    OnPropertyChanged(nameof(SelectedCardText2));
                    OnPropertyChanged(nameof(SelectedCardDescription));
                    OnPropertyChanged(nameof(CardImage));
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
                    LoadCards(UserCards);
                    ProfilePhoto = $@"{ImagesPath}\Users\{SelectedUser}.png";
                    LoadStats();
                    OnPropertyChanged(nameof(SelectedUser));
                    OnPropertyChanged(nameof(UserCards));
                    OnPropertyChanged(nameof(SelectedUserDP));
                    OnPropertyChanged(nameof(SelectedUserWins));
                    OnPropertyChanged(nameof(SelectedUserLoses));
                    OnPropertyChanged(nameof(SelectedUserC));
                    OnPropertyChanged(nameof(SelectedUserR));
                    OnPropertyChanged(nameof(SelectedUserSR));
                    OnPropertyChanged(nameof(SelectedUserUR));
                }
            }
        }

        public string SelectedUserDP { get; set; }
        public string SelectedUserWins { get; set; }
        public string SelectedUserLoses { get; set; }
        public string SelectedUserC { get; set; }
        public string SelectedUserR { get; set; }
        public string SelectedUserSR { get; set; }
        public string SelectedUserUR { get; set; }

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

        public ObservableCollection<string> Users { get; set; }
        
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

        private ObservableCollection<String> _userdecks;
        public ObservableCollection<String> UserDecks
        {
            get { return _userdecks; }
            set
            {
                _userdecks = value;
                OnPropertyChanged(nameof(UserDecks));
            }
        }

        private ObservableCollection<Card> _deckCards;
        public ObservableCollection<Card> DeckCards
        {
            get { return _deckCards; }
            set
            {
                _deckCards = value;
                OnPropertyChanged(nameof(DeckCards));
            }
        }

        private ObservableCollection<Card> _extraDeckCards;
        public ObservableCollection<Card> ExtraDeckCards
        {
            get { return _extraDeckCards; }
            set
            {
                _extraDeckCards = value;
                OnPropertyChanged(nameof(ExtraDeckCards));
            }
        }

        public ObservableCollection<string> TypeList { get; set; }
        public ObservableCollection<string> SpellsTypeList { get; set; }
        public ObservableCollection<string> TrapsTypeList { get; set; }
        public ObservableCollection<string> CategoriesList { get; set; }
        public ObservableCollection<string> SubCategoriesList { get; set; }
        public ObservableCollection<string> AttributeList { get; set; }

        public string LogoImage {  get; set; }
        public string HomeImage { get; set; }
        public string ShopImage { get; set; }
        public string ExchangeImage { get; set; }
        public string CraftingImage { get; set; }
        public string EDOPROImage { get; set; }

        public string SelectedCardText1 { get; set; }
        public string SelectedCardText2 { get; set; }
        public string SelectedCardDescription { get; set; }

        public string SelectedType { get; set; }
        public string SelectedAttribute { get; set; }
        public string SelectedSummonType { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedSubCategory { get; set; }
        public string ATKText { get; set; }
        public string DEFText { get; set; }

        private string _duelistexchange1;
        public string DuelistExchange1
        {
            get { return _duelistexchange1; }
            set
            {
                if (_duelistexchange2 == value && value != null) DeselectDuelistExchange(1);
                _duelistexchange1 = value;
                OnPropertyChanged(nameof(DuelistExchange1));
            }
        }

        private string _duelistexchange2;
        public string DuelistExchange2
        {
            get { return _duelistexchange2; }
            set
            {
                if (_duelistexchange1 == value && value != null) DeselectDuelistExchange(0);
                _duelistexchange2 = value;
                OnPropertyChanged(nameof(DuelistExchange2));
            }
        }

        private string _duelistexchangeindex1;
        public string DuelistExchangeIndex1
        {
            get { return _duelistexchangeindex1; }
            set
            {
                _duelistexchangeindex1 = value;
                OnPropertyChanged(nameof(DuelistExchangeIndex1));
            }
        }

        private string _duelistexchangeindex2;
        public string DuelistExchangeIndex2
        {
            get { return _duelistexchangeindex2; }
            set
            {
                _duelistexchangeindex2 = value;
                OnPropertyChanged(nameof(DuelistExchangeIndex2));
            }
        }

        private string _exchangeimage1;
        public string ExchangeImage1
        {
            get { return _exchangeimage1; }
            set
            {
                _exchangeimage1 = value;
                OnPropertyChanged(nameof(ExchangeImage1));
            }
        }

        private string _exchangeimage2;
        public string ExchangeImage2
        {
            get { return _exchangeimage2; }
            set
            {
                _exchangeimage2 = value;
                OnPropertyChanged(nameof(ExchangeImage2));
            }
        }
        
        private Card _exchangecard1;
        public Card ExchangeCard1
        {
            get { return _exchangecard1; }
            set
            {
                if (_exchangecard1 != value)
                {
                    _exchangecard1 = value;
                    OnPropertyChanged(nameof(ExchangeCard1));
                }
            }
        }

        private Card _exchangecard2;
        public Card ExchangeCard2
        {
            get { return _exchangecard2; }
            set
            {
                if (_exchangecard2 != value)
                {
                    _exchangecard2 = value;
                    OnPropertyChanged(nameof(ExchangeCard2));
                }
            }
        }

        // OPEN EDOPRO VARIABLES
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        // COMMANDS
        public ICommand ChangeViewCommand { get; }
        public ICommand FilterSpellsCommand { get; }
        public ICommand FilterTrapsCommand { get; }
        public ICommand FilterOffCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExchangeCardsCommand { get; }
        public ICommand ExchangeCommand { get; }
        public ICommand CraftCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand GuestInCommand { get; }
        public ICommand CardClickedCommand { get; }
        public ICommand OpenEDOProCommand { get; }

        // VISIBILITY
        public bool exchange = false;
        public string SettingsBarVisibility => ActualView is LoginView || ActualView is StartView? "Collapsed" : "Visible";
        public string MainViewSettingsBarVisibility => ActualView is MainView? "Collapsed" : "Visible";
        public string DuelistsVisibility => ActualView is UserDataView || ActualView is UserCardsView? "Visible" : "Collapsed";
        public string ShopVisibility => ActualView is ShopView ? "Collapsed" : "Visible";
        public string ExchangeCardVisibility => exchange ? "Visible" : "Collapsed";

        public ViewModelControl()
        {
            ActualView = new LoginView();
            ChangeViewCommand = new RelayCommand(ChangeView);
            FilterOffCommand = new RelayCommand(FilterOff);
            SearchCommand = new RelayCommand(SearchFilter);
            ExchangeCardsCommand = new RelayCommand(SelectExchange);
            ExchangeCommand = new RelayCommand(Exchange);
            LoginCommand = new RelayCommand(Login);
            GuestInCommand = new RelayCommand(GuestIn);
            CardClickedCommand = new RelayCommand(OnCardClicked);
            OpenEDOProCommand = new RelayCommand(OpenEDOPro);

            TypeList = new ObservableCollection<string>
            {
                "Warrior", "Spellcaster", "Dragon", "Zombie", "Machine", "Fairy", "Fiend", "Beast",
                "Beast-Warrior", "Dinosaur", "Fish", "Sea Serpent", "Reptile", "Psychic", "Wyrm",
                "Cyberse", "Thunder", "Pyro", "Rock", "Plant", "Aqua", "Insect", "Dinosaur", "Divine-Beast"
            };
            CategoriesList = new ObservableCollection<string> { "Monster", "Spell", "Trap"};
            SubCategoriesList = new ObservableCollection<string> { };
            AttributeList = new ObservableCollection<string> { "Light", "Dark", "Fire", "Water", "Earth", "Wind", "Divine" };
            SpellsTypeList = new ObservableCollection<string> { "Normal", "Continuous", "Quick-Play", "Field", "Ritual"};
            TrapsTypeList = new ObservableCollection<string> { "Normal", "Continuous", "Counter"};

            string[] keys_path = File.ReadAllLines(@"C:\ProjectChronoz\key.txt");
            DuelistPath = keys_path[0];
            ImagesPath = keys_path[1];
            ResourcesPath = keys_path[2];
            EDOProPath = keys_path[3];
            LogoImage = $@"{ImagesPath}\Logo.png";
            HomeImage = $@"{ImagesPath}\Icons\home.png";
            ShopImage = $@"{ImagesPath}\Icons\store.png";
            ExchangeImage = $@"{ImagesPath}\Icons\exchange.png";
            CraftingImage = $@"{ImagesPath}\Icons\hammer.png";
            EDOPROImage = $@"{ImagesPath}\Icons\duel disk.png";

            Users = new ObservableCollection<string> { "Siegfried", "Walko", "Paka" };
            CardImage = $@"{ImagesPath}\Icons\normal.png";

            UserCards = new ObservableCollection<Card>();
            DeckCards = new ObservableCollection<Card>();
            ExtraDeckCards = new ObservableCollection<Card>();

            Sets = new ObservableCollection<string> { "Alien Invasion", "Core Update"};
            CraftSelectedImage = $@"{ImagesPath}\Icons\Back.jpg";
        }

        private void ChangeView(object parameter)
        {
            if (parameter.ToString() == "Main")
                ActualView = new MainView();
            else if (parameter.ToString() == "UserData")
                ActualView = new UserDataView();
            else if (parameter.ToString() == "Cards")
            {
                ActualView = new UserCardsView();
                TakeFiltersOff();
                ResetFilters("");
            }
            else if (parameter.ToString() == "Shop")
                ActualView = new ShopView();
            else if (parameter.ToString() == "BuyPacks")
                ActualView = new BuyPacksShopView();
            else if (parameter.ToString() == "Exchange")
            {
                exchange = true;
                ActualView = new ExchangeView();
            }
            else if (parameter.ToString() == "Duelist1")
            {
                if (DuelistExchange1 != null)
                {
                    SelectedUser = DuelistExchange1;
                    ActualView = new UserCardsView();
                }
            }
            else if (parameter.ToString() == "Duelist2")
            {
                if (DuelistExchange2 != null)
                {
                    SelectedUser = DuelistExchange2;
                    ActualView = new UserCardsView();
                }
            }
            else if (parameter.ToString() == "Crafting")
                ActualView = new CraftingView();
            else if (Users.Contains(parameter.ToString()))
            {
                SelectedUser = parameter.ToString();
                ActualView = new UserDataView();
            }
            OnPropertyChanged(nameof(SettingsBarVisibility));
            OnPropertyChanged(nameof(DuelistsVisibility));
            OnPropertyChanged(nameof(MainViewSettingsBarVisibility));
            OnPropertyChanged(nameof(ShopVisibility));
        }

        private void GuestIn(object parameter)
        {
            ActualView = new StartView();
            OnPropertyChanged(nameof(SettingsBarVisibility));
        }

        public string GetPassword()
        {
            string storedpassword = null;
            return storedpassword;
        }

        private void Login(object parameter)
        {
            string storedHash = GetPassword();
            string inputHash = Password;

            if (inputHash == storedHash)
            {
                MessageBox.Show("Hash: " + HashPassword(storedHash));
                ActualView = new MainView();
            }
            else
            {
                MessageBox.Show(inputHash);
                MessageBox.Show("Contraseña Incorrecta");
            }
        }

        private string HashPassword(string password)
        {
            if (password != null)
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha.ComputeHash(bytes);
                    return Convert.ToBase64String(hash);
                }
            else return null;
        }

        public void LoadStats()
        {
            string path = $@"{DuelistPath}\{SelectedUser} -- DB.txt";
            if (!File.Exists(path)) throw new FileNotFoundException("El archivo no existe.");
            var data = File.ReadAllLines(path);

            SelectedUserDP = data[0];
            SelectedUserWins = data[1];
            SelectedUserLoses = data[2];
            SelectedUserC = data[3];
            SelectedUserR = data[4];
            SelectedUserSR = data[5];
            SelectedUserUR = data[6];
        }

        public void LoadSetCards()
        {
            string path = $@"{ResourcesPath}\Sets\{SelectedSet}.txt";
            if (!File.Exists(path))
                throw new FileNotFoundException("El archivo no existe.");

            SetCards = new ObservableCollection<string>();
            var cartas = File.ReadAllLines(path);

            foreach (var carta in cartas) SetCards.Add(carta);
        }

        private void ResetFilters(string parameter)
        {
            if (parameter != "Category") SelectedCategory = null;
            if (parameter != "SubCategory") SelectedSubCategory = null;
            if (parameter != "Type1") SelectedType = null;
            if (parameter != "Attribute") SelectedAttribute = null;
            if (parameter != "Type3") SelectedSummonType = null;
            if (parameter != "Atk") ATKText = "";
            if (parameter != "Def") DEFText = "";
        }

        public void FilterCards(string parameter, string input)
        {
            ResetFilters(parameter);
            UserCards = new ObservableCollection<Card>();
            LoadCards(UserCards);

            var filteredItems = UserCards.Where(item =>
            {
                var property = item.GetType().GetProperty(parameter);

                if (input == "Monster") // Monster
                {
                    property = item.GetType().GetProperty("Attribute");

                    if (property != null)
                    {
                        var value = property.GetValue(item)?.ToString();
                        if (value == "Spell" || value == "Trap") return false;
                        return true;
                    }
                }

                if (input == "Spell" || input == "Trap") // Spell or Trap
                    property = item.GetType().GetProperty("Attribute");

                if (property != null)
                {
                    var value = property.GetValue(item)?.ToString();
                    if (parameter == "Attribute") return value == input.ToUpper(); // Attribute
                    else if (parameter == "Atk" || parameter == "Def") return value == input && value != "-1" && value != "0"; // ATK or DEF
                    else if (parameter == "Type1") return value == input; // Type
                    else if (parameter == "Name") return value.Contains(input); // Name
                }
                return false;
            }).ToList();

            UserCards.Clear();
            foreach (var item in filteredItems) UserCards.Add(item);
        }

        private void FilterOff(object parameter)
        {
            TakeFiltersOff();
        }

        public void TakeFiltersOff()
        {
            UserCards = new ObservableCollection<Card>();
            SearchBarText = null;
            LoadCards(UserCards);
        }

        private void SearchFilter(object parameter)
        {   
            if (SearchBarText != null && SearchBarText != "")
                FilterCards("Name", (char.ToUpper(SearchBarText[0]) + SearchBarText.Substring(1)));
            if (SelectedCategory != null) FilterCards("Category", SelectedCategory);
            if (SelectedSubCategory != null) FilterCards("SubCategory", SelectedSubCategory);
            if (SelectedAttribute != null) FilterCards("Attribute", SelectedAttribute);
            if (SelectedType != null) FilterCards("Type1", SelectedType);
            if (ATKText != null && ATKText != "") FilterCards("Atk", ATKText);
            if (DEFText != null && DEFText != "") FilterCards("Def", DEFText);
        }

        public void LoadCards(ObservableCollection<Card> Cards)
        {
            string path = $@"{DuelistPath}\{SelectedUser}.txt";
            if (!File.Exists(path))
                throw new FileNotFoundException("El archivo no existe.");

            Cards = new ObservableCollection<Card>();
            var lineas = File.ReadAllLines(path);
            foreach (var linea in lineas)
            {
                var carddata = linea.Split('|');
                Card Carta = new Card
                {
                    Id = carddata[0],
                    Name = carddata[1],
                    Attribute = carddata[2],
                    Level = carddata[3],
                    Type1 = carddata[4],
                    Type2 = carddata[5],
                    Type3 = carddata[6],
                    Type4 = carddata[7],
                    Atk = carddata[8],
                    Def = carddata[9],
                    Quantity = carddata[10],
                    Rarity = "Ultra Rare"
                };
                UserCards.Add(Carta);
            }
        }

        private void SelectExchange(object parameter)
        {
            ActualView = new ExchangeView();
            OnPropertyChanged(nameof(DuelistsVisibility));
            if (SelectedUser == DuelistExchange1)
            {
                ExchangeCard1 = (Card)SelectedCard.Clone();
                ExchangeImage1 = $"{ImagesPath}\\Cards\\{SelectedCard.Id}.jpg";
            }
            else if (SelectedUser == DuelistExchange2)
            {
                ExchangeCard2 = (Card)SelectedCard.Clone();
                ExchangeImage2 = $"{ImagesPath}\\Cards\\{SelectedCard.Id}.jpg";
            }
        }

        private void Exchange(object parameter)
        {
            DoTheExchange(DuelistExchange1, DuelistExchange2, ExchangeCard1, ExchangeCard2);
            DoTheExchange(DuelistExchange2, DuelistExchange1, ExchangeCard2, ExchangeCard1);
            DuelistExchange1 = null; DuelistExchange2 = null;
            ExchangeCard1 = null; ExchangeCard2 = null;
            ExchangeImage1 = null; ExchangeImage2 = null;
        }

        public void DoTheExchange(string duelist, string duelist2, Card cardout, Card cardin)
        {
            string path = $@"{DuelistPath}\{duelist}.txt";
            var lineas = File.ReadAllLines(path).ToList();
            exchange = true;
            bool aux = false;
            for (int i = lineas.Count - 1; i >= 0; i--)
            {
                var elementos = lineas[i].Split('|');
                if (elementos[1] == cardin.Name)
                {
                    aux = true;
                    if (int.TryParse(elementos[10], out int cantidad))
                    {
                        cantidad += 1;
                        elementos[10] = cantidad.ToString();
                        lineas[i] = string.Join("|", elementos);
                    }
                }
                if (elementos[1] == cardout.Name)
                {
                    if (int.TryParse(elementos[10], out int cantidad))
                    {
                        cantidad -= 1;
                        if (cantidad > 0)
                        {
                            elementos[10] = cantidad.ToString();
                            lineas[i] = string.Join("|", elementos); 
                        }
                        else lineas.RemoveAt(i);
                    }
                }
            }

            File.WriteAllLines(path, lineas);
            if (! aux)
                File.AppendAllText(path, $"{cardin.Id}|{cardin.Name}|{cardin.Attribute}|{cardin.Level}|{cardin.Type1}|{cardin.Type2}|{cardin.Type3}|{cardin.Type4}|{cardin.Atk}|{cardin.Def}|1|");
        }

        public void DeselectDuelistExchange(int aux)
        {
            if (aux == 0)
            {
                DuelistExchange1 = null;
                DuelistExchangeIndex1 = "-1";
                ExchangeCard1 = null;
                ExchangeImage1 = null;
            }
            else
            {
                DuelistExchange2 = null;
                DuelistExchangeIndex2 = "-1";
                ExchangeCard2 = null;
                ExchangeImage1 = null;
            }
        }

        private void OnCardClicked(object parameter)
        {
            if (parameter is Card selectedCard)
            {
                SelectedCard = selectedCard;
            }
        }

        private void OpenEDOPro(object parameter)
        {
            string processName = "EDOPro";
            string path = $@"{EDOProPath}\EDOPro.exe";

            Process[] process = Process.GetProcessesByName(processName);

            if (process.Length > 0)
            {
                Process proceso = process[0];
                IntPtr handle = proceso.MainWindowHandle;

                if (handle != IntPtr.Zero)
                {
                    ShowWindow(handle, SW_RESTORE);
                    SetForegroundWindow(handle);
                }
            }
            else
            {
                try
                {
                    Process.Start(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("No se pudo abrir EDOPro");
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
