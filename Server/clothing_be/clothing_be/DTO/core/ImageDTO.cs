namespace clothing_be.DTO.core
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Type { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string? AltText { get; set; }
    }
}
