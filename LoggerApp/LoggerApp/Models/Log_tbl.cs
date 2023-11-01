namespace LoggerApp.Models
{
    public class Log_tbl
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Module { get; set; }
        public string HostName { get; set; }
        public string ipaddress { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set;}
        public string remarks { get; set;}
    }
}
