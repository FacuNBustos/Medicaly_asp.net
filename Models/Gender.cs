using System.ComponentModel.DataAnnotations;

namespace Medicaly.Models
{
    public class Gender
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El id es obligatorio")]
        public int ID { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Description { get; set; }
    }
}
