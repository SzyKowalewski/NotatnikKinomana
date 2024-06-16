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
    /// Logika interakcji dla klasy RateMovieWindow.xaml
    /// </summary>
    public partial class RateMovieWindow : Window
    {
        private MovieContext _context;
        public RateMovieWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            int rating = (int)RatingSlider.Value;

            var movie = _context.Movies.FirstOrDefault(m => m.Title == title);
            if (movie != null)
            {
                movie.Rating = rating;
                _context.Movies.Update(movie);
                _context.SaveChanges();
                MessageBox.Show($"Dodano ocenę: {title} - {rating}");
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Nie znaleziono filmu o podanym tytule.");
            }
        }
    }
}
