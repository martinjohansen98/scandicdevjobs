namespace ScandicDevJobApi.Services
{
    public interface IGeocodingService
    {
        Task<(double lat, double lon)?> GeocodeAsync(string address, CancellationToken ct = default);
    }
}
