namespace clothing_be.DTO.production.vatians
{
    public class AttributeValuesDTO
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string AttriName { get; set; } = string.Empty;
        public string AttriType { get; set; } = string.Empty;
    }

    public class CreateAttributeValuesDTO
    {
        public string Value { get; set; } = string.Empty;
        public int AttributeId { get; set; }
    }

    public class MiniAttributeValuesDTO
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
