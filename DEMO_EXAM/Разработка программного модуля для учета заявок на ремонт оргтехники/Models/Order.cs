public class Order
{
    public Order(int num, string tech_type, string model, string description, string client_name, string status, string master, string num_tel, DateTime? checkInDate = null, DateTime? checkOutDate = null)
    {
        Num = num;
        Tech_type = tech_type;
        Model = model;
        Description = description;
        Client_name = client_name;
        Status = status;
        Master = master;
        Num_tel = num_tel;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
    }

    public int Num { get; set; }
    public string Tech_type { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string Client_name { get; set; }
    public string Status { get; set; }
    public string Master { get; set; }
    public string Num_tel { get; set; }
    public string? MasterComment { get; set; }
    public DateTime? CheckInDate { get; set; } 
    public DateTime? CheckOutDate { get; set; }
}

public class OrderUpdateDTO
{
    public string? Status { get; set; }
    public string? Description { get; set; }
    public string? Master { get; set; }
    public string? MasterComment { get; set; }
}
