using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotatnikKinomana.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime? PremiereDate { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
    }
}
