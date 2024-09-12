namespace Web_253505_Sniazhko.UI.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class Book { public int BookId { get; set; } public string Name { get; set; } public string Author { get; set; } }
}
