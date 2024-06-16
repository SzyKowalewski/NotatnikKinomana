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

namespace NotatnikKinomana
{
    /// <summary>
    /// Logika interakcji dla klasy ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ObservableCollection<Movie> Schedule { get; set; }
        private MovieContext _context;
        private Point _dragStartPoint;

        public ScheduleWindow()
        {
            InitializeComponent();
            _context = new MovieContext();
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            Schedule = new ObservableCollection<Movie>(_context.Movies.Where(m => m.IsInSchedule).OrderBy(m => m.OrderIndex).ToList());
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

        private void ScheduleList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void ScheduleList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Initialize the drag & drop operation
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem != null)
                {
                    Movie movie = (Movie)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    DataObject dragData = new DataObject("myFormat", movie);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        private void ScheduleList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Movie droppedMovie = e.Data.GetData("myFormat") as Movie;
                ListView listView = sender as ListView;
                Movie targetMovie = ((FrameworkElement)e.OriginalSource).DataContext as Movie;

                if (droppedMovie != null && listView != null)
                {
                    int removedIdx = Schedule.IndexOf(droppedMovie);
                    int targetIdx = Schedule.IndexOf(targetMovie);

                    if (removedIdx >= 0)
                    {
                        if (targetIdx < 0)
                        {
                            targetIdx = Schedule.Count - 1;
                        }

                        if (removedIdx != targetIdx)
                        {
                            Schedule.Move(removedIdx, targetIdx);

                            // Aktualizuj kolejność w bazie danych
                            for (int i = 0; i < Schedule.Count; i++)
                            {
                                Schedule[i].OrderIndex = i;
                            }
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }

        private void MarkAsWatched_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleList.SelectedItem is Movie selectedMovie)
            {
                selectedMovie.IsWatched = true;
                selectedMovie.IsInSchedule = false;

                var watched = new Watched
                {
                    MovieId = selectedMovie.Id,
                    WatchedDate = DateTime.Now
                };

                _context.Watched.Add(watched);
                _context.Movies.Update(selectedMovie);
                _context.SaveChanges();

                Schedule.Remove(selectedMovie);
                MessageBox.Show("Film oznaczony jako obejrzany.");
            }
            else
            {
                MessageBox.Show("Proszę wybrać film do oznaczenia jako obejrzane.");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

