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
    /// Logika interakcji dla klasy AddReviewWindow.xaml
    /// </summary>
    public partial class AddReviewWindow : Window
    {
        private MovieContext _context;
        public AddReviewWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            LoadMovies();
        }

        private void LoadMovies()
        {
            var movies = _context.Movies.ToList();
            TitleComboBox.ItemsSource = movies;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleComboBox.SelectedItem is Movie selectedMovie)
            {
                string review = ReviewTextBox.Text;
                selectedMovie.Review = review;
                MessageBox.Show($"Dodano recenzję do filmu: {selectedMovie.Title}");
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Nie wybrano filmu.");
            }
        }
    }
}
