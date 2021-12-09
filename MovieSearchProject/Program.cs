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

            Console.WriteLine(movie.Title);

        }
    }
}
