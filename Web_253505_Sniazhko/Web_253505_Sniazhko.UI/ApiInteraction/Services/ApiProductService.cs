using System.Text;
using System.Text.Json;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;
using Web_253505_Sniazhko.UI.Services.Authentication;
using Web_253505_Sniazhko.UI.Services.FileService;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.ApiInteraction.Services
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly IFileService _fileService;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;
        ITokenAccessor _tokenAccessor;
        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value ?? "3";
            _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            _logger = logger;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
        }
        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}dishes");
            if (categoryNormalizedName != null)
            {
                urlString.Append($"/category/{categoryNormalizedName}/");
            }
            var a = new Uri(urlString.ToString());
            if (pageNo > 1)
            {
                urlString.Append($"?pageNo={pageNo}");
            }
            if (!_pageSize.Equals("3"))
            {
                if (pageNo > 1)
                    urlString.Append($"&pageSize={_pageSize}");
                else
                    urlString.Append($"?pageSize={_pageSize}");
            }
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<ListModel<Dish>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return ResponseData<ListModel<Dish>>.Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        }
        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            product.Image = "Images/noimage.jpg";
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl; // Установить URL загруженного изображения
                }
            }
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.PostAsJsonAsync(uri, product);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>();
                return data;
            }
            _logger.LogError($"-----> object not created. Error: {response.StatusCode.ToString()}");
            return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
        }
        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"dishes/{id}");
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> Продукт не найден. Error: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Продукт не найден. Error: {response.StatusCode}");
        }
        public async Task UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (!string.IsNullOrEmpty(product.Image) && product.Image != "Images/noimage.jpg")
                    {
                        await _fileService.DeleteFileAsync(product.Image);
                    }
                    product.Image = imageUrl;
                }
            }
            // Формирование URI для запроса обновления
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"dishes/{id}");
            // Отправка запроса на обновление объекта
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Объект не обновлен. Error: {response.StatusCode}");
                throw new InvalidOperationException($"Объект не обновлен. Error: {response.StatusCode}");
            }
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}dishes/{id}");
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.DeleteAsync(uri);

            Dish data = product.Data;
            await _fileService.DeleteFileAsync(data.Image);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not deleted. Error: {response.StatusCode}");
                throw new InvalidOperationException($"Объект не удален. Error: {response.StatusCode}");
            }
        }
    }
}
