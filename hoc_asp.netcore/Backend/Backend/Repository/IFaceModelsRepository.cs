namespace Backend.Repository
{
    public interface IFaceModelsRepository
    {
        Task<int> SaveImageUrlsAsync(List<string> imageUrls, int employeeId);
    }
}
