using ComandaX.Domain.Entities;
using Xunit;

namespace ComandaX.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_WithNameAndPrice_CreatesProduct()
    {
        // Arrange & Act
        var product = new Product("Test Product", 10.99m);

        // Assert
        Assert.Equal("Test Product", product.Name);
        Assert.Equal(10.99m, product.Price);
        Assert.False(product.NeedPreparation);
        Assert.Null(product.ProductCategoryId);
    }

    [Fact]
    public void Constructor_WithAllParameters_CreatesProduct()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        // Act
        var product = new Product("Test Product", 10.99m, categoryId);

        // Assert
        Assert.Equal("Test Product", product.Name);
        Assert.Equal(10.99m, product.Price);
        Assert.Equal(categoryId, product.ProductCategoryId);
    }

    [Fact]
    public void SetName_WithValidName_UpdatesName()
    {
        // Arrange
        var product = new Product("Original Name", 10.99m);

        // Act
        product.SetName("New Name");

        // Assert
        Assert.Equal("New Name", product.Name);
    }

    [Fact]
    public void SetName_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetName(""));
    }

    [Fact]
    public void SetName_WithWhitespaceName_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetName("   "));
    }

    [Fact]
    public void SetPrice_WithValidPrice_UpdatesPrice()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m);

        // Act
        product.SetPrice(15.99m);

        // Assert
        Assert.Equal(15.99m, product.Price);
    }

    [Fact]
    public void SetPrice_WithZeroPrice_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetPrice(0m));
    }

    [Fact]
    public void SetPrice_WithNegativePrice_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.SetPrice(-5m));
    }

    [Fact]
    public void SetNeedPreparation_WithTrue_UpdatesNeedPreparation()
    {
        // Arrange
        var product = new Product("Test Product", 10.99m);
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
        var product = new Product("Test Product", 10.99m);
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
        var product = new Product("Test Product", 10.99m);
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
        var product = new Product("Test Product", 10.99m, categoryId);
        Assert.Equal(categoryId, product.ProductCategoryId);

        // Act
        product.SetProductCategory(null);

        // Assert
        Assert.Null(product.ProductCategoryId);
    }
}

