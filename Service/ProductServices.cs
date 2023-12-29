using Data;
using Data.Abtract;
using Domain.BaseEntity;
using Domain.Entity;
using Microsoft.IdentityModel.Tokens;
using Service.Abtract;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{

    public class ProductServices : IProductServices
    {
        IRepository<Product> _productRepo;
        IDapperHelper _dapperHelper;

        public ProductServices(IRepository<Product> productRepo, IDapperHelper dapperHelper)
        {
            _productRepo = productRepo;
            _dapperHelper = dapperHelper;
        }

        public async Task<IEnumerable<Product>> GetProduct(int pageNumber, int pageSize = 10)
        {
            /*string sql = $"GetProduct";
            var data = await _dapperHelper.ExecuteProcGetList<Product>(sql);*/
            string sql = $"Select * from products";
            var data = await _dapperHelper.ExecuteSqlGetList<Product>(sql);
            return QueryHelper.Standard<Product>(data, pageNumber, pageSize);
        }
        public async Task<Product> GetProductById(int id)
        {
            try
            {
                string sql = $"SELECT * FROM PRODUCTS WHERE id = {id}";
                return await _dapperHelper.ExecuteReturnFirst<Product>(sql);
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public async Task<int> CountAllProduct()
        {
            string sql = $"SELECT COUNT(*) FROM PRODUCTS";
            return await _dapperHelper.ExecuteReturnScalar<int>(sql);
        }
        public async Task<ResultData<int>> CreateProduct(Product product)
        {
            if (product.Id != 0)
                return new ResultData<int>().FalseResult("Id must be equal 0");
            await _productRepo.Insert(product);
            await _productRepo.Commit();
            return new ResultData<int>().SuccessResult(product.Id);
        }
        public async Task<ResultData<List<int>>> CreateListProduct(IEnumerable<Product> products)
        {
            if (products.IsNullOrEmpty())
                return new ResultData<List<int>>().FalseResult("It must not be null or empty");

            if(!products.Select(p => p.Id).Contains(0))
                return new ResultData<List<int>>().FalseResult("Id must be equal 0");

            await _productRepo.Insert(products);
            await _productRepo.Commit();
            return new ResultData<List<int>>().SuccessResult(products.Select(p => p.Id).ToList());
        }
        public async Task<ResultData<int>> UpdateAllFieldProduct(Product products)
        {
            try
            {
                _productRepo.Update(products);
                await _productRepo.Commit();
                return new ResultData<int>().SuccessResult();
            }
            catch (Exception ex)
            {
                return new ResultData<int>().FalseResult(ex.InnerException.Message);
            }
        }
        public async Task<ResultData<int>> UpdatePriceProduct(int id, double sellingPrice, double purchasePrice)
        {
            string sql = $"Update products " +
                                $"set SellingPrice = {sellingPrice}, PurchasePrice = {purchasePrice} " +
                                $"where id = {id}";
            await _dapperHelper.ExecuteNotReturn(sql);
            return new ResultData<int>().SuccessResult();
        }
        public async Task<ResultData<int>> DeleteProduct(int id)
        {
            Product product = await GetProductById(id);
            _productRepo.Delete(product); // delete bằng cách query dapper
            await _productRepo.Commit();
            return new ResultData<int>().SuccessResult();
        }
        public async Task<ResultData<int>> DeleteWithConditionProduct()
        {
            _productRepo.Delete(p => p.Id > 1000); 
            // delete bằng cách query EF và LinQ nên sẽ lâu hơn dapper
            // nhưng delete thì không cần query quá nhiều nên có thể sử dụng EF
            // nếu xóa nhiều data 1 cách quá nhanh cũng cần phải xem xét
            await _productRepo.Commit(); 
            return new ResultData<int>().SuccessResult();
        }
    }
}
