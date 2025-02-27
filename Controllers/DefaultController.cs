﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RequestTracker.Controllers
{
	[ApiController]
	public class DefaultController : ControllerBase
	{
		[HttpGet]
		[HttpPatch]
		[HttpPut]
		[HttpPost]
		[HttpDelete]
		[Route("{url}")]
		public string Response()
		{
			return GetDetails(Request);
		}
		
		public static string GetDetails(HttpRequest request)
		{
			string baseUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString.Value}";
			StringBuilder sbHeaders = new StringBuilder();
			foreach (var header in request.Headers)
				sbHeaders.Append($"{header.Key}: {header.Value}\n");

			string body = "no-body";
			if (request.Body.CanSeek)
			{
				request.Body.Seek(0, SeekOrigin.Begin);
				using (StreamReader sr = new StreamReader(request.Body))
					body = sr.ReadToEnd();
			}

			return $"{request.Protocol} {request.Method} {baseUrl}\n\n{sbHeaders}\n{body}";
		}
	}
}