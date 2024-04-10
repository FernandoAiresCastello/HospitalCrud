using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Services;
using HospitalCrud.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;

namespace HospitalCrud.Controllers
{
    /// <summary>
    /// API controller for managing patients.
    /// For the controller used by the views, see <see cref="PatientController"/>
    /// </summary>
    [ApiController]
	[Route("api/patients")]
	public class PatientApiController : ControllerBase
	{
		private readonly IPatientService patientService;

		/// <summary>
		/// Initializes a new instance of the <see cref="PatientApiController"/> class.
		/// </summary>
		/// <param name="patientService">The injected patient service</param>
		public PatientApiController(IPatientService patientService)
		{
			this.patientService = patientService;
		}

        /// <summary>
        /// Create and add a new patient.
        /// </summary>
        /// <param name="patientToAdd">Object containing the required data to register a new patient</param>
        /// <returns>The JSON of the created patient.</returns>
        /// <exception cref="DuplicateCpfException">Thrown if there is already a patient with the specified id</exception>
        [HttpPost]
		[SwaggerRequestExample(typeof(Patient), typeof(PatientExample))]
		public async Task<IActionResult> AddNewPatient([FromBody] Patient patientToAdd)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var createdPatient = await patientService.AddNewPatient(patientToAdd);

				return Success($"Novo paciente inserido com sucesso. Id: {createdPatient.Id}");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return BadRequest(ModelState);
			}
			catch (DuplicateCpfException ex)
			{
				ModelState.AddModelError("Cpf", ex.Message);
				return BadRequest(ModelState);
			}
			catch (ValueTooLongException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return BadRequest(ModelState);
			}
			catch (DbUpdateException ex)
			{
				ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
				return BadRequest(ModelState);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		/// <summary>
		/// Retrieves a list of all patients.
		/// </summary>
		/// <returns>The list of all patients.</returns>
		[HttpGet]
		public async Task<IActionResult> GetAllPatients()
		{
			try
			{
				var patients = await patientService.GetAllPatients();

				return Ok(patients);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		/// <summary>
		/// Retrieves a patient by their database id.
		/// </summary>
		/// <param name="id">The database id of the patient</param>
		/// <returns>The patient with the specified id.</returns>
		/// <exception cref="PatientNotFoundException">Thrown if no patient is found with the specified id</exception>
		[HttpGet("id/{id}")]
		public async Task<IActionResult> GetPatientById(int id)
		{
			try
			{
				var patient = await patientService.GetPatientById(id);

				return Ok(patient);
			}
			catch (PatientNotFoundException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return NotFound(ModelState);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		/// <summary>
		/// Retrieves a patient by their CPF number.
		/// </summary>
		/// <param name="cpf">The CPF number of the patient to look for</param>
		/// <returns>The patient with the specified CPF number.</returns>
		/// <exception cref="PatientNotFoundException">Thrown if no patient is found with the specified id</exception>
		[HttpGet("cpf/{cpf}")]
		public async Task<IActionResult> GetPatientByCpf(string cpf)
		{
			try
			{
				var patient = await patientService.GetPatientByCpf(cpf);

				return Ok(patient);
			}
			catch (PatientNotFoundException ex)
			{
				ModelState.AddModelError("Cpf", ex.Message);
				return NotFound(ModelState);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		/// <summary>
		/// Update an existing patient.
		/// </summary>
		/// <param name="patientUpdateDto">Object containing the updated data, including the id of the patient to be updated</param>
		/// <exception cref="DuplicateCpfException">Thrown if there is already a patient with the specified id</exception>
		/// <exception cref="MissingIdException">Thrown if the request body is missing the id field</exception>
		[HttpPut]
		[SwaggerRequestExample(typeof(Patient), typeof(PatientExample))]
		public async Task<IActionResult> UpdatePatient([FromBody] PatientUpdateDTO patientUpdateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await patientService.UpdatePatientFromApi(patientUpdateDto);

				return Success($"Paciente id {patientUpdateDto.Id} foi atualizado com sucesso");
			}
			catch (PatientNotFoundException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return NotFound(ModelState);
			}
            catch (DuplicateCpfException ex)
			{
				ModelState.AddModelError("Cpf", ex.Message);
				return BadRequest(ModelState);
			}
			catch (MissingIdException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return BadRequest(ModelState);
			}
			catch (ValueTooLongException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return BadRequest(ModelState);
			}
			catch (DbUpdateException ex)
			{
				ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
				return BadRequest(ModelState);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		/// <summary>
		/// Delete the patient with the specified database id.
		/// </summary>
		/// <param name="id">The database id of the patient</param>
		/// <exception cref="PatientNotFoundException">Thrown if no patient is found with the specified id</exception>
		[HttpDelete("id/{id}")]
		public async Task<IActionResult> DeletePatientById(int id)
		{
			try
			{
				await patientService.DeletePatientById(id);

				return Success($"Paciente id {id} foi deletado com sucesso");
			}
			catch (PatientNotFoundException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return NotFound(ModelState);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		private OkObjectResult Success(string message)
		{
			return new OkObjectResult(new
			{
				resultado = "sucesso",
				mensagem = message
			});
		}

		private ObjectResult UnexpectedError(Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro inesperado: {ex.Message}");
		}
	}
}
