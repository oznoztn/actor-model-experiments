using System;
using Akka.Actor;
using Akka.Event;
using AkkaNet.MovieStreaming.Irrelevant;

namespace AkkaNet.MovieStreaming.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        private readonly ILoggingAdapter _loggingAdapter = Context.GetLogger();

        public PlaybackActor()
        {
            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            _loggingAdapter.Debug("PlaybackActor PreStart");
        }

        protected override void PostStop()
        {
            _loggingAdapter.Debug("PlaybackActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _loggingAdapter.Debug("PlaybackActor PreRestart because: {0}", message);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _loggingAdapter.Debug("PlaybackActor PostRestart because: {0}", reason.Message);

            base.PostRestart(reason);
        }
        #endregion
    }
}
