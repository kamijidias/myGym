using api.DataBase;
using api.Models;

namespace api.Services
{
    public class ExerciseService
    {
        private readonly AppDbContext _context;

        public ExerciseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise> CreateExercise(string name, string muscleGroup)
        {
            var exercise = new Exercise
            {
                Name = name,
                MuscleGroup = muscleGroup
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return exercise;
        }
    }
}