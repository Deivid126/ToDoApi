﻿namespace ToDo.Domain.Entities
{
    public class User : Entity
    {
        public User(string name, string email, string password) 
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool Active { get; private set; }
        private List<Tasks> _tasks = new List<Tasks>();
        public IReadOnlyCollection<Tasks> tasks => _tasks;

        public void SetAcitve(bool status) 
        {
            Active = status;
        }
    }
}