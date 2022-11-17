using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Medicaly.Models
{
    public class Specialty
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El id es obligatorio")]
        public int ID { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; }
    }
}
