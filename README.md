# Movies Application

A sample application that showcases the use of Microsoft Orleans by creating a various set of APIs to cater the following functionality:
  - List top 5 highest rated movies
  - List Movies
  - Search
  - Filter by Genre
  - Display selected movie detail information
  - Create a new movie that can be retrieved in the movies list
  - Update movies data.  

Data is preloaded under SiloBuilderExtensions and is being done by adding a start up task.

### Running the Movie Application

- Make sure the startup project is set to `Movies.Server`
- The project has the controller `MoviesController` that consists of the following request to showcase the features of the app:
  - [GET] http://localhost:6600/api/movies
      - This endpoint returns the list of all pre-loaded data of movies
  - [GET] http://localhost:6600/api/movies/top5
      - This endpoint returns the list of top 5 movies with the highest rank
  - [GET] http://localhost:6600/api/movies/genre/{genre}
      - This endpoint returns the list of movies with the specific type of genre
  - [GET] http://localhost:6600/api/movies/{key}
      - This endpoint returns selected movie detail information
  - [GET] http://localhost:6600/api/movies/search/{searchKey}
      - This endpoint returns a list of movies based on the search text that they sent, can be either searching for id, name, key, rate, or genres.
  - [POST] http://localhost:6600/api/movies/{id}
      - This requires a request payload of an existing movie 
      - Sample request:
        {
            "id": 4,
            "key": "gridiron-gang",
            "name": "Gridiron Gang Say Whut Now?",
            "description": "Teenagers at a juvenile detention center, under the leadership of their counselor, gain self-esteem by playing football together.",
            "genres": [
                "crime",
                "drama",
                "sport"
            ],
            "rate": "6.9",
            "length": "2hr 5mins",
            "img": "gridiron1-gang.jpg"
        }
  - [PUT] http://localhost:6600/api/movies 
	- This requires a request payload of an existing movie 
        - Sample request: 
        {
    		"key": "spider-man-no-way-home",
   		"name": "Spider Man: No Way Home",
    		"description": "With Spider-Man's identity now revealed, our friendly neighborhood web-slinger is unmasked and no longer able to separate his 
				normal life as Peter Parker from the high stakes of being a superhero. 
				When Peter asks for help from Doctor Strange, the stakes become even more dangerous, 
				forcing him to discover what it truly means to be Spider-Man.",
    		"genres": [
        		"action",
        		"superhero",
        		"comedy",
			"adventure"
    		],
    		"rate": 7.0,
    		"length": "2hr 28mins",
    		"img": "pretty-woman.jpg"
	 }
