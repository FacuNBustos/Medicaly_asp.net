using Medicaly.Models;
using System.Collections.Generic;

namespace Medicaly.ModelsView
{
    public class MedicalRecordsViewModel
    {
        public List<Medical_Record> ListMedicalRecords { get; set; }
        public string searchDni { get; set; }
        public paginador paginador { get; set; }
    }
}
