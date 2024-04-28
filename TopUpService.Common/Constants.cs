namespace TopUpService.Common
{
    public static class Constants
    {
        public static readonly string AED5 = "AED 5";
        public static readonly string AED10 = "AED 10";
        public static readonly string AED20 = "AED 20";
        public static readonly string AED30 = "AED 30";
        public static readonly string AED50 = "AED 50";
        public static readonly string AED75 = "AED 75";
        public static readonly string AED100 = "AED 100";
        public static List<string> GetTopUpOptions()
        {
            return [AED5, AED10, AED20, AED30, AED50, AED75, AED100];
        }
    }
}
