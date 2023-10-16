using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class DocumentSearchResult
    {
        public List<Document> Documents { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
