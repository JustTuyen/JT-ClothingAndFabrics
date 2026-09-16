namespace clothing_be.DTO.production.tag
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreateTagDTO
    {
        public string Name { get; set; } = string.Empty;
        public int StatusId { get; set; }
    }

    public class ProductTagDTO
    {
        public int Id { get; set; }
        public int TagId { get; set; } 
        public int ProductId { get; set; } 
    }
}
