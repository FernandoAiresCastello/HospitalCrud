using HospitalCrud.DTO;
using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Repositories;
using HospitalCrud.Util;
using HospitalCrud.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using HospitalCrud.Controllers;

namespace HospitalCrud.Services
{
    public class PatientService : IPatientService
	{
		private readonly IPatientRepository patientRepository;

		public PatientService(IPatientRepository patientRepository)
		{
			this.patientRepository = patientRepository;
		}

		public async Task<Patient> AddNewPatient(Patient patient)
		{
            try
			{
				if (patient.Id.HasValue)
					throw new IdNotAllowed();

				patient.Cpf = patient.Cpf.KeepOnlyDigits();
                patient.DateOfBirth = patient.DateOfBirth.ToUtc();

                await patientRepository.Add(patient);

				return patient;
			}
			catch (DbUpdateException ex)
			{
				IdentifyAndRethrowDbUpdateException(ex, patient);
				throw;
			}
		}

		public async Task<ICollection<Patient>> GetAllPatients()
		{
			return await patientRepository.GetAll();
		}

		public async Task<Patient> GetPatientById(int? id)
		{
			return await patientRepository.GetById(id);
		}

		public async Task<Patient> GetPatientByCpf(string cpf)
		{
			return await patientRepository.GetPatientByCpf(cpf);
		}

		public async Task DeletePatientById(int? id)
		{
			await patientRepository.Delete(id);
		}

		/// <summary>
		/// This is the update method called by the view in <see cref="PatientController.Edit(Patient)"/>
		/// </summary>
		public async Task UpdatePatientFromView(Patient newData)
		{
			var currentData = await GetPatientById(newData.Id);

			currentData.FirstName = newData.FirstName;
			currentData.LastName = newData.LastName;
			currentData.Phone = newData.Phone;
			currentData.Email = newData.Email;
			currentData.Cpf = newData.Cpf.KeepOnlyDigits();
			currentData.DateOfBirth = newData.DateOfBirth.ToUtc();

			PerformUpdate(currentData);
		}

		/// <summary>
		/// This is the update method called by the API in <see cref="PatientApiController.UpdatePatient(PatientUpdateDTO)"/>
		/// </summary>
		public async Task UpdatePatientFromApi(PatientUpdateDTO dto)
		{
			var patient = await GetPatientById(dto.Id);

			if (dto.FirstName != null)
				patient.FirstName = dto.FirstName;
			if (dto.LastName != null)
				patient.LastName = dto.LastName;
			if (dto.Phone != null)
				patient.Phone = dto.Phone;
			if (dto.Email != null)
				patient.Email = dto.Email;
			if (dto.Cpf != null)
				patient.Cpf = dto.Cpf.KeepOnlyDigits();
            if (dto.DateOfBirth != null)
                patient.DateOfBirth = dto.DateOfBirth.ToUtc();

			PerformUpdate(patient);
		}

		private async void PerformUpdate(Patient patient)
		{
			try
			{
				await patientRepository.Update(patient);
			}
			catch (DbUpdateException ex)
			{
				IdentifyAndRethrowDbUpdateException(ex, patient);
				throw;
			}
		}

		private void IdentifyAndRethrowDbUpdateException(DbUpdateException ex, Patient? patient = null)
		{
			if (PostgresExceptionIdentifier.IsValueTooLongException(ex))
				throw new ValueTooLongException(ex.InnerException!.Message);
			if (PostgresExceptionIdentifier.IsUniqueConstraintViolationError(ex, Patient.UniqueCpfConstraint))
				throw new DuplicateCpfException(patient.Cpf!);
			if (PostgresExceptionIdentifier.IsUndefinedTable(ex))
				throw new UndefinedTableException(ex);
		}
	}
}
