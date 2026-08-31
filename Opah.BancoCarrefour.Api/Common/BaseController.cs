using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Opah.BancoCarrefour.Api.Common;

[ApiController]
public class BaseController : ControllerBase
{
    protected int GetCurrentUserId()
        => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data)
        => base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Ok<T>(T data, string message)
        => base.Ok(new ApiResponseWithData<T> { Data = data, Success = true, Message = message });

    protected IActionResult Created<T>(string routeName, object routeValues, T data)
        => base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data, string message)
        => base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true, Message = message });

    protected IActionResult BadRequest(string message)
        => base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found")
        => base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList)
        => base.Ok(new
        {
            currentPage = pagedList.CurrentPage,
            totalPages = pagedList.TotalPages,
            totalCount = pagedList.TotalCount,
            data = pagedList.Items,
            success = true,
            message = (pagedList.TotalCount == 0) ? "Nenhum registro encontrado" : $"{pagedList.TotalCount} registros retornados com sucesso!",
            errors = Array.Empty<object>()
        });
}
