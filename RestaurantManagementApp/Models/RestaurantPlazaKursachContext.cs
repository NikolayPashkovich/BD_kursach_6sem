using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementApp.Models;

public partial class RestaurantPlazaKursachContext : DbContext
{
    public RestaurantPlazaKursachContext()
    {
    }

    public RestaurantPlazaKursachContext(DbContextOptions<RestaurantPlazaKursachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishIngredient> DishIngredients { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Supply> Supplies { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=USER-PC\\SQLEXPRESS;Database=Restaurant_Plaza_Kursach;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8E1EEC315");

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359E872B9326").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.DishId).HasName("PK__Dishes__18834F702BDAF892");

            entity.HasIndex(e => e.DishName, "UQ__Dishes__189C30FDF36D61E5").IsUnique();

            entity.Property(e => e.DishId).HasColumnName("DishID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.DishName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<DishIngredient>(entity =>
        {
            entity.HasKey(e => new { e.DishId, e.IngredientId }).HasName("PK__DishIngr__A369A45708B3BFF5");

            entity.Property(e => e.DishId).HasColumnName("DishID");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Dish).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.DishId)
                .HasConstraintName("FK__DishIngre__DishI__6E01572D");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("FK__DishIngre__Ingre__6EF57B66");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB27ACD68D7AD");

            entity.HasIndex(e => e.IngredientName, "UQ__Ingredie__A1B2F1CCB4AFF114").IsUnique();

            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.CurrentStock)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IngredientName).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(20);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Menus__C99ED2500CC5AD51");

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.MenuName).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasMany(d => d.Dishes).WithMany(p => p.Menus)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuDish",
                    r => r.HasOne<Dish>().WithMany()
                        .HasForeignKey("DishId")
                        .HasConstraintName("FK__MenuDishe__DishI__6B24EA82"),
                    l => l.HasOne<Menu>().WithMany()
                        .HasForeignKey("MenuId")
                        .HasConstraintName("FK__MenuDishe__MenuI__6A30C649"),
                    j =>
                    {
                        j.HasKey("MenuId", "DishId").HasName("PK__MenuDish__D816E6A784CB050B");
                        j.ToTable("MenuDishes");
                        j.IndexerProperty<int>("MenuId").HasColumnName("MenuID");
                        j.IndexerProperty<int>("DishId").HasColumnName("DishID");
                    });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFD4633FA5");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("FreeTable");
                    tb.HasTrigger("ReserveTable");
                });

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Orders__Customer__5DCAEF64");

            entity.HasOne(d => d.Staff).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__StaffID__5EBF139D");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__TableID__5FB337D6");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C36FE5B1B");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("SetOrderDetailsPrice");
                    tb.HasTrigger("UpdateOrderTotal");
                });

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.DishId).HasColumnName("DishID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(20);
            entity.Property(e => e.PriceAtOrder).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

            entity.HasOne(d => d.Dish).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__DishI__1AD3FDA4");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__19DFD96B");

            entity.HasOne(d => d.Promotion).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OrderDeta__Promo__1BC821DD");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42F2FB90C60F8");

            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PromotionName).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasMany(d => d.Dishes).WithMany(p => p.Promotions)
                .UsingEntity<Dictionary<string, object>>(
                    "PromotionDish",
                    r => r.HasOne<Dish>().WithMany()
                        .HasForeignKey("DishId")
                        .HasConstraintName("FK__Promotion__DishI__04E4BC85"),
                    l => l.HasOne<Promotion>().WithMany()
                        .HasForeignKey("PromotionId")
                        .HasConstraintName("FK__Promotion__Promo__03F0984C"),
                    j =>
                    {
                        j.HasKey("PromotionId", "DishId").HasName("PK__Promotio__434C1BD895ADDF20");
                        j.ToTable("PromotionDishes");
                        j.IndexerProperty<int>("PromotionId").HasColumnName("PromotionID");
                        j.IndexerProperty<int>("DishId").HasColumnName("DishID");
                    });
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AE59420E49");

            entity.HasIndex(e => e.OrderId, "UQ__Reviews__C3905BAE1B24D597").IsUnique();

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.Order).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => d.OrderId)
                .HasConstraintName("FK__Reviews__OrderID__6754599E");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF7373C2C8E");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE66694E4334FE0");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.HasKey(e => e.SupplyId).HasName("PK__Supplies__7CDD6C8E700F8BD9");

            entity.ToTable(tb => tb.HasTrigger("UpdateStockAfterSupply"));

            entity.Property(e => e.SupplyId).HasColumnName("SupplyID");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplyDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Supplies__Ingred__76969D2E");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Supplies__Suppli__75A278F5");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__Tables__7D5F018EA09DAEEC");

            entity.HasIndex(e => e.TableNumber, "UQ__Tables__E8E0DB52033B499B").IsUnique();

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.TableNumber).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
