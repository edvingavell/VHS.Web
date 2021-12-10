using System;

namespace VHS.Core
{
    public class Identity
    {
        public static string CdsToken { get; set; }
        public static Guid CdsUserId { get; set; }
        public static Guid CdsCustomerId { get; set; }
    }
}
