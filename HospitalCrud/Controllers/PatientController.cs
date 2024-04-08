using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalCrud.Controllers
{
	/// <summary>
	/// Controller for managing patients.
	/// </summary>
	[ApiController]
	[Route("api/patients")]
	public class PatientController : ControllerBase
	{
		private readonly IPatientService patientService;

		/// <summary>
		/// Initializes a new instance of the <see cref="PatientController"/> class.
		/// </summary>
		/// <param name="patientService">The injected patient service</param>
		public PatientController(IPatientService patientService)
		{
			this.patientService = patientService;
		}

		/// <summary>
		/// Create and add a new patient.
		/// </summary>
		/// <param name="patientDto">Object containing the required data to register a new patient</param>
		/// <returns>The JSON of the created patient.</returns>
		/// <exception cref="DuplicateCpfException">Thrown if there is already a patient with the specified id</exception>
		[HttpPost]
		public async Task<IActionResult> AddNewPatient([FromBody] PatientDTO patientDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (patientDto.Id.HasValue)
			{
				ModelState.AddModelError("Id", "Não é permitido informar o id ao cadastrar um novo paciente.");
				return BadRequest(ModelState);
			}

			try
			{
				var patient = await patientService.AddNewPatient(patientDto);

				return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
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
				var patientDto = await patientService.GetPatientDtoById(id);

				return Ok(patientDto);
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
				var patientDto = await patientService.GetPatientDtoByCpf(cpf);

				return Ok(patientDto);
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
		/// <param name="patientDto">Object containing the updated data, including the id of the patient to be updated</param>
		/// <exception cref="DuplicateCpfException">Thrown if there is already a patient with the specified id</exception>
		/// <exception cref="MissingIdException">Thrown if the request body is missing the id field</exception>
		[HttpPut]
		public async Task<IActionResult> UpdatePatient([FromBody] PatientUpdateDTO patientDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await patientService.UpdatePatient(patientDto);

				return Ok($"Paciente id {patientDto.Id} foi atualizado com sucesso");
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
