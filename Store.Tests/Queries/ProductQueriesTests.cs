using Store.Domain.Entities;
using Store.Domain.Queries;

namespace Store.Tests.Queries;

[TestClass]
public class ProductQueriesTests
{
    private IList<Product> _products;

    public ProductQueriesTests()
    {
        _products = new List<Product>();
        _products.Add(new Product("Produto 1",10,true));
        _products.Add(new Product("Produto 2",10,true));
        _products.Add(new Product("Produto 3",10,true));
        _products.Add(new Product("Produto 4",10,false));
        _products.Add(new Product("Produto 5",10,false));

    }
    [TestCategory("Queries")]
    [TestMethod]
    public void Dado_a_consulta_de_produtos_ativos_deve_retornar_3()
    {
        var result = _products.AsQueryable().Where(ProductQueries.GetActiveProducts());
        Assert.AreEqual(result.Count(),3);
    }

    [TestCategory("Queries")]
    [TestMethod]
    public void Dado_a_consulta_de_produtos_inativos_deve_retornar_2()
    {
        var result = _products.AsQueryable().Where(ProductQueries.GetInactiveProducts());
        Assert.AreEqual(result.Count(),2);
    }
}