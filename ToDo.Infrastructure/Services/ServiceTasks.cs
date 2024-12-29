using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Services
{
    public class ServiceTasks : IServiceTasks
    {
        private readonly ITasksRepository _repository;
        private readonly IUserRepository _userRepositoryl;
        private readonly IMapper _mapper;

        public ServiceTasks(ITasksRepository repository, IMapper mapper, IUserRepository userRepositoryl)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepositoryl = userRepositoryl;
        }

        public async Task<bool> Create(TasksRequest task)
        {
            var user = _userRepositoryl.Get(task.IdUser) ?? throw new ValidationException("User not exist");
            var newTask = new Tasks(task.Name, task.Description, task.IdUser);
            return await _repository.Create(newTask);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        public IEnumerable<TasksReponse> GetAll(Guid idUser)
        {
            var user = _userRepositoryl.Get(idUser) ?? throw new ValidationException("User not exist");
            return _mapper.Map<IEnumerable<TasksReponse>>(_repository.GetAllByUser(idUser));
        }

        public async Task<TasksReponse> GetTask(Guid id)
        {
            return _mapper.Map<TasksReponse>(await _repository.Get(id));
        }

        public async Task<bool> Update(TasksRequest task)
        {
            var user = _userRepositoryl.Get(task.IdUser) ?? throw new ValidationException("User not exist");
            var newTask = new Tasks(task.Name, task.Description, task.IdUser);
            return await _repository.Update(newTask);
        }
    }
}
