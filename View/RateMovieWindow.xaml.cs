using NotatnikKinomana.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class RateMovieWindow : Window, INotifyPropertyChanged
    {
        private MovieContext _context;
        private int _currentRating;
        public int CurrentRating
        {
            get { return _currentRating; }
            set
            {
                _currentRating = value;
                OnPropertyChanged(nameof(CurrentRating));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RateMovieWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            LoadMovies();
            DataContext = this;
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
                int rating = (int)RatingSlider.Value;
                selectedMovie.Rating = rating;
                MessageBox.Show($"Dodano ocenę: {selectedMovie.Title} : {rating}");
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Nie wybrano filmu.");
            }
        }

        
    }
}
