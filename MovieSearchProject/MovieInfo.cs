using System;
namespace MovieSearchProject
{
    public class MovieInfo
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; } // Om filmen
        public string Original_language { get; set; }
        public string Release_date { get; set; }
        public double Vote_average { get; set; }
        public int Runtime { get; set; }
        public string Homepage { get; set; }
        public string Poster_path { get; set; } // Endast halva strängen kommer med skickar med hårdkodat värde för första delen i metoden 


        public MovieInfo(int id, string title, string overview, string original_language, string release_date, double vote_average, int runtime, string homepage, string poster_path)
        {
            this.Id = id;
            this.Title = title;
            this.Overview = overview;
            this.Original_language = original_language;
            this.Release_date = release_date;
            this.Vote_average = vote_average;
            this.Runtime = runtime;
            this.Homepage = homepage;
            this.Poster_path = poster_path;
        }


    }
}
