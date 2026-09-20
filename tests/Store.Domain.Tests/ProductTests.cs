using Store.Domain.Entities;

namespace Store.Domain.Tests;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var id = Guid.NewGuid();
        var product = new Product(id, "Notebook", 3500m);

        Assert.Equal(id, product.Id);
        Assert.Equal("Notebook", product.Name);
        Assert.Equal(3500m, product.Price);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyId()
    {
        Assert.Throws<ArgumentException>(() => new Product(Guid.Empty, "Mouse", 100m));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldRejectBlankName(string? name)
    {
        Assert.Throws<ArgumentException>(() => new Product(Guid.NewGuid(), name!, 100m));
    }

    [Fact]
    public void Constructor_ShouldRejectNegativePrice()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Product(Guid.NewGuid(), "Mouse", -1m));
    }

    [Fact]
    public void Constructor_ShouldAllowZeroPrice()
    {
        var product = new Product(Guid.NewGuid(), "Brinde", 0m);

        Assert.Equal(0m, product.Price);
    }
}
