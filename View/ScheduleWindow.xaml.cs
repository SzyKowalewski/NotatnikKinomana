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
            Schedule = new ObservableCollection<NotatnikKinomana.Models.Movie>(_context.Movies.Where(m => m.IsInSchedule == true).ToList());
            ScheduleList.ItemsSource = Schedule;
        }

        private void RemoveFromSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleList.SelectedItem is Movie selectedMovie)
            {
                selectedMovie.IsInSchedule = false;
                Schedule.Remove(selectedMovie);
                _context.Movies.Update(selectedMovie);
                _context.SaveChanges();
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

