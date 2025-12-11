using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProducts();
    Task<List<Product>> GetAvailableProducts();
    Task<ProductResponse> GetProductById(int id);
    Task<Product> CreateProduct(CreateProductDto productData);
    Task<Product> UpdateProduct(int id, UpdateProductDto updatedProduct);
    Task<bool> DeleteProduct(int id);
    ProductResponse CreateProductResponse(Product product);
}