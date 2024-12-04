namespace Разработка_программного_модуля_для_учета_заявок_на_ремонт_оргтехники.Models
{
    public class OrderUpdateDTO
    {
        public string? Status { get; set; }
        public string? Description { get; set; } 
        public string? Master { get; set; }
        public string MasterComment { get; set; }
    }
}
