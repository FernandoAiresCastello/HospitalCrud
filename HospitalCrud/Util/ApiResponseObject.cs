using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HospitalCrud.Util
{
	public class ApiResponseObject : ObjectResult
	{
		public ApiResponseObject(int statusCode, string title, string message) : 
			this(statusCode, title, new List<string>{ message })
		{
		}

		public ApiResponseObject(int statusCode, string title, IEnumerable<string> messages) : 
			base(new
			{
				codigo = statusCode,
				resultado = title,
				mensagens = messages
			})
		{
			StatusCode = statusCode;
		}
	}
}
