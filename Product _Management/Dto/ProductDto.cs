namespace Product_Management.Dto
{
    public record ProductDto(int Id, string Name, decimal Price, string Category);
    public record ProductCreateDto(string Name, decimal Price, int CategoryId);
}
