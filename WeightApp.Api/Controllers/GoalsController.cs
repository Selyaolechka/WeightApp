using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WeightApp.Api.Models;
using WeightApp.Api.Services;
using WeightApp.Db;

namespace WeightApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/goals")]
    public class GoalsController : ControllerBase
    {
        private IRepository _repository;
        private IWeightCalculator _calculator;

        public GoalsController(IRepository repository, IWeightCalculator calculator)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (calculator == null)
                throw new ArgumentNullException(nameof(calculator));

            _repository = repository;
            _calculator = calculator;
        }

        [HttpGet("{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GoalModel>> GetGoal([FromRoute] int goalId)
        {
            try
            {
                var result = await _repository.GetGoals(User.Claims.GetUserId(), goalId);
                var goal = result.FirstOrDefault();

                if (goal == null)
                    return NotFound();

                var response = new GoalModel
                {
                    GoalId = goal.GoalId,
                    Weight = goal.Weight,
                    WeightGoal = goal.WeightGoal,
                    Height = goal.Height,
                    StartDate = goal.StartDate,
                    IsCompleted = goal.IsCompleted,
                    IsSuccess = goal.IsSuccess
                };

                return response;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GoalModel>>> GetGoals()
        {
            try
            {
                var result = await _repository.GetGoals(User.Claims.GetUserId(), null);
                var response = result.Select(
                    p => new GoalModel
                    {
                        GoalId = p.GoalId,
                        Weight = p.Weight,
                        WeightGoal = p.WeightGoal,
                        Height = p.Height,
                        StartDate = p.StartDate,
                        IsCompleted = p.IsCompleted,
                        IsSuccess = p.IsSuccess
                    });

                return response.ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GoalModel>> SetNewGoal([FromBody] SetNewGoalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                var result = await _repository.SetNewGoal(
                    new GoalEntity
                    {
                        UserId = User.Claims.GetUserId(),
                        Weight = request.Weight,
                        WeightGoal = request.WeightGoal,
                        StartDate = request.StartDate,
                        Height = request.Height
                    });
                var response = new GoalModel
                {
                    GoalId = result.GoalId,
                    Weight = result.Weight,
                    WeightGoal = result.WeightGoal,
                    Height = result.Height,
                    StartDate = result.StartDate,
                    IsCompleted = result.IsCompleted,
                };

                return CreatedAtAction(nameof(GetGoal), new { goalId = response.GoalId }, response);
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

        [HttpPut("{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GoalModel>> CompleteGoal(int goalId)
        {
            try
            {
                var result = await _repository.CompleteGoal(goalId, User.Claims.GetUserId(), DateTime.Now);

                if (result == null)
                    return NotFound();

                var response = new GoalModel
                {
                    GoalId = result.GoalId,
                    Weight = result.Weight,
                    WeightGoal = result.WeightGoal,
                    Height = result.Height,
                    StartDate = result.StartDate,
                    IsCompleted = result.IsCompleted,
                    IsSuccess = result.IsSuccess
                };

                return response;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{goalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteGoal([FromRoute] int goalId)
        {
            try
            {
                await _repository.DeleteGoal(goalId, User.Claims.GetUserId());
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProgressModel>>> GetProgress()
        {
            try
            {
                var result = await _repository.GetProgress(User.Claims.GetUserId());
                var response = result.Select(
                    p => new ProgressModel
                    {
                        ProgressId = p.ProgressId,
                        Weight = p.Weight,
                        GoalId = p.GoalId,
                        Date = p.Date
                    });

                return response.ToList();
            }
            catch (DbException ex)
            {
                if (ex.Code == 50100)
                    return Forbid();

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProgressModel>> TrackProgress(TrackProgressRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                var result = await _repository.TrackProgress(
                    User.Claims.GetUserId(),
                    new GoalProgressEntity
                    {
                        GoalId = request.GoalId,
                        Weight = request.Weight,
                        Date = request.Date
                    });
                var response = new ProgressModel
                {
                    ProgressId = result.ProgressId,
                    Weight = result.Weight,
                    GoalId = result.GoalId,
                    Date = result.Date
                };

                return response;
            }
            catch (DbException ex)
            {
                if (ex.Code == 50100)
                    return Forbid();

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("progress/{progressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveProgress([FromRoute] int progressId)
        {
            try
            {
                await _repository.RemoveProgress(progressId, User.Claims.GetUserId());
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{goalId}/meals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DailyMealInstanceModel>>> GetMealsForGoal(int goalId)
        {
            try
            {
                var result = (await _repository.GetMealsForGoal(goalId, User.Claims.GetUserId())).ToList();
                var meals = result.Select(
                    p => new MealInstanceModel
                    {
                        MealInstanceId = p.MealInstanceId,
                        GoalId = p.Goal.GoalId,
                        Amount = p.Amount,
                        Date = p.Date,
                        Product = new MealProductModel { ProductId = p.Product.ProductId, Name = p.Product.Name },
                        MealType = new MealTypeModel { Id = p.MealType.MealTypeId, Name = p.MealType.Name }
                    });

                var goal = result.First().Goal;
                var norm = _calculator.CalculateDailyCaloriesNorm(
                    goal.WeightGoal,
                    goal.Height,
                    User.Claims.GetDateOfBirth(),
                    goal.StartDate,
                    User.Claims.GetIsMale()
                    );

                var totals = (await _repository.GetMealTotalsForGoal(User.Claims.GetUserId(), goalId)).ToList();

                if (!totals.Any())
                {
                    return new[]
                    {
                        new DailyMealInstanceModel
                        {
                            DailyNorm = norm,
                            Consumed = 0.0,
                            DateTime = DateTime.Now,
                            Meals = Enumerable.Empty<MealInstanceModel>()
                        }
                    };
                }

                var response = totals.Select(
                    p =>
                    {
                        return new DailyMealInstanceModel
                        {
                            Consumed = p.Consumed,
                            DailyNorm = norm,
                            DateTime = p.Date,
                            Meals = meals.Where(pp => (pp.Date - p.Date).Days == 0)
                        };
                    });

                return response.ToList();
            }
            catch (DbException ex)
            {
                if (ex.Code == 50100)
                    return Forbid();

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("meals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DailyMealInstanceModel>> GetMealsForDate(DateTime? date)
        {
            try
            {
                var result = (await _repository.GetMealsForDate(User.Claims.GetUserId(), date ?? DateTime.Now)).ToList();
                var meals = result.Select(
                    p => new MealInstanceModel
                    {
                        MealInstanceId = p.MealInstanceId,
                        GoalId = p.Goal.GoalId,
                        Amount = p.Amount,
                        Date = p.Date,
                        Product = new MealProductModel { ProductId = p.Product.ProductId, Name = p.Product.Name },
                        MealType = new MealTypeModel { Id = p.MealType.MealTypeId, Name = p.MealType.Name }
                    });

                var goal = result.First().Goal;
                var norm = _calculator.CalculateDailyCaloriesNorm(
                    goal.WeightGoal,
                    goal.Height,
                    User.Claims.GetDateOfBirth(),
                    goal.StartDate,
                    User.Claims.GetIsMale()
                    );

                var response = new DailyMealInstanceModel
                {
                    DateTime = (date ?? DateTime.Now),
                    Consumed = await _repository.GetMealTotalsForDate(User.Claims.GetUserId(), date ?? DateTime.Now),
                    DailyNorm = norm,
                    Meals = meals
                };

                return response;
            }
            catch (DbException ex)
            {
                if (ex.Code == 50100)
                    return Forbid();

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("meals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MealInstanceModel>> AddMeal([FromBody] AddMealRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                var result = await _repository.AddMeal(
                    User.Claims.GetUserId(),
                    new MealInstanceEntity
                    {
                        Goal = new GoalEntity { GoalId = request.GoalId },
                        Product = new ProductEntityRef { ProductId = request.ProductId },
                        MealType = new MealTypeEntity { MealTypeId = request.MealTypeId },
                        Date = request.Date,
                        Amount = request.Amount
                    });

                var response = new MealInstanceModel
                {
                    MealInstanceId = result.MealInstanceId,
                    GoalId = result.Goal.GoalId,
                    Amount = result.Amount,
                    Date = result.Date,
                    Product = new MealProductModel { ProductId = result.Product.ProductId, Name = result.Product.Name },
                    MealType = new MealTypeModel { Id = result.MealType.MealTypeId, Name = result.MealType.Name }
                };

                return response;
            }
            catch (DbException ex)
            {
                if (ex.Code == 50100)
                    return Forbid();

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("meals/{mealInstanceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveMeal(int mealInstanceId)
        {
            try
            {
                await _repository.RemoveMeal(mealInstanceId, User.Claims.GetUserId());
                return Ok();
            }
            catch (DbException ex)
            {
                if (ex.Code == 50100)
                    return Forbid();

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}