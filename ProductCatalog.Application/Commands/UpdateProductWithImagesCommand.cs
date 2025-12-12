namespace ProductCatalog.Application.Commands
{
    public class UpdateProductWithImagesCommand : CreateProductWithImagesCommand
    {
        public int Id { get; set; }
    }
}
