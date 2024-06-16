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
    /// Logika interakcji dla klasy AddPremiereDateWindow.xaml
    /// </summary>
    public partial class AddPremiereDateWindow : Window
    {
        private MovieContext _context;
        public AddPremiereDateWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            LoadMovies();
        }

        private void LoadMovies()
        {
            var movies = _context.Movies.ToList();
            MovieComboBox.ItemsSource = movies;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Movie selectedMovie = MovieComboBox.SelectedItem as Movie;

            if (selectedMovie != null)
            {
                DateTime? premiereDate = PremiereDatePicker.SelectedDate;

                if (premiereDate != null)
                {
                    MessageBox.Show($"Dodano datę premiery dla filmu {selectedMovie.Title}: {premiereDate.Value.ToShortDateString()}");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Proszę wybrać datę premiery.");
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać film.");
            }
        }
    }
}
