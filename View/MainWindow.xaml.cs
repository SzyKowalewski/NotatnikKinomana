using Microsoft.EntityFrameworkCore;
using NotatnikKinomana.Models;
using NotatnikKinomana.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NotatnikKinomana
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();

        public ObservableCollection<Movie> Schedule = new ObservableCollection<Movie>();

        private MovieContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            _context.Database.EnsureCreated();
            _context.EnsureSchemaUpdated();
            MoviesList.ItemsSource = Movies;
            LoadMovies();
        }
        private void LoadMovies()
        {
            Movies.Clear();
            var movies = _context.Movies.Include(m => m.Director).ToList();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }



        private void AddTitle_Click(object sender, RoutedEventArgs e)
        {
            AddTitleWindow addTitleWindow = new AddTitleWindow();
            if (addTitleWindow.ShowDialog() == true)
            {
                if (int.TryParse(addTitleWindow.LengthTextBox.Text, out int runtime))
                {
                    Director director = _context.Directors.FirstOrDefault(d => d.FirstName == addTitleWindow.DirectorFirstNameTextBox.Text &&
                                                                        d.LastName == addTitleWindow.DirectorLastNameTextBox.Text);
                    if (director == null)
                    {
                        director = new Director
                        {
                            FirstName = addTitleWindow.DirectorFirstNameTextBox.Text,
                            LastName = addTitleWindow.DirectorLastNameTextBox.Text
                        };
                        _context.Directors.Add(director);
                        _context.SaveChanges();
                        LoadMovies();
                    }

                    var movie = new Movie
                    {
                        Title = addTitleWindow.TitleTextBox.Text,
                        Genre = addTitleWindow.GenreTextBox.Text,
                        Description = addTitleWindow.DescriptionTextBox.Text,
                        Runtime = runtime,
                        DirectorId = director.Id
                    };

                    _context.Movies.Add(movie);
                    _context.SaveChanges();
                    LoadMovies();
                }
            }
        }

        private void AddPremiereDate_Click(object sender, RoutedEventArgs e)
        {
            AddPremiereDateWindow addPremiereDateWindow = new AddPremiereDateWindow();
            if (addPremiereDateWindow.ShowDialog() == true)
            {
                Movie selectedMovie = addPremiereDateWindow.MovieComboBox.SelectedItem as Movie;

                if (selectedMovie != null)
                {
                    var movie = _context.Movies.FirstOrDefault(m => m.Id == selectedMovie.Id);

                    if (movie != null)
                    {
                        movie.PremiereDate = addPremiereDateWindow.PremiereDatePicker.SelectedDate;
                        _context.SaveChanges();
                        MoviesList.Items.Refresh();
                        LoadMovies();
                    }
                }
                else
                {
                    MessageBox.Show("Nie wybrano filmu.");
                }
            }
        }

        private void ViewSchedule_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.ShowDialog();
        }

        private void AddToSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesList.SelectedItem is Movie selectedMovie)
            {
                if (selectedMovie.IsInSchedule || selectedMovie.IsWatched)
                {
                    MessageBox.Show("Ten film już znajduje się na liście do obejrzenia, lub został już obejrzany.", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                selectedMovie.IsInSchedule = true;
                _context.Movies.Update(selectedMovie);
                _context.SaveChanges();
                MessageBox.Show("Film został dodany do harmonogramu.");
            }
            else
            {
                MessageBox.Show("Proszę wybrać film do dodania do harmonogramu.");
            }
        }

        private void ViewWatched_Click(object sender, RoutedEventArgs e)
        {
            WatchedWindow watchedMoviesWindow = new WatchedWindow();
            watchedMoviesWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AddReview_Click(object sender, RoutedEventArgs e)
        {
            AddReviewWindow addReviewWindow = new AddReviewWindow();
            if (addReviewWindow.ShowDialog() == true)
            {
                var movie = Movies.FirstOrDefault(m => m.Title == addReviewWindow.TitleComboBox.Text);
                if (movie != null)
                {
                    movie.Review = addReviewWindow.ReviewTextBox.Text;
                    _context.Movies.Update(movie);
                    _context.SaveChanges();
                    MoviesList.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono filmu o podanym tytule.");
                }
            }
        }

        private void RateMovie_Click(object sender, RoutedEventArgs e)
        {
            RateMovieWindow rateMovieWindow = new RateMovieWindow();
            if (rateMovieWindow.ShowDialog() == true)
            {
                var movie = Movies.FirstOrDefault(m => m.Title == rateMovieWindow.TitleComboBox.Text);
                if (movie != null)
                {
                    movie.Rating = (int)rateMovieWindow.RatingSlider.Value;
                    _context.Movies.Update(movie);
                    _context.SaveChanges();
                    MoviesList.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono filmu o podanym tytule.");
                }
            }
        }

        private void RemoveTitle_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesList.SelectedItem is Movie selectedMovie)
            {
                _context.Movies.Remove(selectedMovie);
                _context.SaveChanges();
                LoadMovies();
            }
            else
            {
                MessageBox.Show("Nie wybrano filmu do usunięcia.");
            }
        }

        private void MoviesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MoviesList.SelectedItem is Movie selectedMovie)
            {
                _context.Entry(selectedMovie).Reload();
                MovieDetailsWindow detailsWindow = new MovieDetailsWindow(selectedMovie);
                detailsWindow.DataContext = selectedMovie;
                detailsWindow.ShowDialog();
            }
        }

        private Button _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void SortByColumn_Click(object sender, RoutedEventArgs e)
        {
            var header = sender as Button;
            string sortBy = header?.Tag.ToString();
            if (sortBy == null) return;

            ListSortDirection direction = ListSortDirection.Ascending;
            if (_lastHeaderClicked == header && _lastDirection == ListSortDirection.Ascending)
            {
                direction = ListSortDirection.Descending;
            }

            _lastHeaderClicked = header;
            _lastDirection = direction;

            ICollectionView dataView = CollectionViewSource.GetDefaultView(Movies);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void FilterMovies(string filter)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Movies);
            view.Filter = item =>
            {
                Movie movie = item as Movie;
                if (movie == null) return false;

                bool matchesTitle = !string.IsNullOrEmpty(movie.Title) && movie.Title.ToLower().Contains(filter.ToLower());
                bool matchesDirectorFirstName = movie.Director != null && !string.IsNullOrEmpty(movie.Director.FirstName) && movie.Director.FirstName.ToLower().Contains(filter.ToLower());
                bool matchesDirectorLastName = movie.Director != null && !string.IsNullOrEmpty(movie.Director.LastName) && movie.Director.LastName.ToLower().Contains(filter.ToLower());
                bool matchesUnwatched = filter.ToLower() == "nieobejrzane" && (movie.IsWatched) == false;

                return matchesTitle || matchesDirectorFirstName || matchesDirectorLastName || matchesUnwatched;
            };
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (FilterTextBox != null)
            {
                FilterMovies(FilterTextBox.Text);
            }
        }

        private void ShowHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }

    }

}
