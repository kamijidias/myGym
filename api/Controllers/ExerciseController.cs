using api.Dtos;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseService _exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Exercise>> Create([FromBody] Exercise exercise)
        {
            if (exercise == null) return BadRequest("Invalid data.");

            try
            {
                var createdExercise = await _exerciseService.CreateExercise(
                    exercise.Name,
                    exercise.MuscleGroup
                );
                return Ok(createdExercise);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}