namespace AkkaNet.Nascence.Messages
{
    public class PlayMovieMessage
    {
        public string Title { get; private set; }
        public PlayMovieMessage(string title)
        {
            Title = title;
        }
    }
}