using ScandicDevJobApi.Services;
using System.Net.Http.Json;
using System.Web;

public class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _http;
    public NominatimGeocodingService(HttpClient http)
    {
        _http = http;
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("ScandicDevJobs/1.0 (contact: admin@scandicdevjobs.com)");
    }

    public async Task<(double lat, double lon)?> GeocodeAsync(string address, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(address)) return null;

        var q = HttpUtility.UrlEncode(address);
        var url = $"https://nominatim.openstreetmap.org/search?q={q}&format=json&limit=1&addressdetails=0";
        var res = await _http.GetFromJsonAsync<List<NominatimResult>>(url, cancellationToken: ct);
        var first = res?.FirstOrDefault();
        if (first == null) return null;

        if (double.TryParse(first.lat, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
            double.TryParse(first.lon, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lon))
        {
            return (lat, lon);
        }
        return null;
    }

    private record NominatimResult(string lat, string lon);
}
