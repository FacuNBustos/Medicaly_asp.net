using Medicaly.Models;
using System.Collections.Generic;

namespace Medicaly.ModelsView
{
    public class SpecialtiesViewModel
    {
        public List<Specialty> ListSpecialties { get; set; }
        public paginador paginador { get; set; }
    }
}
