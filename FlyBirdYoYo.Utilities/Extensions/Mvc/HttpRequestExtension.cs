using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
 
namespace Microsoft.AspNetCore.Mvc
{
	public static class HttpRequestExtension
	{
		/// <summary>
		/// ipv6本机环回址
		/// </summary>
		public const string LocalLoopHostIpAddress = "::1";

		#region HttpRequest Extension


		public static T GetQuery<T>(this HttpRequest request, string name)
		{
			try
			{
				Type t = typeof(T);
				//StringValues if there are multiple values like ?page=1&page=2 then the result of the ToString call will be 1,2
				object result = request.Query[name].ToString();
				switch (t.Name)
				{
					case "String":
					case "Object":
						return (T)result;
					default:
						BindingFlags flag = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;


						return (T)t.InvokeMember("Parse", flag, null, null, new object[] { result });
				}
			}
			catch { return default(T); }
		}

		public static T GetForm<T>(this HttpRequest request, string name)
		{
			try
			{
				Type t = typeof(T);
				object result = request.Form[name];//request[name]
				switch (t.Name)
				{
					case "String":
					case "Object":
						return (T)result;
					default:
						BindingFlags flag = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
						return (T)t.InvokeMember("Parse", flag, null, null, new object[] { result });
				}
			}
			catch { return default(T); }
		}

		/// <summary>
		/// Determines whether the specified HTTP request is an AJAX request.
		/// </summary>
		/// 
		/// <returns>
		/// true if the specified HTTP request is an AJAX request; otherwise, false.
		/// </returns>
		/// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			if (request.Headers != null)
				return request.Headers["X-Requested-With"] == "XMLHttpRequest";
			return false;
		}

		public static int GetFormCheck(this HttpRequest request, string name)
		{
			var r = GetForm<string>(request, name);
			if (r.IsNull())
			{
				return 0;
			}
			else
			{
				if (r.ToString().Equals("on"))
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}
		public static T GetFormOrDefault<T>(this HttpRequest request, string name, T value)
		{
			return GetForm<T>(request, name).IsNull() ? value : GetForm<T>(request, name);
		}

		public static T GetEntity<T>(this HttpRequest request)
		{
			string jsonStr = request.GetForm<string>("Entity");
			return jsonStr.FromJson<T>();
		}


		public static List<T> GetList<T>(this HttpRequest request)
		{
			string jsonStr = request.GetForm<string>("List");
			return jsonStr.FromJson<List<T>>();
		}
		/// <summary>
		/// 返回当前请求的IP地址
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string GetIP(this HttpRequest request)
		{
			var ipAddr = request.HttpContext.Connection.RemoteIpAddress;
			 string ipStr=ipAddr==null?"":ipAddr.ToString();
			if (ipStr.Equals(LocalLoopHostIpAddress))
			{
				ipStr = "127.0.0.1";
			}

			return ipStr;
		}

		#endregion

	}
}
