using HospitalCrud.Model;
using Microsoft.EntityFrameworkCore;

namespace HospitalCrud.Data
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Patient> Patients { get; set; } = null!;

		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Patient>()
				.ToTable("Patient")
				.HasIndex(patient => patient.Cpf)
				.IsUnique(); // This will prevent including patients with duplicate CPF numbers
		}
	}
}
