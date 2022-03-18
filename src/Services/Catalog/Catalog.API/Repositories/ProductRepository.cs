using Catalog.API.Data;
using Catalog.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await catalogContext.Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<Product> Create(Product product)
        {
            await catalogContext.Products.InsertOneAsync(product);
            return product;
        }

        public async Task DeleteById(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            await catalogContext.Products
                .DeleteOneAsync(filter);                
        }

        

        public async Task<IEnumerable<Product>> GetByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await catalogContext.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await catalogContext.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await catalogContext.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Product> Update(Product product)
        {
            await catalogContext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return product;
        }
    }
}
