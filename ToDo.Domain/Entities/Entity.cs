namespace ToDo.Domain.Entities
{
    public class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
            Active = true;
        }

        public Guid Id { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public DateTime? DeleteDate { get; private set; }
        public bool Active { get; private set; }

        public void UpdateId(Guid id)
        {
            Id = id;
        }

        public void UpdateDateEdit()
        {
            UpdateDate = DateTime.UtcNow;
        }

        public void UpdateActive(bool status)
        {
            Active = status;
        }

        public void UpdateDeleteDate()
        {
            DeleteDate = DateTime.UtcNow;
        }

        public void UpdateCreateDate(DateTime? dateCreate) 
        {
            CreateDate = dateCreate ?? DateTime.UtcNow;
        }
    }
}
