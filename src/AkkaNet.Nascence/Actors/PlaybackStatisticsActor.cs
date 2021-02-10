using System;
using Akka.Actor;
using Akka.Event;
using AkkaNet.MovieStreaming.Exceptions;
using AkkaNet.MovieStreaming.Irrelevant;

namespace AkkaNet.MovieStreaming.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {
        private readonly ILoggingAdapter _loggingAdapter = Context.GetLogger();

        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(Decider);
        }

        private Directive Decider(Exception exception)
        {
            if (exception is SimulatedCorruptStateException)
            {
                return Directive.Restart;
            }

            if (exception is SimulatedInvalidMovieException)
            {
                return Directive.Resume;
            }

            return Directive.Restart;
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            _loggingAdapter.Debug("PlaybackStatisticsActor PreStart");
        }

        protected override void PostStop()
        {
            _loggingAdapter.Debug("PlaybackStatisticsActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _loggingAdapter.Debug("PlaybackStatisticsActor PreRestart because: {0}", reason.Message);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _loggingAdapter.Debug("PlaybackStatisticsActor PostRestart because: {0}", reason.Message);

            base.PostRestart(reason);
        }
        #endregion
    }
}