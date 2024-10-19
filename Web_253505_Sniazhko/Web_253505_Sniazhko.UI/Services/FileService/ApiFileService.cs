
using Web_253505_Sniazhko.UI.Services.Authentication;

namespace Web_253505_Sniazhko.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;
        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }
        public async Task DeleteFileAsync(string file)
        {
            if (string.IsNullOrEmpty(file))
                return;
            Uri uri = new Uri(file);
            string fileName = Path.GetFileName(uri.LocalPath);
            var request = new HttpRequestMessage(HttpMethod.Delete, _httpClient.BaseAddress + "/" + fileName);
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Error deleting file at {file}");
            }
        }
        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return String.Empty;
            var request = new HttpRequestMessage(HttpMethod.Post, ""); // URL будет настроен через BaseAddress
            // Сгенерировать случайное имя файла с сохранением расширения
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            // Создать контент с файлом для передачи
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName); // Имя параметра "file" должно совпадать с именем в контроллере
            // Установить контент запроса
            request.Content = content;
            // Отправить запрос к API
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Вернуть URL сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }
    }
}
