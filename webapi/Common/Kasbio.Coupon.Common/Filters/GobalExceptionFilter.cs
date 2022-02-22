using Kasbio.Coupon.Common.Constant.Exception;
using Kasbio.Coupon.Common.DTO;
using Kasbio.Coupon.Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kasbio.Coupon.Common.Filters
{
    /// <summary>
    /// 1.生成一个结果对象
    /// 2. 返回
    /// </summary>
  public  class GobalExceptionFilter: ExceptionFilterAttribute
    {
        
        public override void OnException(ExceptionContext context)
        {
            string msg = (context.Exception is CouponException) ? "发生业务错误" :"系统遇到错误";
            int code = (context.Exception is CouponException) ? 30:31;
            context.Result = new JsonResult(
                new ResponseMessage<ExceptionResponse>(
                    code, msg,
                    new ExceptionResponse( context.Exception.Message)));
      
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            string msg = (context.Exception is CouponException) ? "发生业务错误" : "系统遇到错误";
            int code = (context.Exception is CouponException) ? 30 : 31;
            context.Result = new JsonResult(
                new ResponseMessage<ExceptionResponse>(
                    code, msg,
                    new ExceptionResponse(context.Exception.Message)));
            return Task.FromResult(0);
        }
    }
}
