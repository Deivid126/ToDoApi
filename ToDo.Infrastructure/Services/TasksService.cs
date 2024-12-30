using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TasksService(ITasksRepository repository, IMapper mapper, IUserRepository userRepositoryl)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepositoryl;
        }

        public async Task<TasksResponse> Create(TasksRequest task)
        {
            var user = await _userRepository.GetActive(task.IdUser) ?? throw new KeyNotFoundException($"User with ID {task.IdUser} not found.");
            var newTask = new Tasks(task.Name, task.Description, task.IdUser);
            newTask.UpdateCreateDate(null);
            await _repository.Create(newTask);
            return _mapper.Map<TasksResponse>(await _repository.GetActive(task.Id) ?? throw new KeyNotFoundException($"Task with ID {task.Id} not found."));
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<TasksResponse>> GetAll(Guid idUser)
        {
            var user = await _userRepository.GetActive(idUser) ?? throw new KeyNotFoundException($"Task's with ID {idUser} not found.");
            return _mapper.Map<IEnumerable<TasksResponse>>(_repository.GetAllByUser(idUser));
        }

        public async Task<TasksResponse> GetTask(Guid id)
        {
            var task = await _repository.GetActive(id) ?? throw new KeyNotFoundException($"Task with ID {id} not found.");
            return _mapper.Map<TasksResponse>(task);
        }

        public async Task<TasksResponse> Update(TasksRequest task)
        {
            task.IsEdit = true;
            var user = await _userRepository.GetActive(task.IdUser) ?? throw new KeyNotFoundException($"User with ID {task.IdUser} not found.");
            var oldTask = await _repository.GetActive(task.Id) ?? throw new KeyNotFoundException($"Task with ID {task.Id} not found.");
            var newTask = new Tasks(task.Name, task.Description, task.IdUser);
            newTask.UpdateId(task.Id);
            newTask.UpdateDateEdit();
            newTask.UpdateCreateDate(oldTask.CreateDate);
            if( await _repository.Update(newTask))
                return _mapper.Map<TasksResponse>(await _repository.GetActive(task.Id) ?? throw new KeyNotFoundException($"Task with ID {task.Id} not found."));
            throw new InvalidOperationException($"Erro for update.");
        }
    }
}
