using MiniBank.Core;
namespace MiniBank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                if (ex._accountExceptionMessage != null)
                {
                    await context.Response.WriteAsJsonAsync(new { Message = ex.Message, ex._accountExceptionMessage });
                }
                else if (ex._currencyExceptionMessage != null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { Message = ex.Message, ex._currencyExceptionMessage });
                }
                else if (ex._transactionExceptionMessage != null)
                {
                    await context.Response.WriteAsJsonAsync(new { Message = ex.Message, ex._transactionExceptionMessage });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new { Message = ex.Message});
                }
                
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(ex => $"{ex.PropertyName}: {ex.ErrorMessage}");
                var errorMessage = string.Join(Environment.NewLine, errors);
                await context.Response.WriteAsJsonAsync(new { Message = errorMessage });
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsJsonAsync(new { Message = "Внутренняя ошибка сервиса"});
            }
        }
    }
}
