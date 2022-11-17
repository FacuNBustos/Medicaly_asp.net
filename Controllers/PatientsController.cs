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
using System.Collections;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Medicaly.ModelsView;

namespace Medicaly.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public PatientsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string searchLastName, string searchDni, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var applicationDbContext = _context.Patient.Include(p => p.Gender).Select(a => a);
            if (!string.IsNullOrEmpty(searchLastName))
            {
                applicationDbContext = applicationDbContext.Where(a => a.LastName.Contains(searchLastName));
            };
            if (!string.IsNullOrEmpty(searchDni))
            {
                applicationDbContext = applicationDbContext.Where(a => a.Dni.StartsWith(searchDni));
            };

            paginador.cantReg = applicationDbContext.Count();

            var datosAmostrar = applicationDbContext
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            PatientsViewModel Datos = new PatientsViewModel()
            {
                ListPatients = datosAmostrar.ToList(),
                searchLastName = searchLastName,
                searchDni = searchDni,
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Description");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,GenderID,Dni,Birthday,Street,Number,Floor,Department,CellNumber,Photo")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                var archive = HttpContext.Request.Form.Files;
                if (archive != null && archive.Count > 0)
                {
                    var archivePhoto = archive[0];
                    var pathDestiny = Path.Combine(env.WebRootPath, "assets/patients");
                    if (archivePhoto.Length > 0)
                    {
                        var archiveDestiny = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivePhoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestiny, archiveDestiny), FileMode.Create))
                        {
                            archivePhoto.CopyTo(filestream);
                            patient.Photo = archiveDestiny;
                        };
                    }
                };
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Description", patient.GenderID);
            return View(patient);
        }

        // GET: Patients/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Description", patient.GenderID);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,GenderID,Dni,Birthday,Street,Number,Floor,Department,CellNumber,Photo")] Patient patient)
        {
            if (id != patient.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var archive = HttpContext.Request.Form.Files;
                    if (archive != null && archive.Count > 0)
                    {
                        var archivePhoto = archive[0];
                        var pathDestiny = Path.Combine(env.WebRootPath, "assets/staffs");
                        if (archivePhoto.Length > 0)
                        {
                            var archiveDestiny = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivePhoto.FileName);

                            if (!string.IsNullOrEmpty(patient.Photo))
                            {
                                string previousPhoto = Path.Combine(pathDestiny, patient.Photo);
                                if (System.IO.File.Exists(previousPhoto))
                                    System.IO.File.Delete(previousPhoto);
                            }

                            using (var filestream = new FileStream(Path.Combine(pathDestiny, archiveDestiny), FileMode.Create))
                            {
                                archivePhoto.CopyTo(filestream);
                                patient.Photo = archiveDestiny;
                            }
                        }
                    }
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.ID))
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
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Description", patient.GenderID);
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.ID == id);
        }
    }
}
