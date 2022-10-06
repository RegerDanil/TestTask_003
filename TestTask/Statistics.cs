using System;

namespace TestTask
{
    public static class Statistics
    {
        public static int numberOfSuccessfulExchangeCycles { get; set; }
        public static int numberOfUnsuccessfulExchangesCycles { get; set; }
        public static int numberOfBytesPerTransfer { get; set; }
        public static int numberOfBytesPerReception { get; set; }
        public static TimeSpan downtimeInterval { get; set; }
        public static string timeFirstCycle { get; set; }
        public static string timeLastCycle { get; set; }
    }
}
