using System;

namespace AkkaNet.MovieStreaming.Exceptions
{
    public class SimulatedCorruptStateException : Exception
    {
        public SimulatedCorruptStateException(string movieTitle)
        {
            MovieTitle = movieTitle;
        }

        public string MovieTitle { get; private set; }
    }
}
