using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://localhost:7001/");
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var res = await _http.GetAsync("api/" + url);
        if (!res.IsSuccessStatusCode) return default;
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> PostAsync<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var res = await _http.PostAsync("api/" + url, new StringContent(json, Encoding.UTF8, "application/json"));
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var res = await _http.PutAsync("api/" + url, new StringContent(json, Encoding.UTF8, "application/json"));
        return res.IsSuccessStatusCode;
    }
    public async Task<T?> PostForObjectAsync<T>(string url, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var res = await _http.PostAsync("api/" + url, new StringContent(json, Encoding.UTF8, "application/json"));
        if (!res.IsSuccessStatusCode) return default;
        var json2 = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var res = await _http.DeleteAsync("api/" + url);
        return res.IsSuccessStatusCode;
    }
}
