namespace Game.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class CommonWarningViewModel
    {
        public string Summary { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}