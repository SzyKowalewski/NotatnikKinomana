using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotatnikKinomana.Models
{
    public class Watched
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public DateTime WatchedDate { get; set; }
        public Movie Movie { get; set; }
    }
}
