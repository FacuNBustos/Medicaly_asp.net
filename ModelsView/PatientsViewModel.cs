using Medicaly.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Medicaly.ModelsView
{
    public class PatientsViewModel
    {
        public List<Patient> ListPatients { get; set; }
        public string searchLastName { get; set; }
        public string searchDni { get; set; }
        public paginador paginador { get; set; }
    }

    public class paginador
    {
        public int cantReg { get; set; }
        public int regXpag { get; set; }
        public int pagActual { get; set; }
        public int totalPag => (int)Math.Ceiling((decimal)cantReg / regXpag);
        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
    }
}
