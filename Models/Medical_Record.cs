using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Medicaly.Models
{
    public class Medical_Record
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "El id es obligatorio")]
        public int ID { get; set; }
        public int StaffID { get; set; }
        [Display(Name = "Doctor/a")]
        public Staff Staff { get; set; }
        public int PatientID { get; set; }
        [Display(Name = "Paciente")]
        public Patient Patient { get; set; }
        [Display(Name = "Fecha")]
        public DateTime CreateAt { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }
}
