﻿namespace NetMovies.Models.Movie
{
    using NetMovies.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AllMovieQueryModel
    {
        public const int MoviesPerPage = 3;

        public string SearchTerm { get; set; }

        public int TotalMovies { get; set; }

        public int CurrentPage { get; set; } = 1;

        public IEnumerable<MovieServiceModel> Movies { get; set; }
    }
}