using System;
using System.ComponentModel.DataAnnotations;

namespace Medicaly.Models
{
    public class Patient
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

        public int GenderID { get; set; }
        [Display(Name = "Genero")]
        public Gender Gender { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El documento es obligatorio")]
        public string Dni { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Calle")]
        public string Street { get; set; }

        [Display(Name = "Numero")]
        public int Number { get; set; }

        [Display(Name = "Piso")]
        public int Floor { get; set; }

        [Display(Name = "Departamento")]
        public string Department { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El numero de celular es obligatorio")]
        public string CellNumber { get; set; }

        [Display(Name = "Foto")]
        public string Photo { get; set; }

    }
}
