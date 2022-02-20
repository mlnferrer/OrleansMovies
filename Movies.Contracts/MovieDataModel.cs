using System.Collections.Generic;

namespace Movies.Contracts
{
	/// <summary>
	/// Holds the Movie List
	/// </summary>
	public class MovieDataModel
	{
		/// <summary>
		/// List of Movies, did this based on the format of json
		/// </summary>
		public List<Movie> movies { get; set; }
	}

	/// <summary>
	/// Movie Class
	/// </summary>
	public class Movie
	{

		/// <summary>
		/// ctor
		/// </summary>
		public Movie()
		{
			genres = new string[] { };
		}

		/// <summary>
		/// id
		/// </summary>
		public int id { get; set; }

		/// <summary>
		/// key
		/// </summary>
		public string key { get; set; }

		/// <summary>
		/// name
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// description
		/// </summary>
		public string description { get; set; }

		/// <summary>
		/// list of genres
		/// </summary>
		public string[] genres { get; set; }

		/// <summary>
		/// rate
		/// </summary>
		public decimal rate { get; set; }

		/// <summary>
		/// length
		/// </summary>
		public string length { get; set; }

		/// <summary>
		/// img
		/// </summary>
		public string img { get; set; }
	}
}
