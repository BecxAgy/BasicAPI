using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);


app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context) => {
    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    var produto = new Product {
        Code = productRequest.Code,
        Nome = productRequest.Nome,
        Description = productRequest.Description,
        Categories = category
    };

    if(productRequest.Tags != null){
        produto.Tags = new List<Tag>();

        foreach(var item in productRequest.Tags){
            produto.Tags.Add(new Tag{Name = item});
        }
    }
    context.Products.Add(produto);
    context.SaveChanges();
    return Results.Created($"/products/{produto.Id}", produto.Id);

    });


//api.app.com/user/{code}
app.MapGet("/products/{id}",([FromRoute] int id, ApplicationDbContext context) => {
    var produto = context.Products
    .Include(p => p.Categories)
    .Include(p => p.Tags)
    .Where(p => p.Id == id);
    
    if(produto.Count() > 0){
        return Results.Ok(produto.First());
    }else{
        return Results.NotFound();
    }
    });

app.MapPut("/products/{id}", ([FromRoute]int id, ProductRequest productRequest, ApplicationDbContext context) => {
    var produto = context.Products
    .Include(p => p.Categories)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    
    produto.Code = productRequest.Code;
    produto.Nome = productRequest.Nome;
    produto.Description = productRequest.Description;
    produto.Categories = category;
    
    if(productRequest.Tags != null){
        produto.Tags = new List<Tag>();

        foreach(var item in productRequest.Tags){
            produto.Tags.Add(new Tag{Name = item});
        }
    }

    context.SaveChanges();
    return Results.Ok();
    
});

app.MapDelete("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) => {
    var produto = context.Products.Where(p => p.Id == id).First();
    context.Remove(produto);
    context.SaveChanges();
    return Results.Ok();
});

if(app.Environment.IsStaging())
    app.MapGet("/configuration/database", (IConfiguration configuration) => {
    return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
    });


app.Run();
