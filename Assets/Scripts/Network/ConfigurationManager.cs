namespace Assets.Scripts.Network
{
    public static class ConfigurationManager
    {
        public const bool Debug = true;
        //public const string Server = "http://localhost:64283";
        public static string Server { get; set; }

        public static string SearchBattleUrl
        {
            get { return Server + "/battlesearch/search/"; }
        }

        public static string CheckSearchBattleUrl
        {
            get { return Server + "/battlesearch/check/"; }
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
            get { return Server + "/game/{0}/checkstate/{1}"; }
        }

        public static string TryEndUrl
        {
            get { return Server + "/game/{0}/tryend"; }
        }

        public static string LoginUserUrl
        {
            get { return Server + "/user/signin"; }
        }

        public static string SignUpUserUrl
        {
            get { return Server + "/user/signup"; }
        }

        public static string UserRatingUrl
        {
            get { return Server + "/user/rating"; }
        }

        public static string UserExpUrl
        {
            get { return Server + "/user/exp"; }
        }

        public static string StatsDataUrl
        {
            get { return Server + "/data/stats"; }
        }
    }
}