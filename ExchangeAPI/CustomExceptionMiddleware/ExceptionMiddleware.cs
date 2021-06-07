using ExchangeAPI.Controllers;
using ExchangeAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeAPI.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<CurrencyController> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (WebException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadGateway, "One of our providers is not available.");
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest, "You execeded you month limit.");
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound, "Currency not implemented in our exchange.");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError, "There was a server error.");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode status, string message)
        {
            _logger.LogError(exception.Message + ". " + DateTime.Now.ToString());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
