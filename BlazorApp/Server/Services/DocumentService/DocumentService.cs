using BlazorApp.Shared;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace BlazorApp.Server.Services.DocumentService
{
    public class DocumentService : IDocumentService
    {
        private readonly IConfiguration _config;
        private readonly ApiSearcher _apiSearcher;
        private readonly string _defaultRows;

        public DocumentService(IConfiguration config)
        {
            _config = config;
            string defaultRows = _config.GetValue<string>("DefaultRows");
            _defaultRows = defaultRows;
            string baseUrl = _config.GetValue<string>("APIBaseUrl");
            _apiSearcher = new ApiSearcher(baseUrl, new HttpClient());
        }

        public List<Document> Documents { get; set; } = new List<Document>();
        public string SearchTerm { get; set; }
        public HttpClient Http { get; }

        public async Task<ServiceResponse<DocumentSearchResult>> SearchDocuments(string searchTerm)
        {
            string uriString = BuildUriString(searchTerm);
            HttpResponseMessage Res;

            using (_apiSearcher.HttpClient)
            {
                _apiSearcher.HttpClient.BaseAddress = new Uri(uriString);

                //Define request data format  
                _apiSearcher.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request
                Res = await _apiSearcher.HttpClient.GetAsync(_apiSearcher.HttpClient.BaseAddress);

                //Checking the response is successful
                if (Res.IsSuccessStatusCode)
                {
                    if (Documents.Count > 0)
                        Documents.Clear();

                    //Storing the response details recieved from web api   
                    string documentResponse = Res.Content.ReadAsStringAsync().Result;

                    //Converting to Jobjects
                    JObject o = JObject.Parse(documentResponse);

                    if (o.ContainsKey("documents"))
                    {
                        var doc = o["documents"];

                        foreach (var item in doc)
                        {
                            var childTokens = item.Children().ToArray();

                            string id = (string)childTokens[0].SelectToken("id");
                            string displayTitle = (string)childTokens[0].SelectToken("display_title");

                            if (!string.IsNullOrEmpty(id))
                                Documents.Add(new Document { Id = id, Display_Title = displayTitle });
                        }
                    }
                }
            }
            var response = new ServiceResponse<DocumentSearchResult>
            {
                Data = new DocumentSearchResult
                {
                    Documents = Documents,
                    StatusCode = (int)Res.StatusCode,
                    Message = Res.ReasonPhrase
                }
            };
            return response;
        }
        private string BuildUriString(string searchTerm)
        {
            string search_displayTitle = string.Empty;
            string search_queryParam = string.Empty;

            if (searchTerm.Contains('&'))
            {
                string[] splitQuery = searchTerm.Split('&');
                search_displayTitle = splitQuery[0];
                search_queryParam = splitQuery[1];
            }
            else if (searchTerm.StartsWith("display_title", StringComparison.OrdinalIgnoreCase))
                search_displayTitle = searchTerm;
            else
                search_queryParam = searchTerm;

            search_displayTitle = $"&{search_displayTitle}";
            search_queryParam = ExtractQueryParameterFromInput(search_queryParam);
            
            float pageRows = float.Parse(_defaultRows);
            string rowsParameter = $"&rows={pageRows}";

            return $"{_apiSearcher.BaseUrl}{search_displayTitle}{search_queryParam}{rowsParameter}";
        }
        private string ExtractQueryParameterFromInput(string searchTerm_queryParam)
        {
            if (searchTerm_queryParam.StartsWith("query_parameter="))
                searchTerm_queryParam = searchTerm_queryParam.Substring(searchTerm_queryParam.IndexOf("=") + 1);

            return $"&{searchTerm_queryParam}";
        }
    }
}

