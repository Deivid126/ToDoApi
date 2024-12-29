namespace ToDo.Domain.Entities
{
    public class Tasks : Entity
    {
        public Tasks(string name, string description, Guid idUSer)
        {
            Name = name;
            Description = description;
            IdUser = idUSer;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public Guid IdUser { get; private set; }
        public User User { get; private set; }

        public void UpdateActive(bool status) 
        {
            Active = status;
        }

    }
}
