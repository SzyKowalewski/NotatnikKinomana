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
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string review = ReviewTextBox.Text;

            var movie = _context.Movies.FirstOrDefault(m => m.Title == title);
            if (movie != null)
            {
                movie.Review = review;
                _context.Movies.Update(movie);
                _context.SaveChanges();
                MessageBox.Show($"Dodano recenzję: {title}");
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Nie znaleziono filmu o podanym tytule.");
            }
        }
    }
}
