using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTOs;

namespace ToDo.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected ActionResult CustomResponse(int status, bool success, object data = null)
        {
            return (status, success) switch
            {
                (404, false) => NotFound(new BaseResponse { StatusCode = status, Success = success, Message = "No elements found." }),
                (400, false) => BadRequest(new BaseResponse { StatusCode = status, Success = success, Message = "Errors during the transaction.", Data = data}),
                (201, true) => Ok(new BaseResponse { StatusCode = status, Success = success, Message = "Created", Data = data }),
                (200, true) => Ok(new BaseResponse { StatusCode = status, Success = success, Message = "The operation was successful", Data = data }),
                (204, true) => NoContent()
            };
        }
    }
}
