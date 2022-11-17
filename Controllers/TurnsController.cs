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
    public class TurnsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Turns
        public async Task<IActionResult> Index(string searchDni, int pagina = 1)
        {

            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var applicationDbContext = _context.Turn.Include(t => t.Patient).Include(t => t.Staff).Select(a => a);
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

            TurnsViewModel Datos = new TurnsViewModel()
            {
                ListTurns = datosAmostrar.ToList(),
                searchDni = searchDni,
                paginador = paginador
            };

            return View(Datos);
        }

        // GET: Turns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turn = await _context.Turn
                .Include(t => t.Patient)
                .Include(t => t.Staff)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (turn == null)
            {
                return NotFound();
            }

            return View(turn);
        }

        // GET: Turns/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber");
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName");
            return View();
        }

        // POST: Turns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StaffID,PatientID,DateTime,Description")] Turn turn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber", turn.PatientID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName", turn.StaffID);
            return View(turn);
        }

        // GET: Turns/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turn = await _context.Turn.FindAsync(id);
            if (turn == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber", turn.PatientID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName", turn.StaffID);
            return View(turn);
        }

        // POST: Turns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StaffID,PatientID,DateTime,Description")] Turn turn)
        {
            if (id != turn.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnExists(turn.ID))
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
            ViewData["PatientID"] = new SelectList(_context.Patient, "ID", "CellNumber", turn.PatientID);
            ViewData["StaffID"] = new SelectList(_context.Staff, "ID", "LastName", turn.StaffID);
            return View(turn);
        }

        // GET: Turns/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turn = await _context.Turn
                .Include(t => t.Patient)
                .Include(t => t.Staff)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (turn == null)
            {
                return NotFound();
            }

            return View(turn);
        }

        // POST: Turns/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turn = await _context.Turn.FindAsync(id);
            _context.Turn.Remove(turn);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnExists(int id)
        {
            return _context.Turn.Any(e => e.ID == id);
        }
    }
}
