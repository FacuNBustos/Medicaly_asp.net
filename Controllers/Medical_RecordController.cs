using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medicaly.Data;
using Medicaly.Models;
using Microsoft.AspNetCore.Authorization;
using Medicaly.ModelsView;

namespace Medicaly.Controllers
{
    public class Medical_RecordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Medical_RecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medical_Record
        public async Task<IActionResult> Index(string searchDni, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var applicationDbContext = _context.Medical_Record.Include(m => m.Patient).Include(m => m.Staff).Select(a => a);
            if (!string.IsNullOrEmpty(searchDni))
            {
                applicationDbContext = applicationDbContext.Where(a => a.Patient.Dni.StartsWith(searchDni));
            }

            paginador.cantReg = applicationDbContext.Count();

            var datosAmostrar = applicationDbContext
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            MedicalRecordsViewModel Datos = new MedicalRecordsViewModel()
            {
                ListMedicalRecords = datosAmostrar.ToList(),
                searchDni = searchDni,
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: Medical_Record/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medical_Record = await _context.Medical_Record
                .Include(m => m.Patient)
                .Include(m => m.Staff)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medical_Record == null)
            {
                return NotFound();
            }

            return View(medical_Record);
        }

        // GET: Medical_Record/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber");
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName");
            return View();
        }

        // POST: Medical_Record/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StaffID,PatientID,CreateAt,Description")] Medical_Record medical_Record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medical_Record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber", medical_Record.PatientID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName", medical_Record.StaffID);
            return View(medical_Record);
        }

        // GET: Medical_Record/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medical_Record = await _context.Medical_Record.FindAsync(id);
            if (medical_Record == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber", medical_Record.PatientID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName", medical_Record.StaffID);
            return View(medical_Record);
        }

        // POST: Medical_Record/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StaffID,PatientID,CreateAt,Description")] Medical_Record medical_Record)
        {
            if (id != medical_Record.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medical_Record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Medical_RecordExists(medical_Record.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber", medical_Record.PatientID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName", medical_Record.StaffID);
            return View(medical_Record);
        }

        // GET: Medical_Record/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medical_Record = await _context.Medical_Record
                .Include(m => m.Patient)
                .Include(m => m.Staff)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medical_Record == null)
            {
                return NotFound();
            }

            return View(medical_Record);
        }

        // POST: Medical_Record/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medical_Record = await _context.Medical_Record.FindAsync(id);
            _context.Medical_Record.Remove(medical_Record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Medical_RecordExists(int id)
        {
            return _context.Medical_Record.Any(e => e.ID == id);
        }
    }
}
