using NotatnikKinomana.Models;
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

namespace NotatnikKinomana
{
    /// <summary>
    /// Logika interakcji dla klasy AddTitleWindow.xaml
    /// </summary>
    public partial class AddTitleWindow : Window
    {
        private MovieContext _context;
        public AddTitleWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string genre = GenreTextBox.Text;

            var existingMovie = _context.Movies.FirstOrDefault(m => m.Title == title);

            if (existingMovie != null)
            {
                MessageBox.Show("Taki tytuł już istnieje w bazie danych.");
                return; 
            }

            var movie = new Movie
            {
                Title = title,
                Genre = genre
            };
            MessageBox.Show("Dodano nowy tytuł: " + title);
            this.DialogResult = true;
        }
    }
}
