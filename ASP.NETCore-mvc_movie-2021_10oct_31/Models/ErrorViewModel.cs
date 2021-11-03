using System;

namespace ASP.NETCore_mvc_movie_2021_10oct_31.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
