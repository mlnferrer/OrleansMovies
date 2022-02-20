using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Movies.Server.Controllers
{
	/// <summary>
	/// Controller for Movies with the endpoints needed for this technical exam
	/// </summary>
	public class MoviesController : Controller
	{
		private readonly IMovieClient _client;

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="client"></param>
		public MoviesController(IMovieClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Gets all movie
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("api/movies")]
		public async Task<List<Movie>> Search() => await _client.Get();

		/// <summary>
		/// List top 5 highest rated movies
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		[Route("api/movies/top5")]
		public async Task<List<Movie>> GetTop5() => await _client.GetTop5();

		/// <summary>
		/// Gets the all movie associated with the genre selected
		/// </summary>
		/// <param name="genre"></param>
		/// <returns></returns>
		[HttpGet()]
		[Route("api/movies/genre/{genre}")]
		public async Task<List<Movie>> GetByGenre(string genre)
		{
			if (string.IsNullOrEmpty(genre) || string.IsNullOrWhiteSpace(genre))
				throw new ArgumentNullException("Genre is required to fulfill this request.");

			var result = await _client.GetByGenre(genre);
			return result;
		}

		/// <summary>
		/// Gets the detail of the movie based on the key, this can also be done on the search
		/// but for completeness sake, I accomplished this:
		/// Display selected movie detail information
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpGet()]
		[Route("api/movies/{key}")]
		public async Task<Movie> GetSelectedMovieDetail(string key)
		{
			if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
				throw new ArgumentNullException("Key is required to fulfill this request.");

			var result = await _client.GetSelectedMovieDetail(key);
			return result;
		}

		/// <summary>
		/// Search a movie based on the name, rate, key or even genres
		/// </summary>
		/// <param name="searchKey"></param>
		/// <returns></returns>
		[HttpGet()]
		[Route("api/movies/search/{searchKey}")]
		public async Task<List<Movie>> Search(string searchKey)
		{
			if (string.IsNullOrEmpty(searchKey) || string.IsNullOrWhiteSpace(searchKey))
				throw new ArgumentNullException("Key is required to fulfill this request.");

			var result = await _client.Search(searchKey);
			return result;
		}

		/// <summary>
		/// Update an existing movie within the list
		/// </summary>
		/// <param name="movie"></param>
		/// <returns></returns>
		[HttpPost()]
		[Route("api/movies/{id}")]
		public async Task UpdateMovie(int id, [FromBody] Movie movie) 
		{
			if(movie == null || id == 0)
				throw new ArgumentNullException("Cannot perform update without the required information.");

			await _client.UpdateMovie(id, movie);
		}

		/// <summary>
		/// Create a new Movie within the list
		/// </summary>
		/// <param name="movie"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("api/movies")]
		public async Task<Movie> CreateMovie([FromBody] Movie movie) 
		{
			
			if (movie == null)
				throw new ArgumentNullException("Cannot perform create without the required information.");

			var result = await _client.CreateMovie(movie);
			return result;
			
		}
	}
}
