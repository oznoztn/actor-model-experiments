using System;
using System.Collections.Generic;
using Akka.Actor;
using AkkaNet.MovieStreaming.Exceptions;
using AkkaNet.MovieStreaming.Irrelevant;
using AkkaNet.MovieStreaming.Messages;

namespace AkkaNet.MovieStreaming.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly Dictionary<string, int> _moviePlayCounts;
        public MoviePlayCounterActor()
        {
            ColorConsole.WriteLine("MoviePlayCounterActor constructor executing", ConsoleColor.Magenta);
            _moviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(HandleIncrementMessage);
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if (message.MovieTitle == "corrupt")
            {
                throw new SimulatedInvalidMovieException();
            }

            if (message.MovieTitle == "boom")
            {
                throw new SimulatedCorruptStateException();
            }

            if (_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts[message.MovieTitle]++;
            }
            else
            {
                _moviePlayCounts.Add(message.MovieTitle, 1);
            }

            ColorConsole.WriteLine($"MoviePlayCounterActor '{message.MovieTitle}' has been watched {_moviePlayCounts[message.MovieTitle]} times.", ConsoleColor.Magenta);
        }
    }
}