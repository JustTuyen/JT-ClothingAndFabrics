using Microsoft.EntityFrameworkCore;
using Npgsql;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            _logger.LogWarning(ex, "Duplicate key violation: {ConstraintName}", pgEx.ConstraintName);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Dữ liệu bị trùng lặp, vui lòng kiểm tra lại.",
                field = pgEx.ConstraintName   // trả về tên constraint để debug/hiển thị cụ thể hơn nếu cần
            });
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23503")
        {
            _logger.LogWarning(ex, "Foreign key violation: {ConstraintName}", pgEx.ConstraintName);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Dữ liệu tham chiếu không tồn tại (khóa ngoại không hợp lệ)."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau."
            });
        }
    }
}