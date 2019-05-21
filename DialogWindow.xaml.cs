using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Project2
{

    public partial class DialogWindow : Window
    {

        public Book ObjectBook;

        public DialogWindow()
        {
            InitializeComponent();
            for (int i = 2019; i >= 1900; i--)
            {
                ComboYear.Items.Add(i);
            }
            
        }
  private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ObjectBook = new Book();

            if (AuthorData.Text == String.Empty || TitleData.Text == String.Empty || ComboYear.SelectedIndex == -1)
            {
                MessageBox.Show("At least one of the parameters has not been added or has been added incorrectly", "Error");
            }
            else
            {
                if (true)
                {
                    ObjectBook.Author = AuthorData.Text;
                    ObjectBook.Title = TitleData.Text;
                    ObjectBook.Year = int.Parse(ComboYear.Text);

                    DialogResult = true;
                    this.Close();
                }
            }
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
