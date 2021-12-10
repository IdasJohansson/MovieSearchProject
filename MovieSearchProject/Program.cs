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

            // Startar programmet
            Menu();

            void Menu()
            {
                Console.Clear(); 
                Console.WriteLine("Welcome to MovieSearch!");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("\nA. Search for movies by ID");
                Console.WriteLine("B. Search for movies by Title");
                Console.WriteLine("C. Exit program..");
                try
                {
                    char userChoice = Convert.ToChar(Console.ReadLine());
                    Console.Clear();

                    switch (Char.ToUpper(userChoice))
                    {
                        case 'A':
                            // Search for movies by ID
                            SearchMovieByIdAsync().Wait();
                            break;
                        case 'B':
                            // Search for movies by Title
                            SearchMovieByTitleAsync().Wait();
                            break;
                        case 'C':
                            Console.WriteLine("Program will now be closed...Press a key to exit :)");
                            Console.ReadKey();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Incorrect choice, only A, B or C is valid, press a key and try again.");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            Menu();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Something went wrong, press a key and try again.");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    Menu();
                }
            }

            async Task SearchMovieByIdAsync()
            {
                // Här sparar vi api key i variablen key. 
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

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press a key to return to Menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                Menu();
            }

            async Task SearchMovieByTitleAsync()
            {
                DotNetEnv.Env.TraversePath().Load();
                string key = Environment.GetEnvironmentVariable("API_KEY");

                Console.WriteLine("Enter a title to search for movie: ");
                string searchForTitle = Console.ReadLine();

                // Länk till api för att kunna söka via titel. Api key är lagrat i {key}, userinput i {searchForTitle}
                string titlesearch = $@"https://api.themoviedb.org/3/search/movie?api_key={key}&query={searchForTitle}";

                // Hämtar datan
                var resp = await client.GetAsync(titlesearch);
                resp.EnsureSuccessStatusCode(); // Metod som kollar om vi har statuskod 200 om vi har kontakt med api. Vilket behövs. 

                // Resp - går in i Content och hämtar det som står där. 
                string respContent = await resp.Content.ReadAsStringAsync();

                // Skapa ett objekt utifrån jsondatan 
                SearchTitle search = JsonConvert.DeserializeObject<SearchTitle>(respContent);

                // Om listan Results med objekt är större än 0 skrivs resultaten ut annars körs villkoret i else-satsen. 
                //search.Results listan med alla filmobjekt i
                if (search.Results.Count > 0)
                {
                    Console.WriteLine("\nMovies found: ");
                    foreach (var item in search.Results)
                    {
                        // Skriver ut en lista med alla titlar som inkluderar det man söker på
                        Console.WriteLine($"{item.Title}");
                    }
                    /*
                     obs funkar ej
                    Console.WriteLine("\nEnter a number to see some more info about the movie:  (list starts with index 0)");
                    int userInput = Convert.ToInt32(Console.ReadLine());
                    ShowMovieInfo(search.Results[userInput]);
                    */
                }
                else
                {
                    Console.WriteLine($"{searchForTitle}, is not found.");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press a key to return to menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                Menu();
            }

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
