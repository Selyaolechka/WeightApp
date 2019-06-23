using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WeightApp.Api.Models;
using WeightApp.Db;

namespace WeightApp.Api.Controllers
{
    [ApiController]
    [Route("api/ref")]
    public class ReferenceDataController : ControllerBase
    {
        private IRepository _repository;

        public ReferenceDataController(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        [HttpGet("mealTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MealTypeModel>>> GetMealTypes()
        {
            try
            {
                var result = await _repository.GetMealTypes();
                var response = result.Select(p => new MealTypeModel { Id = p.MealTypeId, Name = p.Name });
                return response.ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetProductCategories()
        {
            try
            {
                var result = await _repository.GetAllCategories();
                var response = result.Select(p => new ProductCategoryModel { Id = p.CategoryId, Name = p.Name });
                return response.ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}