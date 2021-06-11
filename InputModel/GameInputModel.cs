using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game's name must have at least 3 and at maximum 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game's producer must have at least 3 and at maximum 100 characters")]
        public string Producer { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "The game's price must be at least 3 and at maximum 1000 money")]
        public double Price { get; set; }
    }
}