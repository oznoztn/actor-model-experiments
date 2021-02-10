using System;
using Akka.Actor;
using Akka.Event;
using AkkaNet.MovieStreaming.Irrelevant;
using AkkaNet.MovieStreaming.Messages;

namespace AkkaNet.MovieStreaming.Actors
{
    public class UserActor : ReceiveActor
    {
        private readonly int _userId;
        private string _currentlyWatching;
        private ILoggingAdapter _loggingAdapter = Context.GetLogger();

        public UserActor(int userId)
        {
            _userId = userId;
            
            _loggingAdapter.Debug("UserActor:{0} Constructor", userId);
            _loggingAdapter.Debug("Setting initial behavior to 'Stopped'");
            
            Stopped();
        }

        // Represents the 'playing' behavior
        private void Playing()
        {
            Receive<PlayMovieMessage>(message => 
                _loggingAdapter
                    .Warning("ERROR: User:{0} Cannot start playing another movie ({1}) before stopping the existing one.", _userId, message.Title));
            
            Receive<StopMovieMessage>(message => StopPlayingMovie());

            _loggingAdapter.Info("UserActor:{0} has now become 'Playing'.", _userId);
        }

        // Represents the 'stopped' behavior
        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.Title));
            Receive<StopMovieMessage>(message => _loggingAdapter.Warning("ERROR: User:{0} Cannot stop if nothing is playing.", _userId));

            _loggingAdapter.Info("UserActor:{0} has now become 'Stopped'.", _userId);
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;
            _loggingAdapter.Info("User:{0} is watching {1}.", _userId, title);

            Context
                .ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(title));

            Become(Playing);
        }

        private void StopPlayingMovie()
        {
            _loggingAdapter.Info("User:{0} has stopped watching {1}.", _userId, _currentlyWatching);
            Become(Stopped);
        }

        protected override void PreStart()
        {
            _loggingAdapter.Debug("UserActor:{0} PreStart", _userId);
            base.PreStart();
        }

        protected override void PostStop()
        {
            _loggingAdapter.Debug("UserActor:{0} PostStop", _userId);
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _loggingAdapter.Debug("UserActor:{0} PreRestart", _userId);
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _loggingAdapter.Debug("UserActor:{0} PostRestart", _userId);
            base.PostRestart(reason);
        }
    }
}
