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

namespace NotatnikKinomana
{
    /// <summary>
    /// Logika interakcji dla klasy ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ObservableCollection<Movie> Schedule { get; set; }
        private MovieContext _context;

        public ScheduleWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            Schedule = new ObservableCollection<NotatnikKinomana.Models.Movie>(_context.Movies.Where(m => m.PremiereDate != null).ToList());
            ScheduleList.ItemsSource = Schedule;
        }

        private void AddToSchedule_Click(object sender, RoutedEventArgs e)
        {
            Movie selectedMovie = ScheduleList.SelectedItem as Movie;
            if (selectedMovie != null)
            {
                if (!Schedule.Contains(selectedMovie))
                {
                    Schedule.Add(selectedMovie);
                    MessageBox.Show($"Dodano film '{selectedMovie.Title}' do harmonogramu.");
                }
                else
                {
                    MessageBox.Show("Ten film jest już w harmonogramie.");
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać film do dodania do harmonogramu.");
            }
        }

        private void RemoveFromSchedule_Click(object sender, RoutedEventArgs e)
        {
            Movie selectedMovie = ScheduleList.SelectedItem as Movie;
            if (selectedMovie != null)
            {
                if (Schedule.Contains(selectedMovie))
                {
                    Schedule.Remove(selectedMovie);
                    MessageBox.Show($"Usunięto film '{selectedMovie.Title}' z harmonogramu.");
                }
                else
                {
                    MessageBox.Show("Ten film nie znajduje się w harmonogramie.");
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać film do usunięcia z harmonogramu.");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

