using System;

namespace Movie247.Helpers
{
    public class PagingModel
    {
        public int CurrentPage { get; set; }
        public int CountPage { get; set; }

        public Func<int?, string> GenerateUrl { get; set; }
    }
}
