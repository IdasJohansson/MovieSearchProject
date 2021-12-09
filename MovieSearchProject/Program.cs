using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieSearchProject
{
    class Program
    {
        public static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {

            // Sparar api key i variabeln key
            DotNetEnv.Env.TraversePath().Load();
            string key = Environment.GetEnvironmentVariable("API_KEY");

            Console.WriteLine("Enter numeric ID to search for movie: ");
            int searchId = Convert.ToInt32(Console.ReadLine());
            string uri = $@"https://api.themoviedb.org/3/movie/{searchId}?api_key={key}";

            // Hämtar datan
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode(); // Metod som kollar om vi har statuskod 200 om vi har kontakt med api. Vilket behövs. 

            // Response - går in i Content och hämtar det som står där. 
            string responseContent = await response.Content.ReadAsStringAsync();

            // Skapa ett objekt utifrån jsondatan 
            MovieInfo movie = JsonConvert.DeserializeObject<MovieInfo>(responseContent);

            ShowMovieInfo(movie); 

            // Metod som visar all info om movies
            void ShowMovieInfo(MovieInfo movie)
            {
                // Hämtar värdet genom fieldsen i MovieInfo klassen
                Console.WriteLine("\nID: ");
                Console.WriteLine($"{movie.Id}");
                Console.WriteLine("\nTitle: ");
                Console.WriteLine($"{movie.Title}");
                Console.WriteLine("\nAbout:");
                Console.WriteLine($"{movie.Overview}");
                Console.WriteLine("\nLanguage:");
                Console.WriteLine($"{movie.Original_language}");
                Console.WriteLine("\nRelease date:");
                Console.WriteLine($"{movie.Release_date}");
                Console.WriteLine("\nRating: ");
                Console.WriteLine($"{movie.Vote_average}");
                Console.WriteLine("\nRuntime:");
                Console.WriteLine($"{movie.Runtime} min");
                Console.WriteLine("\nHomepage:");
                Console.WriteLine($"{movie.Homepage}");
                Console.WriteLine("\nPoster path:");
                Console.WriteLine($"https://image.tmdb.org/t/p/w500{movie.Poster_path}"); // Hårdkodat värde på den tidigare delen av strängen, resterande hämtas i Poster_path
            }

        }
    }
}
