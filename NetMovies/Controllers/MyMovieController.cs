﻿namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class MyMovieController : Controller
    {
        private readonly IMovieService movies;

        public MyMovieController(IMovieService movies) => this.movies = movies;

        [Authorize]
        public IActionResult MyAllMovies(AllMovieQueryModel query)
        {
            var movies = this.movies.MyMovies(this.User.Id());
            query.Movies = movies.Movies;
            query.TotalMovies = movies.TotalMovies;

            return View(query);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var movie = movies.Details(id);

            return View(new MovieFormModel
            {
                Title = movie.Title,
                Year = movie.Year,
                ImageUrl = movie.ImageUrl,
                WatchUrl = movie.WatchUrl,
                Country = movie.Country,
                Directors = movie.Directors,
                Actors = movie.Actors,
                Duration = movie.Duration,
                Descriptions = movie.Description,
                GenreId = movie.GenreId,
                Genres = this.movies.GenreCategories()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, MovieFormModel movie)
        {
            if (!movies.GenreExists(movie.GenreId))
            {
                this.ModelState.AddModelError(nameof(movie.GenreId), "Genre does not exist.");
            }
            if (!ModelState.IsValid)
            {
                movie.Genres = this.movies.GenreCategories();

                return View(movie);
            }
            var directoraList = movies.DirectorsList(movie);
            var actorsList = movies.ActorsList(movie);

            var movieId = this.movies.Edit(id, directoraList, this.User.Id(), movie, actorsList);

            return RedirectToAction(nameof(MyAllMovies));
        }


        public IActionResult Delete(int id)
        {
            var movieForDeletet = this.movies.Delete(id);

            if (movieForDeletet)
            {
                return RedirectToAction(nameof(MyAllMovies));
            }

            return BadRequest();
        }
    }
}
