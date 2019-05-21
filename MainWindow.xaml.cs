using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Project2
{
    
    public class Book
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
    }
    
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

    private string PathToFile = @"C:\Users\user\Desktop\Politechnika\EGUI\Project2\Project2\database.csv";
    private bool filterOn = false;

        private ObservableCollection<Book> _DGItemsSource = new ObservableCollection<Book>();
        public ObservableCollection<Book> DGItemsSource
        {
            get
            {
                Notify(nameof(_DGItemsSource));
                return _DGItemsSource;
            }
            private set {}
        }

        private ObservableCollection<int> _YearCombo = new ObservableCollection<int>();
        public ObservableCollection<int> YearCombo
        {
            get
            {
                Notify(nameof(_YearCombo));
                return _YearCombo;
  
            }
             set {   
                _YearCombo = value;
                Notify(nameof(YearCombo));
                 }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void Notify(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CSVLoad(PathToFile);
            year.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Descending));
            DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
        }


        private void CSVLoad(string path) 
        {
            using (var ReadStream = new StreamReader(path))
            {
                while (!ReadStream.EndOfStream)
                {
                    var Split = ReadStream.ReadLine().Split(';');
                    var NewBook = new Book()
                    {
                        ID = DGItemsSource.Count,
                        Author = Split[0],
                        Title = Split[1],
                        Year = int.Parse(Split[2])
                    };
                    DGItemsSource.Add(NewBook);
                    YearCombo.Add(int.Parse(Split[2]));
                }
                ReadStream.Close();
            }
            YearCombo = new ObservableCollection<int>(YearCombo.Distinct());
        }

        private void CSVSave(string path)
        {
            using (var WriteStream = new StreamWriter(path))
            {
                for (int i = 0; i < DGItemsSource.Count; i++)
                {
                    WriteStream.WriteLine("{0};{1};{2}", DGItemsSource[i].Author, DGItemsSource[i].Title, DGItemsSource[i].Year);
                }
            }
        }

        public void AddBook(string Author, string Title, int Year)
        {
            DGItemsSource.Add(new Book { ID = 0, Author = Author, Title = Title, Year = Year });
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow AddWindow = new DialogWindow();

            bool? result = AddWindow.ShowDialog();

            if (result.HasValue && result.Value == true)
            {
                Book NewBook = AddWindow.ObjectBook;

                AddBook(NewBook.Author, NewBook.Title, NewBook.Year);
                YearCombo.Add(NewBook.Year);

                if (filterOn == true)
                {
                    var Tempyear = year.SelectedValue;
                    CSVSave(PathToFile);
                    DGItemsSource.Clear();
                    YearCombo.Clear();
                    CSVLoad(PathToFile);
                    year.SelectedValue = Tempyear;
                    FilterTheBooks();
                    DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
                }
                else
                {
                    CSVSave(PathToFile);
                    DGItemsSource.Clear();
                    YearCombo.Clear();
                    CSVLoad(PathToFile);
                    FilterTheBooks();
                    DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
                }
            }
        }
        
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                DialogWindow EditWindow = new DialogWindow();

                Book EditedBook = (Book)DataGrid.SelectedItem;

                EditWindow.Accept.Content = "Save";
                EditWindow.Title = "Edit selected book";

                EditWindow.AuthorData.Text = EditedBook.Author;
                EditWindow.TitleData.Text = EditedBook.Title;
                EditWindow.ComboYear.Text = EditedBook.Year.ToString();

                bool? result = EditWindow.ShowDialog();
                if (result.HasValue && result.Value == true)
                {
                    for (int i = 0; i < DGItemsSource.Count; i++)
                    {
                        if (DGItemsSource[i].ID == EditedBook.ID)
                        {
                            DGItemsSource[i] = EditWindow.ObjectBook;
                        }
                    }
                    if (filterOn == true)
                    {
                        var Tempyear = year.SelectedValue;
                        CSVSave(PathToFile);
                        DGItemsSource.Clear();
                        YearCombo.Clear();
                        CSVLoad(PathToFile);
                        year.SelectedValue = Tempyear;
                        FilterTheBooks();
                        DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
                    }
                    else
                    {
                        CSVSave(PathToFile);
                        DGItemsSource.Clear();
                        YearCombo.Clear();
                        CSVLoad(PathToFile);
                        FilterTheBooks();
                        DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
                    }
                }
            }
        }
       
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int numOfItems = DataGrid.SelectedItems.Count;

            for (int i=0; i < numOfItems; i++)
            {
                Book book = DataGrid.SelectedItems[i] as Book;
                DGItemsSource.Remove(book);
            }

                if(filterOn == true)
             {
                var Tempyear = year.SelectedValue;
                CSVSave(PathToFile);
                DGItemsSource.Clear();
                YearCombo.Clear();
                CSVLoad(PathToFile);
                year.SelectedValue = Tempyear;
                FilterTheBooks();
                DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
            }
                else
            {
                CSVSave(PathToFile);
                DGItemsSource.Clear();
                YearCombo.Clear();
                CSVLoad(PathToFile);
                FilterTheBooks();
                DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            author.Text = String.Empty;
            title.Text = String.Empty;
            year.SelectedIndex = -1;
            filterOn = false;
            DataGrid.ItemsSource = DGItemsSource;
            DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            FilterTheBooks();
            DataGrid.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
        }

        private void FilterTheBooks()
        {
            ObservableCollection<Book> FilteredTable = new ObservableCollection<Book>();
            FilteredTable.Clear();
            foreach (Book ObjectBook in DGItemsSource)
            {
                Book TempBook = ObjectBook;
                if (year.SelectedIndex == -1)
                {
                    if (TempBook.Author.Contains(author.Text) && TempBook.Title.Contains(title.Text))
                    {
                        FilteredTable.Add(ObjectBook);
                    }
                }
                else
                {
                    if (TempBook.Author.Contains(author.Text) && TempBook.Title.Contains(title.Text) && TempBook.Year.Equals(year.SelectedValue))
                    {
                        FilteredTable.Add(ObjectBook);
                    }
                }
            }
            DataGrid.ItemsSource = FilteredTable;
            filterOn = true;
        }

        private void Author_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Escape == e.Key)
            {
                this.Close();
            }
            if (e.Key >= Key.D0 && e.Key <= Key.D9 ||

                e.Key >= Key.A && e.Key <= Key.Z ||
                e.Key == Key.Space)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.Key == Key.Tab)
            {
                e.Handled = false;
            }
        }

        private void Title_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Escape == e.Key)
            {
                this.Close();
            }
            if (e.Key >= Key.D0 && e.Key <= Key.D9 ||
                e.Key >= Key.A && e.Key <= Key.Z ||
                e.Key == Key.Space)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.Key == Key.Tab)
            {
                e.Handled = false;
            }
        }
    }
}