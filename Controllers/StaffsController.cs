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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Medicaly.ModelsView;

namespace Medicaly.Controllers
{
    public class StaffsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public StaffsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Staffs
        public async Task<IActionResult> Index(string searchLastName, int? specialtyID, int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var applicationDbContext = _context.Staff.Include(s => s.Specialty).Select(a => a);
            if (!string.IsNullOrEmpty(searchLastName))
            {
                applicationDbContext = applicationDbContext.Where(a => a.LastName.Contains(searchLastName));
            };
            if (specialtyID.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(a => a.SpecialtyID == specialtyID);
            };

            paginador.cantReg = applicationDbContext.Count();

            //paginador.totalPag = (int)Math.Ceiling((decimal)paginador.cantReg / paginador.regXpag);
            var datosAmostrar = applicationDbContext
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            StaffsViewModel Datos = new StaffsViewModel()
            {
                ListStaffs = datosAmostrar.ToList(),
                ListSpecialties = new SelectList(_context.Specialty, "ID", "Description", specialtyID),
                searchLastName = searchLastName,
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["SpecialtyID"] = new SelectList(_context.Specialty, "ID", "Description");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,Dni,CellNumber,Street,Number,Floor,Department,Birthday,SpecialtyID,Antiquity,Photo")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                var archive = HttpContext.Request.Form.Files;
                if (archive != null && archive.Count > 0)
                {
                    var archivePhoto = archive[0];
                    var pathDestiny = Path.Combine(env.WebRootPath, "assets/staffs");
                    if (archivePhoto.Length > 0)
                    {
                        var archiveDestiny = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivePhoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestiny, archiveDestiny), FileMode.Create))
                        {
                            archivePhoto.CopyTo(filestream);
                            staff.Photo = archiveDestiny;
                        };
                    }
                };
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialtyID"] = new SelectList(_context.Specialty, "ID", "Description", staff.SpecialtyID);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["SpecialtyID"] = new SelectList(_context.Specialty, "ID", "Description", staff.SpecialtyID);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,Dni,CellNumber,Street,Number,Floor,Department,Birthday,SpecialtyID,Antiquity,Photo")] Staff staff)
        {
            if (id != staff.ID)
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

                            if (!string.IsNullOrEmpty(staff.Photo))
                            {
                                string previousPhoto = Path.Combine(pathDestiny, staff.Photo);
                                if (System.IO.File.Exists(previousPhoto))
                                    System.IO.File.Delete(previousPhoto);
                            }

                            using (var filestream = new FileStream(Path.Combine(pathDestiny, archiveDestiny), FileMode.Create))
                            {
                                archivePhoto.CopyTo(filestream);
                                staff.Photo = archiveDestiny;
                            }
                        }
                    }
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.ID))
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
            ViewData["SpecialtyID"] = new SelectList(_context.Specialty, "ID", "Description", staff.SpecialtyID);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.ID == id);
        }
    }
}
