namespace Assets.Scripts.Network
{
    public static class ConfigurationManager
    {
        public const bool Debug = true;
        //public const string Server = "http://localhost:64283";
        public static string Server { get; set; }

        public static string SearchBattleUrl => Server + "/battlesearch/";

        public static string SinglePlayUrl => Server + "/battlesearch/singleplay";

        public static string SearchMultiBattleUrl => Server + "/battlesearch/multibattle/";

        public static string CheckSearchBattleUrl => Server + "/battlesearch/check/";

        public static string InitFieldUrl => Server + "/game/{0}/init";

        public static string GameProcessCommandUrl => Server + "/game/command";

        public static string ActionsByTicksUrl => Server + "/game/{0}/ticks/";

        public static string GameCheckStateChanged => Server + "/game/{0}/checkstate/{1}";

        public static string TryEndUrl => Server + "/game/{0}/tryend";

        public static string LoginUserUrl => Server + "/user/signin";

        public static string SignUpUserUrl => Server + "/user/signup";

        public static string UserRatingUrl => Server + "/user/rating";

        public static string UserExpUrl => Server + "/user/exp";

        public static string StaticDataUrl => Server + "/data/static";

        public static string ServerUrlStore => "https://towerland.s3.eu-central-1.amazonaws.com/server_urls.txt";
    }
}