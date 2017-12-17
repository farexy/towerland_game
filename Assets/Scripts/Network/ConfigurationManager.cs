namespace Assets.Scripts.Network
{
    public static class ConfigurationManager
    {
        public const bool Debug = true;
        public const string Server = "http://localhost:64283";

        public static string SearchBattleUrl = Server + "/battlesearch/search/{0}/";
        public static string CheckSearchBattleUrl = Server + "/battlesearch/check/{0}/";

        public static string GameProcessCommandUrl = Server + "/game/command";
        public static string ActionsByTicksUrl = Server + "/game/{0}/ticks/";
        public static string GameCheckStateChanged = Server + "/game/{0}/check/{1}";
        public static string TryEndUrl = Server + "/game/{0}/tryend/{1}";
    }
}