namespace TaskManager.Domain.Domain
{
    public class TarefaModel : BaseDomain
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
