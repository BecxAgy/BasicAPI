public class Product {
    public int Id {get;set;}
    public string Code {get;set;}
    public string Nome {get;set;}
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Categories {get;set;}
    public List<Tag> Tags {get;set;}
     
}
