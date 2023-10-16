using BlazorApp.Shared;

namespace BlazorApp.Server.Services.DocumentService
{
    public interface IDocumentService
    {
        List<Document> Documents { get; set; }
        string SearchTerm { get; set; }

        Task<ServiceResponse<DocumentSearchResult>> SearchDocuments(string searchTerm);
    }
}
