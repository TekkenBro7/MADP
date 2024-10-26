using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;

namespace Web_253505_Sniazhko.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly string _apiUrl;
        private readonly int _pageSize;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _apiUrl = configuration["ApiUrl"];
            _pageSize = int.Parse(configuration["PageSize"]);
        }
        public event Action DataLoaded;
        public List<Category> Categories { get; set; } = new();
        public List<Dish> Dishes { get; set; } = new();
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalPages { get; set; } = 1;
        public int CurrentPage { get; set; } = 1;
        public Category SelectedCategory { get; set; }
        public async Task GetProductListAsync(int pageNo = 1)
        {
            try
            {
                var tokenRequest = await _tokenProvider.RequestAccessToken();
                if (tokenRequest.TryGetToken(out var token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
                }
                else
                {
                    Success = false;
                    ErrorMessage = "Unable to acquire token.";
                    return;
                }
                var route = new StringBuilder("dishes/");
                if (SelectedCategory != null)
                {
                    route.Append($"category/{SelectedCategory.NormalizedName}/");
                }
                var queryData = new List<KeyValuePair<string, string>>();
                if (pageNo > 1)
                {
                    queryData.Add(KeyValuePair.Create("pageNo", pageNo.ToString()));
                }

                if (_pageSize != 3)
                {
                    queryData.Add(KeyValuePair.Create("pageSize", _pageSize.ToString()));
                }
                var fullRoute = queryData.Count > 0
                    ? QueryHelpers.AddQueryString(route.ToString(), queryData)
                   : route.ToString();


                //var fullRoute = route.ToString();
                //if (queryData.Count > 0)
                //{
                //    route.Append((queryData));
                //}

                //  string a = "213";

                var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Dish>>>($"{_apiUrl}/{fullRoute}");
                if (response != null && response.Successful && response.Data != null)
                {
                    Dishes = response.Data.Items;
                    TotalPages = response.Data.TotalPages;
                    CurrentPage = pageNo;
                    DataLoaded?.Invoke();
                }
                else
                {
                    Success = false;
                    ErrorMessage = response?.ErrorMessage ?? "Unknown error";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorMessage = ex.Message;
            }
        }

        public async Task GetCategoryListAsync()
        {
            try
            {
                var tokenRequest = await _tokenProvider.RequestAccessToken();
                if (tokenRequest.TryGetToken(out var token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
                }
                else
                {
                    Success = false;
                    ErrorMessage = "Unable to acquire token.";
                    return;
                }

                // Запрос категорий с токеном в заголовках
                var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>($"{_apiUrl}/categories");
                if (response != null && response.Successful && response.Data != null)
                {
                    Categories = response.Data;
                    DataLoaded?.Invoke();
                }
                else
                {
                    Success = false;
                    ErrorMessage = response?.ErrorMessage ?? "Unknown error";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorMessage = ex.Message;
            }
        }

    }
}
