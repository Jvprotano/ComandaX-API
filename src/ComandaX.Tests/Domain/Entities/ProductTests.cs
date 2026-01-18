using ComandaX.Domain.Entities;
using ComandaX.Tests.Helpers.Builders;
using Xunit;

namespace ComandaX.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_WithNameAndPrice_CreatesProduct()
    {
        // Arrange & Act
        const decimal PRICE = 10.99m;
        const string NAME = "Test Product";

        var product = ProductBuilder.New()
        .WithName(NAME)
        .WithPrice(PRICE).Build();

        // Assert
        Assert.Equal(NAME, product.Name);
        Assert.Equal(PRICE, product.Price);
    }

    [Fact]
    public void Constructor_WithAllParameters_CreatesProduct()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        const decimal PRICE = 10.99m;
        const bool NEED_PREPARATION = true;
        const string NAME = "Test Product";

        // Act
        var product = ProductBuilder
        .New()
        .WithName(NAME)
        .WithPrice(PRICE)
        .WithNeedPreparation(NEED_PREPARATION)
        .WithProductCategoryId(categoryId)
        .WithPricePerKg(true)
        .Build();

        // Assert
        Assert.Equal(NAME, product.Name);
        Assert.Equal(PRICE, product.Price);
        Assert.Equal(NEED_PREPARATION, product.NeedPreparation);
        Assert.Equal(categoryId, product.ProductCategoryId);
        Assert.True(product.IsPricePerKg);
    }

    [Fact]
    public void SetName_WithValidName_UpdatesName()
    {
        // Arrange
        const string NEW_NAME = "New Name";
        var product = ProductBuilder.New().Build();

        // Act
        product.SetName(NEW_NAME);

        // Assert
        Assert.Equal(NEW_NAME, product.Name);
    }

    [Fact]
    public void SetName_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var product = ProductBuilder.New().Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetName(""));
    }

    [Fact]
    public void SetName_WithWhitespaceName_ThrowsArgumentException()
    {
        // Arrange
        var product = ProductBuilder.New().Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetName("   "));
    }

    [Fact]
    public void SetPrice_WithValidPrice_UpdatesPrice()
    {
        // Arrange
        var product = ProductBuilder.New().Build();

        // Act
        product.SetPrice(15.99m);

        // Assert
        Assert.Equal(15.99m, product.Price);
    }

    [Fact]
    public void SetPrice_WithZeroPrice_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m, false);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetPrice(0m));
    }

    [Fact]
    public void SetPrice_WithNegativePrice_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m, false);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetPrice(-5m));
    }

    [Fact]
    public void SetNeedPreparation_WithTrue_UpdatesNeedPreparation()
    {
        // Arrange
        var product = ProductBuilder.New().WithNeedPreparation(false).Build();

        Assert.False(product.NeedPreparation);

        // Act
        product.SetNeedPreparation(true);

        // Assert
        Assert.True(product.NeedPreparation);
    }

    [Fact]
    public void SetNeedPreparation_WithFalse_UpdatesNeedPreparation()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m, false);
        product.SetNeedPreparation(true);
        Assert.True(product.NeedPreparation);

        // Act
        product.SetNeedPreparation(false);

        // Assert
        Assert.False(product.NeedPreparation);
    }

    [Fact]
    public void SetProductCategory_WithValidCategoryId_UpdatesCategory()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m, false);
        var categoryId = Guid.NewGuid();

        // Act
        product.SetProductCategory(categoryId);

        // Assert
        Assert.Equal(categoryId, product.ProductCategoryId);
    }

    [Fact]
    public void SetProductCategory_WithNull_RemovesCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var product = new Product("Test Product", 10.99m, false, categoryId);
        Assert.Equal(categoryId, product.ProductCategoryId);

        // Act
        product.SetProductCategory(null);

        // Assert
        Assert.Null(product.ProductCategoryId);
    }
}

