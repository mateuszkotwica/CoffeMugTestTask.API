using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeMugTestTask.API.Models;

namespace CoffeeMugTestTask.API.Services
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(Guid productId);

        void CreateProduct(Product productToAdd);

        void UpdateProduct(Guid productId, Product changedProduct);

        void RemoveProduct(Product productToRemove);

        Task<bool> SaveChangesAsync();
    }
}
