﻿namespace ToDo.Domain.Entities
{
    public class Tasks : Entity
    {
        public Tasks(string name, string description, Guid idUser)
        {
            Name = name;
            Description = description;
            IdUser = idUser;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid IdUser { get; private set; }
        public User User { get; private set; }
    }
}
