using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Services;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<IActionResult> AddNewPatient([FromBody] Patient patientToAdd)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var createdPatient = await patientService.AddNewPatient(patientToAdd);

				return Ok($"Novo paciente inserido com sucesso. Id: {createdPatient.Id}");
			}
			catch (InvalidOperationException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return BadRequest(ModelState);
			}
			catch (InvalidDateOfBirthException ex)
			{
                ModelState.AddModelError("DateOfBirth", ex.Message);
                return BadRequest(ModelState);
            }
            catch (DuplicateCpfException ex)
			{
				ModelState.AddModelError("Cpf", ex.Message);
				return BadRequest(ModelState);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, 
					$"Ocorreu um erro inesperado ao cadastrar um novo paciente: {ex.Message}");
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
				return StatusCode(StatusCodes.Status500InternalServerError,
					$"Ocorreu um erro inesperado ao buscar lista com todos os pacientes: {ex.Message}");
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
				return StatusCode(StatusCodes.Status500InternalServerError,
					$"Ocorreu um erro inesperado ao buscar paciente pelo id: {ex.Message}");
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
				return StatusCode(StatusCodes.Status500InternalServerError,
					$"Ocorreu um erro inesperado ao buscar paciente pelo id: {ex.Message}");
			}
		}

		/// <summary>
		/// Update an existing patient.
		/// </summary>
		/// <param name="patientUpdateDto">Object containing the updated data, including the id of the patient to be updated</param>
		/// <exception cref="DuplicateCpfException">Thrown if there is already a patient with the specified id</exception>
		/// <exception cref="MissingIdException">Thrown if the request body is missing the id field</exception>
		[HttpPut]
		public async Task<IActionResult> UpdatePatient([FromBody] PatientUpdateDTO patientUpdateDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await patientService.UpdatePatient(patientUpdateDto);

				return Ok($"Paciente id {patientUpdateDto.Id} foi atualizado com sucesso");
			}
			catch (PatientNotFoundException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return NotFound(ModelState);
			}
            catch (InvalidDateOfBirthException ex)
            {
                ModelState.AddModelError("DateOfBirth", ex.Message);
                return BadRequest(ModelState);
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
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					$"Ocorreu um erro inesperado ao atualizar o paciente: {ex.Message}");
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

				return Ok($"Paciente id {id} foi deletado com sucesso");
			}
			catch (PatientNotFoundException ex)
			{
				ModelState.AddModelError("Id", ex.Message);
				return NotFound(ModelState);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					$"Ocorreu um erro inesperado ao deletar o paciente pelo id: {ex.Message}");
			}
		}
	}
}
