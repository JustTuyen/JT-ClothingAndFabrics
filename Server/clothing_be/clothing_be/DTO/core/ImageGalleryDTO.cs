namespace clothing_be.DTO.core
{
    public class ImageGalleryDTO
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public int ProductId { get; set; }
        public int ImageId { get; set; }
    }

    public class MiniImageGalleryDTO
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string ImageURL { get; set; } = string.Empty;
    }
}
