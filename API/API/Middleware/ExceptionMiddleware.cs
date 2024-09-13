using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace API.Middleware;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
 public async Task InvokeAsync(HttpContext context)
 {
  try
  {
   await next(context);
  }
  catch (Exception e)
  {
   await HandleExceptionAsync(context, e, env);
  }
 }

 private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
 {
  context.Response.ContentType = "application/json";
  context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

  var response = env.IsDevelopment()
   ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
   : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal server error");

  var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

  var json = JsonSerializer.Serialize(response, options);

  return context.Response.WriteAsync(json);
 }
}