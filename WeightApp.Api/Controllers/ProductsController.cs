using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WeightApp.Api.Models;
using WeightApp.Db;

namespace WeightApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private IRepository _repository;

        public ProductsController(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts()
        {
            try
            {
                var result = await _repository.GetProducts(User.Claims.GetUserId());
                var response = result.Select(
                    p => new ProductModel
                    {
                        Id = p.ProductId,
                        Name = p.Name,
                        UserId = p.UserId,
                        ProductCategory = new ProductCategoryModel { Id = p.ProductCategory.CategoryId, Name = p.ProductCategory.Name },
                        Calories = p.Calories,
                        Carbohydrates = p.Carbohydrates,
                        Proteins = p.Proteins,
                        Fats = p.Fats
                    });

                return response.ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductModel>> AddProduct([FromBody] AddProductRequest product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                var result = await _repository.AddProduct(
                    new ProductEntity
                    {
                        Name = product.Name,
                        UserId = User.Claims.GetUserId(),
                        ProductCategory = new ProductCategoryEntity { CategoryId = product.CategoryId },
                        Calories = product.Calories,
                        Carbohydrates = product.Carbohydrates,
                        Fats = product.Fats,
                        Proteins = product.Proteins
                    });
                var response = new ProductModel
                {
                    Id = result.ProductId,
                    Name = result.Name,
                    UserId = result.UserId,
                    ProductCategory = new ProductCategoryModel { Id = result.ProductCategory.CategoryId, Name = result.ProductCategory.Name },
                    Calories = result.Calories,
                    Carbohydrates = result.Carbohydrates,
                    Proteins = result.Proteins,
                    Fats = result.Fats
                };

                return response;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveProduct([FromRoute] int productId)
        {
            try
            {
                await _repository.RemoveProduct(productId, User.Claims.GetUserId());
                return Ok();
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}