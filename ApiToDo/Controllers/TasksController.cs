using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;

namespace ToDo.Api.Controllers
{
    [Authorize]
    public class TasksController : ApiController
    {
        private readonly ITasksService _serviceTasks;
        private readonly ITokenService _tokenService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITasksService serviceTasks, ITokenService tokenService, ILogger<TasksController> logger)
        {
            _serviceTasks = serviceTasks;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("[action]", Name = "CreateTask")]
        public async Task<IActionResult> CreateTask(TasksRequest task)
        {
            try
            {
                _tokenService.VerifyUserTokenIsEqualsUserRequest(User, task.IdUser);
                var newTask = await _serviceTasks.Create(task);
                return CustomResponse((int)HttpStatusCode.Created, true, newTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), task);
                return CustomResponse((int)HttpStatusCode.BadRequest, false, ex.Message.ToString());
            }
        }

        [HttpPut("[action]/{id}", Name = "UpdateTaskById")]
        public async Task<IActionResult> UpdateTaskById(TasksRequestEdit taskEdit, Guid id) 
        {
            try
            {
                var task = new TasksRequest { Name = taskEdit.Name, Id = id, IdUser = taskEdit.IdUser, Description = taskEdit.Description, IsEdit = true};
                _tokenService.VerifyUserTokenIsEqualsUserRequest(User, task.IdUser);
                var taskAtt = await _serviceTasks.Update(task);
                return CustomResponse((int)HttpStatusCode.OK, true, taskAtt);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), taskEdit);
                return CustomResponse((int)HttpStatusCode.BadRequest, false, ex.Message.ToString());
            }

        }

        [HttpDelete("[action]/{id}", Name = "DeleteTaskById")]
        public async Task<IActionResult> DeleteTaskById(Guid id) 
        {
            try
            {
                var user = await _serviceTasks.GetTask(id);
                _tokenService.VerifyUserTokenIsEqualsUserRequest(User, user.IdUser);
                await _serviceTasks.Delete(id);
                return CustomResponse((int)HttpStatusCode.NoContent, true);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message.ToString(), id);
                return CustomResponse((int)HttpStatusCode.BadRequest,false, ex.Message.ToString());
            }
            
        }

        [HttpGet("[action]/{id}", Name = "GetTaskById")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            try
            {
                
                var response = await _serviceTasks.GetTask(id);
                _tokenService.VerifyUserTokenIsEqualsUserRequest(User, response.IdUser);
                return CustomResponse((int)HttpStatusCode.OK, true, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), id);
                return CustomResponse((int)HttpStatusCode.BadRequest, false, ex.Message.ToString());
            }
        }

        [HttpGet("[action]/{idUser}", Name = "GetAllByUser")]
        public async Task<IActionResult> GetAllByUser(Guid idUser)
        {
            try
            {
                _tokenService.VerifyUserTokenIsEqualsUserRequest(User, idUser);
                var response = await _serviceTasks.GetAll(idUser);
                if (response == null)
                    return CustomResponse((int)HttpStatusCode.NotFound, false, new List<TasksResponse>());
                return CustomResponse((int)HttpStatusCode.OK, true, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), idUser);
                return CustomResponse((int)HttpStatusCode.BadRequest, false, ex.Message.ToString());
            }
        }
    }
}