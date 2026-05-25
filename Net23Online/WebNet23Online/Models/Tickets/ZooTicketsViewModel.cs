namespace WebNet23Online.Models.Tickets
{
    public class ZooTicketsViewModel
    {
        public string UniqueKey { get; set; }
        public string ZooName { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
