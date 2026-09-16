namespace clothing_be.DTO.production.vatians
{
    public class AttributeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<MiniAttributeValuesDTO> AttributeValues { get; set; } = new();
    }

    public class CreateAttributeDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
