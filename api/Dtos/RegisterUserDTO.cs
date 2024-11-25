using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class RegisterUserDTO
    {
    public required string Email { get; set; }
    public required string FullName { get; set; }
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    public required string UserName { get; set; }
}
}