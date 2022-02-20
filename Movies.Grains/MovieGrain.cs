using Microsoft.Extensions.Logging;
using Movies.Contracts;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<List<Movie>>, IMovieGrain
	{
		private readonly ILogger _logger;

		public MovieGrain(ILogger<MovieGrain> logger)
		{
			_logger = logger;
		}

		public Task Initialize()
		{
			_logger.LogInformation("Initializing Movie Data");

			//This can be improved
			string jsonData = System.IO.File.ReadAllText(@"movies.json");
			MovieDataModel model = JsonConvert.DeserializeObject<MovieDataModel>(jsonData);

			State = model.movies;

			_logger.LogInformation("Initialized");
			return Task.CompletedTask;
		}

		public Task<List<Movie>> Get()
		{
			_logger.LogInformation("Getting all movies..");
			return Task.FromResult(State);
		}

		public Task<List<Movie>> GetTop5()
		{
			_logger.LogInformation("Getting top 5 highest rated movies..");
			return Task.FromResult(Get().Result.OrderByDescending(x => x.rate).Take(5).ToList());
		}

		public Task<List<Movie>> GetByGenre(string genre)
		{
			_logger.LogInformation("Getting movies with genre - {0}", genre);
			return Task.FromResult(Get().Result.Where(x => x.genres.Contains(genre)).ToList());
		}

		public Task<Movie> GetSelectedMovieDetail(string key) {
			
			_logger.LogInformation("Getting movies with key - {0}", key);
			return Task.FromResult(Get().Result.Where(x => x.key.Equals(key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
		}
		
		public Task<List<Movie>> Search(string searchKey) {

			_logger.LogInformation("Getting movies with based on search filter - {0}", searchKey);

			var items = Get().Result.Where(x => x.id.ToString().Equals(searchKey) ||
												x.key.Equals(searchKey, StringComparison.OrdinalIgnoreCase) ||
												x.name.ToLowerInvariant().Contains(searchKey.ToLowerInvariant()) ||
												x.rate.ToString().Equals(searchKey) ||
												x.genres.Contains(searchKey));
			
			return Task.FromResult(items.ToList());
		}

		public Task UpdateMovie(int id, Movie movie)
		{
			_logger.LogInformation("Updating movie with id - {0}", id);

			var item = Get().Result.Where(x => x.id == id).SingleOrDefault();

			if(item == null)
			{
				_logger.LogError("Movie not found with id - {0}", id);
				throw new Exception("Movie not found");
			}
			
			item.length = movie.length;
			item.img = movie.img;
			item.rate = movie.rate;
			item.description = movie.description;
			item.name = movie.name;
			item.genres = movie.genres;

			WriteStateAsync();
			return Task.CompletedTask;
		}

		public Task<Movie> CreateMovie(Movie movie)
		{
			_logger.LogInformation("Creating movie with key - {0}", movie.key);

			//Check if the key is existing, checking the id as well because you never know... the id is being sent here 
			//and we want to avoid duplicates, given that this is only getting from json file
			var item = Get().Result.Where(x => x.key.Equals(movie.key, StringComparison.OrdinalIgnoreCase) 
			|| x.id.Equals(movie.id)).SingleOrDefault();

			if (item != null)
			{
				_logger.LogError("Movie with key {0} is already existing ", movie.key);
				throw new Exception("Movie already existing.");
			}

			//Just getting the max and increment
			movie.id = Get().Result.Max(u => u.id) + 1;
			State.Add(movie);

			WriteStateAsync();
			return Task.FromResult(movie);
		}
	}
}
