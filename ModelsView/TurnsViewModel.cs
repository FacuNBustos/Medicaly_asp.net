using Medicaly.Models;
using System.Collections.Generic;

namespace Medicaly.ModelsView
{
    public class TurnsViewModel
    {
        public List<Turn> ListTurns { get; set; }
        public string searchDni { get; set; }
        public paginador paginador { get; set; }
    }
}
