using ExchangeAPI.Controllers;
using ExchangeAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            string msg = "";

            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                msg = "Currency not implemented in our exchange.";
                _logger.LogError(msg);
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound, msg);
            }
            catch (WebException ex)
            {
                msg = "One of our providers is not available.";
                _logger.LogError(msg);
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadGateway, msg);
            }
            catch (Exception ex)
            {
                msg = "There was a server error.";
                _logger.LogError(msg);
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError, msg);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode status, string message)
        {
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
