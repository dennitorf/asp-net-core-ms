using Catalog.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetByName(string name);
        Task<IEnumerable<Product>> GetByCategory(string categoryName);
        Task<Product> GetById(string id);        
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task DeleteById(string id);        
    }
}
