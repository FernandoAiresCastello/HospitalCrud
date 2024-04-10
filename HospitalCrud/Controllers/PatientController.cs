using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalCrud.Controllers
{
    /// <summary>
    /// Controller for the views related to patient management.
    /// For the API controller, see <see cref="PatientApiController"/>
    /// </summary>
    public class PatientController : Controller
    {
        private readonly IPatientService patientService;

        public PatientController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        public async Task<IActionResult> List()
        {
            var patients = await patientService.GetAllPatients();

            return View(patients);
        }

		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Patient patient)
        {
			if (ModelState.IsValid)
			{
                try
                {
                    await patientService.AddNewPatient(patient);

					return RedirectToAction(nameof(List));
				}
                catch (InvalidDateOfBirthException ex)
                {
                    ModelState.AddModelError("DateOfBirth", ex.Message);
                }
                catch (DuplicateCpfException ex)
                {
                    ModelState.AddModelError("CPF", ex.Message);
                }
				catch (InvalidOperationException ex)
				{
					ModelState.AddModelError("Id", ex.Message);
				}
				catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
                catch (Exception ex)
                {
					ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
				}
			}

			return View(patient);
		}

		public async Task<IActionResult> Edit(int patientId)
        {
			var patient = await patientService.GetPatientById(patientId);

			return View(patient);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Patient patient)
		{
            if (ModelState.IsValid)
            {
				try
				{
					await patientService.UpdatePatient(patient);

					return RedirectToAction(nameof(List));
				}
				catch (InvalidDateOfBirthException ex)
				{
					ModelState.AddModelError("DateOfBirth", ex.Message);
				}
				catch (DuplicateCpfException ex)
				{
					ModelState.AddModelError("CPF", ex.Message);
				}
				catch (DbUpdateException ex)
				{
					ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
				}
			}

			return View(patient);
		}

		public async Task<IActionResult> Delete(int patientId)
		{
            await patientService.DeletePatientById(patientId);

            return RedirectToAction(nameof(List));
        }
    }
}
