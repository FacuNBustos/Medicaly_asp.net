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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Medicaly.Controllers
{
    public class SpecialtiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public SpecialtiesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        public IActionResult Import()
        {
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivoImpo = archivos[0];
                var pathDestino = Path.Combine(env.WebRootPath, "imports");
                if (archivoImpo.Length > 0)
                {
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoImpo.FileName);
                    string rutaCompleta = Path.Combine(pathDestino, archivoDestino);
                    using (var filestream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        archivoImpo.CopyTo(filestream);
                    };

                    using (var file = new FileStream(rutaCompleta, FileMode.Open))
                    {
                        List<string> renglonesArchivo = new List<string>();
                        List<Specialty> SpecialidadesArchivos = new List<Specialty>();

                        StreamReader fileContent = new StreamReader(file);
                        do
                        {
                            renglonesArchivo.Add(fileContent.ReadLine());
                        }
                        while (!fileContent.EndOfStream);

                        foreach (string renglon in renglonesArchivo)
                        {
                            int salida;

                            string[] datos = renglon.Split(';');
                            int tramiteId = int.TryParse(datos[0], out salida) ? salida : 0;
                            var resultado = _context.Specialty.Where(t => t.Description.Contains(datos[1])).FirstOrDefault();

                            if (tramiteId > 0 && resultado == null)
                            {
                                Specialty tramiteTemporal = new Specialty()
                                {
                                    Description = datos[1]
                                };
                                SpecialidadesArchivos.Add(tramiteTemporal);
                            }
                        }

                        if (SpecialidadesArchivos.Count > 0)
                        {
                            _context.Specialty.AddRange(SpecialidadesArchivos);
                            _context.SaveChanges();
                            ViewBag.cantidadRenglones = $"Importación con éxito. Se importaron {SpecialidadesArchivos.Count} de {renglonesArchivo.Count}";
                        }
                        else
                            ViewBag.cantidadRenglones = "Error al importar archivo. El tipo de trámite ya existe en la base de datos.-";
                    }
                }
            }

            return View();
        }

        // GET: Specialties
        public async Task<IActionResult> Index(int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };
            var applicationDbContext = _context.Specialty.Select(a => a);

            paginador.cantReg = applicationDbContext.Count();

            var datosAmostrar = applicationDbContext
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            SpecialtiesViewModel Datos = new SpecialtiesViewModel()
            {
                ListSpecialties = datosAmostrar.ToList(),
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialty
                .FirstOrDefaultAsync(m => m.ID == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // GET: Specialties/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Description")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }

        // GET: Specialties/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialty.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Specialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Description")] Specialty specialty)
        {
            if (id != specialty.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.ID))
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
            return View(specialty);
        }

        // GET: Specialties/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialty
                .FirstOrDefaultAsync(m => m.ID == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialties/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialty = await _context.Specialty.FindAsync(id);
            _context.Specialty.Remove(specialty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyExists(int id)
        {
            return _context.Specialty.Any(e => e.ID == id);
        }
    }
}
