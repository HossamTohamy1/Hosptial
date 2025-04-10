using Hospitel_Project.Context;
using Hospitel_Project.Models;
using Hospitel_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospitel_Project.Controllers
{
    public class PrescribeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;

        public PrescribeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailService emailService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var prescriptions = _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Medicament)
                .ToList();

            return View(prescriptions);
        }
        [HttpGet]
        public IActionResult Prescribe()
        {
            ViewBag.Patients = _context.Patients.ToList();
            ViewBag.Medicaments = _context.Medicaments.ToList();
            return View();
        }
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prescribe(int patientId, int medicamentId, int quantity, string description)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Quantity must be greater than zero.");
                return RedirectToAction("Index");
            }

            var medicament = await _context.Medicaments.FindAsync(medicamentId);

            if (medicament == null || medicament.stock < quantity)
            {
                ModelState.AddModelError("", "Not enough stock available.");
                return RedirectToAction("Index");
            }

            var prescription = new Prescription
            {
                PatientId = patientId,
                MedicamentId = medicamentId,
                Quantity = quantity,
                description = description,
                DatePrescribed = DateTime.Now
            };


            medicament.stock -= quantity;

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Medicament)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PrescriptionId == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.PrescriptionId == id);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || !int.TryParse(id, out int prescriptionId))
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Medicament)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PrescriptionId == prescriptionId);

            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Medicament)
                .FirstOrDefaultAsync(m => m.PrescriptionId == id);

            if (prescription == null)
            {
                return NotFound();
            }

            
            ViewBag.Patients = await _context.Patients.ToListAsync();
            ViewBag.Medicaments = await _context.Medicaments.ToListAsync();

            return View(prescription);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prescription prescription)
        {
            if (id != prescription.PrescriptionId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                
                ViewBag.Patients = await _context.Patients.ToListAsync();
                ViewBag.Medicaments = await _context.Medicaments.ToListAsync();
                return View(prescription);
            }

            try
            {
                _context.Update(prescription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prescriptions.Any(e => e.PrescriptionId == id))
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


        // GET: Prescriptions/Create
        // GET: Prescriptions/Create
    }
}
