using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Hospitel_Project.Models;
using Hospitel_Project.Context;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    // ??? ???? ??????? ??????? ??????? ??????
    public JsonResult GetDoctorsAndPatients()
    {
        try
        {
            var doctors = _context.Doctors
                .AsNoTracking()
                .Select(d => new { Id = d.DoctorId, Name = "Doctor: " + d.Name })
                .ToList();

            var patients = _context.Patients
                .AsNoTracking()
                .Select(p => new { Id = p.PatientId, Name = "Patient: " + p.FirstName + " " + p.LastName })
                .ToList();

            return Json(new { Doctors = doctors, Patients = patients });
        }
        catch (Exception ex)
        {
            return Json(new { Error = "??? ??? ????? ??? ????????.", Details = ex.Message });
        }
    }

    // ??? ?????? ????? ????? ???? ????
    public JsonResult GetPatientsByDoctor(int doctorId)
    {
        if (doctorId <= 0)
            return Json(new { Error = "??? ?????? ??? ????." });

        try
        {
            var patients = _context.Visitations
                .AsNoTracking()
                .Where(v => v.DoctorId == doctorId)
                .Select(v => new { v.Patient.PatientId, Name = v.Patient.FirstName + " " + v.Patient.LastName })
                .Distinct()
                .ToList();

            if (!patients.Any())
                return Json(new { Message = "?? ???? ???? ???? ??????." });

            return Json(patients);
        }
        catch (Exception ex)
        {
            return Json(new { Error = "??? ??? ????? ??? ?????? ??????.", Details = ex.Message });
        }
    }


    // ??? ??????? ?????????? ????? ????
    public JsonResult GetMedicationsAndDiagnosesByPatient(int patientId)
    {
        if (patientId <= 0)
            return Json(new { Error = "??? ?????? ??? ????." });

        try
        {
            var patient = _context.Patients
                .AsNoTracking()
                .Include(p => p.PatientMedications)
                .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Diagnoses)
                .FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null)
                return Json(new { Error = "?????? ??? ?????." });

            var medications = patient.PatientMedications
                .Select(pm => new { Id = pm.MedicamentId, Name = "Medicament: " + pm.Medicament.Name })
                .ToList();

            var diagnoses = patient.Diagnoses
                .Select(d => new { Id = d.DiagnoseId, Name = "Diagnosis: " + d.Name })
                .ToList();

            return Json(new { Medications = medications, Diagnoses = diagnoses });
        }
        catch (Exception ex)
        {
            return Json(new { Error = "??? ??? ????? ??? ?????? ??????.", Details = ex.Message });
        }
    }

}

