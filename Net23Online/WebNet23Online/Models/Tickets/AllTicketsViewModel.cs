namespace WebNet23Online.Models.Tickets
{
    public class AllTicketsViewModel
    {
        public bool CanShowZooTickets { get; set; }
        public List<ZooTicketsViewModel> ZooTickets { get; set; }
    }
}
