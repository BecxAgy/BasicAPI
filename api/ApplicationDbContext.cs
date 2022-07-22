using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext{
    public DbSet<Product> Products {get;set;} //Transformando product em uma tabela
    public DbSet<Category> Categories {get; set;} //abilitando o acesso a tabela

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder builder){
        builder.Entity<Product>()
        .Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        builder.Entity<Product>()
        .Property(p => p.Nome).HasMaxLength(120).IsRequired();
        builder.Entity<Product>()
        .Property(p => p.Code).HasMaxLength(50).IsRequired();
        builder.Entity<Category>()
        .ToTable("Categories");
    }


    
}
