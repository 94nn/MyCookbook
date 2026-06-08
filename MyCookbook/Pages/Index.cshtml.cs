using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCookbook.API.Models;
using System.Text.Json;

namespace MyCookbook.Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IndexModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<Dish> Dishes { get; set; } = new();

    public async Task OnGetAsync()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";

        var client = _httpClientFactory.CreateClient();

        // 🔹 This reads from your freshly built Dishes database endpoint!
        var response = await client.GetAsync($"https://localhost:7299/api/Dishes");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            Dishes = JsonSerializer.Deserialize<List<Dish>>(jsonString, options) ?? new List<Dish>();
        }
    }
}