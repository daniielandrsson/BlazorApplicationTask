using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class ApiSearcher
    {
        private string _baseUrl;
        private HttpClient _http;

        public string BaseUrl { get => _baseUrl; set { _baseUrl = value; } }
        public HttpClient HttpClient { get => _http; set { _http = value; } }

        public ApiSearcher()
        {
        }
        public ApiSearcher(string baseUrl, HttpClient http)
        {
            _baseUrl = baseUrl;
            _http = http;
        }
    }
}
