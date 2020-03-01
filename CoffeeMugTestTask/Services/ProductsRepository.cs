using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CoffeeMugTestTask.API.DAL;
using CoffeeMugTestTask.API.Models;

namespace CoffeeMugTestTask.API.Services
{
    public class ProductsRepository : IProductsRepository, IDisposable
    {
        private ProductsContext _dbContext;

        public ProductsRepository(ProductsContext context, ILogger<ProductsRepository> logger)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(productId));
        }

        public void CreateProduct(Product productToAdd)
        {
            if (productToAdd == null)
            {
                throw new ArgumentNullException(nameof(productToAdd));
            }

            _dbContext.Add(productToAdd);
        }

        public void UpdateProduct(Guid productId, Product changedProduct)
        {
            if (changedProduct == null)
            {
                throw new ArgumentNullException(nameof(changedProduct));
            }

            _dbContext.Entry(changedProduct).State = EntityState.Modified;
        }

        public void RemoveProduct(Product productToRemove)
        {
            if (productToRemove == null)
            {
                throw new ArgumentNullException(nameof(productToRemove));
            }

            _dbContext.Products.Remove(productToRemove);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
    }
}
