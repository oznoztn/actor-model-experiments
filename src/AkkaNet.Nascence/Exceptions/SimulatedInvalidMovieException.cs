using System;

namespace AkkaNet.MovieStreaming.Exceptions
{
    public class SimulatedInvalidMovieException : Exception
    {
        public SimulatedInvalidMovieException(string movieTitle)
        {
            MovieTitle = movieTitle;
        }

        public string MovieTitle { get; private set; }
    }
}