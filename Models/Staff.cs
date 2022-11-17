using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Medicaly.Models
{
    public class Staff
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El id es obligatorio")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string LastName { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El documento es obligatorio")]
        public int Dni { get; set; }

        [Display(Name = "Telefono")]
        public string CellNumber { get; set; }

        [Display(Name = "Calle")]
        public string Street { get; set; }
        [Display(Name = "Numero")]
        public int Number { get; set; }
        [Display(Name = "Piso")]
        public int Floor { get; set; }
        [Display(Name = "Departamento")]
        public string Department { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime Birthday { get; set; }
        public int SpecialtyID { get; set; }

        [Display(Name = "Especialidad")]
        public Specialty Specialty { get; set; }

        [Display(Name = "Antiguedad")]
        [Required(ErrorMessage = "La antiguedad es obligatorio")]
        public DateTime Antiquity { get; set; }
        [Display(Name = "Foto")]
        public string Photo { get; set; }

    }
}
