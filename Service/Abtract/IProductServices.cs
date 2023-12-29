using Domain.BaseEntity;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abtract
{
    public interface IProductServices
    {
        Task<int> CountAllProduct();
        Task<ResultData<List<int>>> CreateListProduct(IEnumerable<Product> products);
        Task<ResultData<int>> CreateProduct(Product product);
        Task<ResultData<int>> DeleteProduct(int id);
        Task<ResultData<int>> DeleteWithConditionProduct();
        Task<IEnumerable<Product>> GetProduct(int pageNumber, int pageSize = 10);
        Task<Product> GetProductById(int id);
        Task<ResultData<int>> UpdateAllFieldProduct(Product products);
        Task<ResultData<int>> UpdatePriceProduct(int id, double sellingPrice, double purchasePrice);
    }
}
