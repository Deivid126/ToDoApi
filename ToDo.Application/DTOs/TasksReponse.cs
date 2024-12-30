namespace ToDo.Application.DTOs
{
    public class TasksReponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid IdUser { get; set; }
    }
}
