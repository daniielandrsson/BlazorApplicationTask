using BlazorApp.Client.Pages;
using BlazorApp.Shared;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;

namespace BlazorApp.Client.Services.DocumentService
{
    public class DocumentService : IDocumentService
    {
        public List<Document> Documents { get; set; } = new List<Document>();
        public string SearchTerm { get; set; }
        public string Message { get; set; }
        public string PageHeading { get; set; }

        private readonly HttpClient _http;

        public DocumentService(HttpClient http)
        {
            _http = http;
        }

        public async Task SearchDocuments(string searchTerm)
        {
            SearchTerm = searchTerm;
            PageHeading = "Search results";
            var result = await _http.GetFromJsonAsync<ServiceResponse<DocumentSearchResult>>($"api/document/search/{searchTerm}");
            Documents = result.Data.Documents;
        }
    }
}
