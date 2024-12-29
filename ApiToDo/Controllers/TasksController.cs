using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Contracts.Services;

namespace ToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        public readonly IServiceTasks _serviceTasks;

        public TasksController(IServiceTasks serviceTasks)
        {
            _serviceTasks = serviceTasks;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTaskById(Guid id)
        {
            try
            {
                var task = await _serviceTasks.GetTask(id);
                return Ok();
            }
            catch (Exception ex)
            {
                {
                    return NotFound(ex);
                }
            }
        }
    }
}
