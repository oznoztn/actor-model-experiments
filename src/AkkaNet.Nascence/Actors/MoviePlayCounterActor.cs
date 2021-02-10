using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using AkkaNet.MovieStreaming.Exceptions;
using AkkaNet.MovieStreaming.Irrelevant;
using AkkaNet.MovieStreaming.Messages;

namespace AkkaNet.MovieStreaming.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly ILoggingAdapter _loggingAdapter = Context.GetLogger();

        private readonly Dictionary<string, int> _moviePlayCounts;
        public MoviePlayCounterActor()
        {
            _loggingAdapter.Debug("MoviePlayCounterActor constructor executing");
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

            _loggingAdapter.Info("MoviePlayCounterActor '{0}' has been watched {1} times.", message.MovieTitle, _moviePlayCounts[message.MovieTitle]);
        }
    }
}