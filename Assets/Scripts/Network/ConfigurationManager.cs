namespace Assets.Scripts.Network
{
    public static class ConfigurationManager
    {
        public const bool Debug = true;
        //public const string Server = "http://localhost:64283";
        public static string Server { get; set; }

        public static string SearchBattleUrl
        {
            get { return Server + "/battlesearch/search/{0}/"; }
        }

        public static string CheckSearchBattleUrl
        {
            get { return Server + "/battlesearch/check/{0}/"; }
        }

        public static string InitFieldUrl
        {
            get { return Server + "/game/{0}/init"; }
        }

        public static string GameProcessCommandUrl
        {
            get { return Server + "/game/command"; }
        }

        public static string ActionsByTicksUrl
        {
            get { return Server + "/game/{0}/ticks/"; }
        }

        public static string GameCheckStateChanged
        {
            get { return Server + "/game/{0}/check/{1}"; }
        }

        public static string TryEndUrl
        {
            get { return Server + "/game/{0}/tryend/{1}"; }
        }

        public static string LoginUserUrl
        {
            get { return Server + "/user/signin"; }
        }

        public static string UserRatingUrl
        {
            get { return Server + "/user/rating"; }
        }

        public static string UserExpUrl
        {
            get { return Server + "/user/exp/{0}"; }
        }
    }
}