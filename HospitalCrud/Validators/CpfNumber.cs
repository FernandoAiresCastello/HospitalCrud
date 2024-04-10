using HospitalCrud.Util;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HospitalCrud.Validators
{
	/// <summary>
	/// Validator for a CPF number property
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class CpfNumber : StringLengthAttribute
	{
		private const int CpfLength = 11;

		public CpfNumber() : base(CpfLength)
		{
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			if (value is string cpf)
				return IsCpf(cpf);

			return false;
		}

		private bool IsCpf(string cpf)
		{
			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");

			if (cpf.Length != CpfLength)
			{
				ErrorMessage = ValidationMessages.InvalidCpfLength;
				return false;
			}

			if (!Regex.IsMatch(cpf, @"^\d{11}$"))
			{
				ErrorMessage = ValidationMessages.InvalidCpfFormat;
				return false;
			}

			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;

			tempCpf = cpf.Substring(0, 9);
			soma = 0;

			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();

			tempCpf += digito;

			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito += resto.ToString();

			bool digitoOk = cpf.EndsWith(digito);

			if (!digitoOk)
				ErrorMessage = ValidationMessages.InvalidCpfFormat;

			return digitoOk;
		}
	}
}
