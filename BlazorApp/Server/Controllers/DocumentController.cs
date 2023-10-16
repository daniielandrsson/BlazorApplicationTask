using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorApp.Shared;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using System.Web;
using BlazorApp.Server.Services.DocumentService;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<ServiceResponse<DocumentSearchResult>>> GetSpecifiedDocuments(string searchTerm)
        {
            try
            {
                var response = await _documentService.SearchDocuments(searchTerm);

                return StatusCode(response.Data.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Status code: {StatusCodes.Status500InternalServerError.ToString()} - Error retrieving data from the server");
            }
        }
    }
}
