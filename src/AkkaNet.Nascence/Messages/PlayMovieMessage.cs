namespace AkkaNet.Nascence.Messages
{
    public class PlayMovieMessage
    {
        public string Title { get; private set; }
        public int UserId { get; private set; }

        public PlayMovieMessage(string title, int userId)
        {
            Title = title;
            UserId = userId;
        }
    }
}