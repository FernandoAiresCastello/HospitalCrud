using HospitalCrud.Data;
using HospitalCrud.Exceptions;
using HospitalCrud.Model;
using HospitalCrud.Util;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HospitalCrud.Repositories
{
	/// <summary>
	/// Data repository containing patients
	/// </summary>
    public class PatientRepository : IPatientRepository
    {
        private DatabaseContext db;

		/// <summary>
		/// Initializes a new instance of the <see cref="PatientRepository"/> class.
		/// </summary>
		/// <param name="db">The injected database context</param>
		public PatientRepository(DatabaseContext db)
        {
            this.db = db;
        }

		/// <inheritdoc/>
		public async Task Add(Patient patient)
		{
			db.Patients.Add(patient);
			await db.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task Update(Patient patient)
		{
			db.Patients.Update(patient);
			await db.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task Delete(int? id)
		{
			if (!id.HasValue)
				throw new MissingIdException();

			var patient = await db.Patients.FindAsync(id) ?? 
				throw new PatientNotFoundException(id);

			db.Patients.Remove(patient);
			await db.SaveChangesAsync();
		}

		/// <inheritdoc/>
		public async Task<Patient> GetById(int? id)
		{
			if (!id.HasValue)
				throw new MissingIdException();

			return await db.Patients.FindAsync(id) ?? 
				throw new PatientNotFoundException(id);
		}

		/// <inheritdoc/>
		public async Task<ICollection<Patient>> GetAll()
		{
			try
			{
				return await db.Patients.ToListAsync();
			}
			catch (NpgsqlException ex)
			{
				if (PostgresExceptionIdentifier.IsUndefinedTable(ex))
					throw new UndefinedTableException(ex);
			}

			return new List<Patient>();
		}

		/// <summary>
		/// Retrieve a patient by their CPF number.
		/// </summary>
		/// <param name="cpf">The CPF number to look for</param>
		/// <returns>The patient with the specified CPF, if found</returns>
		/// 
		/// <exception cref="PatientNotFoundException">
		/// Thrown if no patient is found with the specified CPF number
		/// </exception>
		public async Task<Patient> GetPatientByCpf(string cpf)
		{
			var query = from patient in db.Patients
						where patient.Cpf == cpf
						select patient;

			int count = await query.CountAsync();

			if (count == 0)
				throw new PatientNotFoundException(cpf);

			return await query.FirstAsync();
		}
	}
}
