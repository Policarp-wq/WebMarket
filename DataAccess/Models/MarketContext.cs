using Microsoft.EntityFrameworkCore;

namespace WebMarket.DataAccess.Models;

public partial class MarketContext : DbContext
{
    public MarketContext()
    {
    }

    public MarketContext(DbContextOptions<MarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Special> Specials { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ShoppingCartElement> ShoppingCartElements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Tag, "categories_tag_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tag)
                .HasMaxLength(100)
                .HasColumnName("tag");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("products_category_id_fkey");


        });

        modelBuilder.Entity<Special>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specials_pkey");

            entity.ToTable("specials");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Specials)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("specials_product_id_fkey");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("storage_pkey");

            entity.ToTable("storage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasDefaultValue(0)
                .HasColumnName("amount");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Storages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("storage_product_id_fkey");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transaction_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("transactions_product_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("transactions_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(75)
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasColumnName("password_hash");
            entity.Property(e => e.Wallet)
                .HasDefaultValueSql("0")
                .HasColumnName("wallet");
        });

        modelBuilder.Entity<ShoppingCartElement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shopping_cart_elements_pkey");

            entity.ToTable("shopping_cart_elements");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.ProductAmount)
                .HasColumnName("product_amount");

            entity.HasOne(entity => entity.User)
                .WithMany(u => u.ShoppingCartElements)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(entity => entity.Product)
                .WithMany(p => p.ShoppingCartElements)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(entity => entity.Id).HasName("reviews_pkey");
            entity.ToTable("reviews");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.ReviewContent)
                .HasColumnName("review_content");
            entity.Property(e => e.SetRating)
                .HasColumnName("set_rating");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");


        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
