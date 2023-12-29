using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abtract;

namespace Api.Controllers
{
    /*       EXAMPLE SERVICE           */
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber, int pageSize = 10)
        {
            return Ok(await _productServices.GetProduct(pageNumber, pageSize));
        }

        [HttpGet("{id:int}/detail")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productServices.GetProductById(id));
        }
        [HttpGet("Count")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _productServices.CountAllProduct());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]Product product)
        {
            var rs = await _productServices.CreateProduct(product);
            if(rs.Success)
                return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost("CreateList")]
        public async Task<IActionResult> CreateList([FromBody] IEnumerable<Product> products)
        {
            var rs = await _productServices.CreateListProduct(products);
            if (rs.Success)
                return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAllField([FromBody] Product products)
        {
            var rs = await _productServices.UpdateAllFieldProduct(products);
            if (rs.Success)
                return Ok();
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePrice(int id, double sellingPrice, double purchasePrice)
        {
            var rs = await _productServices.UpdatePriceProduct(id, sellingPrice, purchasePrice);
            if (rs.Success)
                return Ok();
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var rs = await _productServices.DeleteProduct(id);
            if (rs.Success)
                return Ok();
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("Condition")]
        public async Task<IActionResult> ConditionDelete()
        {
            var rs = await _productServices.DeleteWithConditionProduct();
            if (rs.Success)
                return Ok();
            return BadRequest(rs.ErrorMessage);
        }
    }
}
