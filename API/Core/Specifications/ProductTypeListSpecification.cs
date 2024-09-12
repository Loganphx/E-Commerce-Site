using Core.Entities;

namespace Core.Specifications;

public class ProductTypeListSpecification : BaseSpecification<Product, string>
{
    public ProductTypeListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}