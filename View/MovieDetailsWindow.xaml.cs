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

namespace NotatnikKinomana.View
{
    /// <summary>
    /// Logika interakcji dla klasy MovieDetailsWindow.xaml
    /// </summary>
    public partial class MovieDetailsWindow : Window
    {
        private Movie _selectedMovie;

        public MovieDetailsWindow(Movie selectedMovie)
        {
            InitializeComponent();

            _selectedMovie = selectedMovie;
            TitleTextBlock.Text = _selectedMovie.Title;
            GenreTextBlock.Text = _selectedMovie.Genre;
            PremiereDateTextBlock.Text = _selectedMovie.PremiereDate?.ToString("dd/MM/yyyy");
            ReviewTextBlock.Text = _selectedMovie.Review;
            RatingTextBlock.Text = _selectedMovie.Rating.ToString();
            RuntimeTextBlock.Text = $"{_selectedMovie.Runtime} min";
            DescriptionTextBlock.Text = _selectedMovie.Description;
            IsInScheduleTextBlock.Text = _selectedMovie.IsInSchedule ? "Tak" : "Nie";
            IsWatchedTextBlock.Text = _selectedMovie.IsWatched ? "Tak" : "Nie";
            DirectorNameTextBlock.Text = _selectedMovie.Director != null
                ? $"{_selectedMovie.Director.FirstName} {_selectedMovie.Director.LastName}"
                : "Nieznany";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
