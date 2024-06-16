using System;
using System.Collections.Generic;
using System.IO;
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
        public bool IsInSchedule { get; set; }
        public string Description { get; set; }
        public int Runtime { get; set; }
        public int OrderIndex { get; set; }
        public bool IsWatched { get; set; }
    }
}
