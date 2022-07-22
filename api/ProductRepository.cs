public static class ProductRepository{
    public static  List<Product> Products {get; set;} = Products = new List<Product>();


    public static void Init(IConfiguration configuration){
       var products = configuration. GetSection("Products").Get<List<Product>>();
       Products = products;
    }

    public static void Add(Product product) {
        Products.Add(product);    
    } 

    //GetByCode
    public static Product GetBy(string id){
        return Products.FirstOrDefault(p => p.Code == id);
    }

    public static void Remove(Product product){
        Products.Remove(product);
    }



}
