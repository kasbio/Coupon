using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Common.DTO
{
    public class ResponseMessage<T> 
	{
		private string message;

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		private int code;

		public int Code
		{
			get { return code; }
			set { code = value; }
		}


		private T data;

		public T Data
		{
			get { return data; }
			set { data = value; }
		}

		public ResponseMessage(int code,string message,T data)
		{
			this.code = code;
			this.message = message;
			this.data = data;
		}


		public static ResponseMessage<T> GetSuccessResponse(T data)
		{
			return new ResponseMessage<T>(200, "success", data);
		}

		public static ResponseMessage<T> GetFailResponse(T data)
		{
			return new ResponseMessage<T>(400, "success", data);
		}
	}


	public class ResponseMessage : ResponseMessage<object>
	{
		public ResponseMessage(int code, string message, object data):base(code,message,data)
		{
			 
		}
	}
}
