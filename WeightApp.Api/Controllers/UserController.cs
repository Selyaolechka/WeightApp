using System;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private IRepository _repository;

        public UserController(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RegisterUserResponse> LoginUser()
        {
            try
            {
                return new RegisterUserResponse
                {
                    Email = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value,
                    Name = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value,
                    DateOfBirth = User.Claims.GetDateOfBirth(),
                    IsMale = User.Claims.GetIsMale()
                };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserRequest user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                var result = await _repository.RegisterUser(
                    new UserEntity
                    {
                        Email = user.Email, 
                        Password = user.Password, 
                        Name = user.Name, 
                        IsMale = user.IsMale, 
                        DateOfBirth = user.DateOfBirth
                    });

                if (result != null)
                {
                    var response = new RegisterUserResponse
                    {
                        Email = user.Email,
                        Name = user.Name,
                        DateOfBirth = user.DateOfBirth,
                        IsMale = user.IsMale
                    };

                    return response;
                }
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return StatusCode(500);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUser()
        {
            try
            {
                await _repository.RemoveUser(User.Claims.GetUserId());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateUserResponse>> UpdateUserDetails([FromBody] UpdateUserRequest updateUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            try
            {
                var result = await _repository.UpdateDetails(
                    new UserEntity
                    {
                        UserId = User.Claims.GetUserId(),
                        Name = updateUser.Name,
                        Password = updateUser.Password,
                        IsMale = updateUser.IsMale,
                        DateOfBirth = updateUser.DateOfBirth
                    });

                var response = new UpdateUserResponse
                {
                    Email = result.Email,
                    Name = result.Name,
                    Password = result.Password,
                    IsMale = result.IsMale,
                    DateOfBirth = result.DateOfBirth
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}