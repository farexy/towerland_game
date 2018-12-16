namespace Helpers
{
    public static class ComputerPlayer
    {
        public static string SessionKey { get; private set; }

        public static void Init(string sessionKey)
        {
            SessionKey = sessionKey;
        }
    }
}