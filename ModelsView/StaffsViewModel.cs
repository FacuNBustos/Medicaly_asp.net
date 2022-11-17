using Medicaly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Medicaly.ModelsView
{
    public class StaffsViewModel
    {
        public List<Staff> ListStaffs { get; set; }
        public SelectList ListSpecialties { get; set; }
        public string searchLastName { get; set; }
        public paginador paginador { get; set; }
    }
}
