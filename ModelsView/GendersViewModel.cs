using Medicaly.Models;
using System.Collections.Generic;

namespace Medicaly.ModelsView
{
    public class GendersViewModel
    {
        public List<Gender> ListGenders { get; set; }
        public paginador paginador { get; set; }
    }
}
