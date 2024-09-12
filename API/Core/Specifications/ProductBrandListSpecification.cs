using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductBrandListSpecification : BaseSpecification<Product, string>
{
    public ProductBrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}