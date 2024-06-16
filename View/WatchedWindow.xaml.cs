using Microsoft.EntityFrameworkCore;
using NotatnikKinomana.Models;
using System;
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
using System.Windows.Shapes;

namespace NotatnikKinomana.View
{
    /// <summary>
    /// Logika interakcji dla klasy WatchedWindow.xaml
    /// </summary>
    public partial class WatchedWindow : Window
    {
        public ObservableCollection<Watched> Watched { get; set; }
        private MovieContext _context;

        public WatchedWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            LoadWatchedMovies();
        }

        private void LoadWatchedMovies()
        {
            Watched = new ObservableCollection<Watched>(_context.Watched.Include(wm => wm.Movie).ToList());
            WatchedMoviesList.ItemsSource = Watched;
        }

        private void CalculateTotalWatchTime_Click(object sender, RoutedEventArgs e)
        {
            int totalMinutes = Watched.Sum(wm => wm.Movie.Runtime);
            TimeSpan totalTime = TimeSpan.FromMinutes(totalMinutes);
            MessageBox.Show($"Łączny czas spędzony na oglądaniu filmów: {totalTime.Hours} godzin {totalTime.Minutes} minut.");
        }

        private void RemoveFromWatched_Click(object sender, RoutedEventArgs e)
        {
            if (WatchedMoviesList.SelectedItem is Watched selectedWatchedMovie)
            {
                _context.Watched.Remove(selectedWatchedMovie);
                _context.SaveChanges();
                Watched.Remove(selectedWatchedMovie);
            }
            else
            {
                MessageBox.Show("Proszę wybrać film do usunięcia z obejrzanych.");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
