namespace BlazorApp.Client.Services.DocumentService
{
    public interface IDocumentService
    {
        List<Document> Documents { get; set; }

        string SearchTerm { get; set; }
        string Message { get; set; }
        string PageHeading { get; set; } 

        Task SearchDocuments(string searchTerm);
    }
}
