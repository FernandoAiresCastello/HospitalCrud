using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Services;
using HospitalCrud.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Mime;

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

		public PatientApiController(IPatientService patientService)
		{
			this.patientService = patientService;
		}

        [HttpPost]
		[SwaggerOperation(
			Summary = "Create a new Patient",
			Description = "Create a new Patient and add it to the database"
		)]	
		[SwaggerResponse(200, "Patient created and successfully added to the database", typeof(Patient), MediaTypeNames.Application.Json)]
		[SwaggerResponse(400, "Could not create Patient due to invalid data in the JSON payload")]
		[SwaggerResponse(500, "Could not create Patient due to an unexpected internal server error")]
		[SwaggerRequestExample(typeof(Patient), typeof(PatientExample))]
		public async Task<IActionResult> AddNewPatient([FromBody, SwaggerParameter("The Patient data")] Patient patientToAdd)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var createdPatient = await patientService.AddNewPatient(patientToAdd);

				return Success($"Novo paciente inserido com sucesso. Id: {createdPatient.Id}");
			}
			catch (IdNotAllowed ex)
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
				return DatabaseUpdateError(ex);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		[HttpGet]
		[SwaggerOperation(
			Summary = "Retrieve a list of all Patients",
			Description = "Returns a list containing all Patients"
		)]
		[SwaggerResponse(200, "List with all Patients", typeof(ICollection<Patient>), MediaTypeNames.Application.Json)]
		[SwaggerResponse(500, "An unexpected internal server error occurred")]
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

		[HttpGet("{id}")]
		[HttpGet("id/{id}")]
		[SwaggerOperation(
			Summary = "Retrieve a Patient by their database id",
			Description = "Returns the Patient if found, otherwise returns 404"
		)]
		[SwaggerResponse(200, "Patient found with specified id", typeof(Patient), MediaTypeNames.Application.Json)]
		[SwaggerResponse(404, "Patient not found with specified id")]
		[SwaggerResponse(500, "An unexpected internal server error occurred")]
		public async Task<IActionResult> GetPatientById([SwaggerParameter("Database id of the Patient")] int id)
		{
			try
			{
				var patient = await patientService.GetPatientById(id);

				return Ok(patient);
			}
			catch (PatientNotFoundException ex)
			{
				return PatientNotFound(ex);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		[HttpGet("cpf/{cpf}")]
		[SwaggerOperation(
			Summary = "Retrieve a Patient by their CPF number",
			Description = "Returns the Patient if found, otherwise returns 404"
		)]
		[SwaggerResponse(200, "Patient found with specified CPF number", typeof(Patient), MediaTypeNames.Application.Json)]
		[SwaggerResponse(404, "Patient not found with specified CPF number")]
		[SwaggerResponse(500, "An unexpected internal server error occurred")]
		public async Task<IActionResult> GetPatientByCpf([SwaggerParameter("The CPF number to look for")] string cpf)
		{
			try
			{
				var patient = await patientService.GetPatientByCpf(cpf);

				return Ok(patient);
			}
			catch (PatientNotFoundException ex)
			{
				return PatientNotFound(ex);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		[HttpPut]
		[SwaggerOperation(
			Summary = "Update an existing Patient",
			Description = "Update data for an existing Patient"
		)]
		[SwaggerResponse(200, "Patient updated successfully")]
		[SwaggerResponse(400, "Could not update Patient due to invalid data in the JSON payload")]
		[SwaggerResponse(404, "Patient not found with specified id")]
		[SwaggerResponse(500, "Could not update Patient due to an unexpected internal server error")]
		[SwaggerRequestExample(typeof(Patient), typeof(PatientExample))]
		public async Task<IActionResult> UpdatePatient([FromBody, SwaggerParameter("The data to update for the Patient")] PatientUpdateDTO patientUpdateDto)
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
				return PatientNotFound(ex);
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
				return DatabaseUpdateError(ex);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		[HttpDelete("id/{id}")]
		[SwaggerOperation(
			Summary = "Delete a Patient",
			Description = "Delete an existing Patient"
		)]
		[SwaggerResponse(200, "Patient deleted successfully")]
		[SwaggerResponse(404, "Patient not found with specified id")]
		[SwaggerResponse(500, "An unexpected internal server error occurred")]
		public async Task<IActionResult> DeletePatientById([SwaggerParameter("Database id of the Patient")] int id)
		{
			try
			{
				await patientService.DeletePatientById(id);

				return Success($"Paciente id {id} foi deletado com sucesso");
			}
			catch (PatientNotFoundException ex)
			{
				return PatientNotFound(ex);
			}
			catch (Exception ex)
			{
				return UnexpectedError(ex);
			}
		}

		private ApiResponseObject Success(string message)
		{
			return new ApiResponseObject(StatusCodes.Status200OK, "Sucesso", message);
		}

		private ApiResponseObject PatientNotFound(PatientNotFoundException ex)
		{
			return new ApiResponseObject(StatusCodes.Status404NotFound, "Não encontrado", ex.Message);
		}

		private ApiResponseObject DatabaseUpdateError(Exception ex)
		{
			return new ApiResponseObject(StatusCodes.Status500InternalServerError, "Erro no update da base", ex.Message);
		}

		private ApiResponseObject UnexpectedError(Exception ex)
		{
			return new ApiResponseObject(StatusCodes.Status500InternalServerError, "Erro interno", ex.Message);
		}
	}
}
