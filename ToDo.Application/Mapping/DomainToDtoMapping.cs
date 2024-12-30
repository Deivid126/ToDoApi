using AutoMapper;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Mapping
{
    public class DomainToDtoMapping : Profile
    {
        public DomainToDtoMapping() 
        {
            CreateMap<User, UserResponse>()
                .ConstructUsing(src => new UserResponse
                {
                    Id = src.Id,
                    Email = src.Email,
                    Name = src.Name
                });
            CreateMap<Tasks, TasksResponse>()
                .ConstructUsing(src => new TasksResponse
                {
                    Id = src.Id,
                    Name = src.Name,
                    Description = src.Description,
                    IdUser = src.IdUser,
                });
        }
    }
}
