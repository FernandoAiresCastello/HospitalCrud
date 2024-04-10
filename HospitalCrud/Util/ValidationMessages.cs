namespace HospitalCrud.Util
{
	public static class ValidationMessages
	{
		public const string RequiredFirstName = "Nome é obrigatório";
		public const string RequiredLastName = "Sobrenome é obrigatório";
		public const string RequiredCpf = "CPF é obrigatório";

		public const string InvalidEmail = "Endereço de e-mail inválido";
		public const string InvalidPhone = "Formato de telefone inválido";
		public const string InvalidCpfFormat = "CPF inválido";
		public const string InvalidCpfLength = "Tamanho de CPF inválido (necessário exatamente 11 dígitos)";
		public const string InvalidDateOfBirth = "Data de nascimento inválida";
	}
}
