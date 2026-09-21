using clothing_be.Models.customers;
using clothing_be.Models.customers.Address;
using clothing_be.Models.Others;
using clothing_be.Models.productions;
using clothing_be.Models.productions.Activity;
using clothing_be.Models.productions.Tagging;
using clothing_be.Models.productions.Varied;
using clothing_be.Models.trading;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.X509Certificates;

namespace clothing_be.Data
{
    public class MyDbContextApplication : DbContext
    {
        //public MyDbContextApplication() { }
        
        //public MyDbContextApplication() { }
        public MyDbContextApplication(DbContextOptions<MyDbContextApplication> options) : base(options) { }
        #region
        //products
        public DbSet<BannerModel> Banners { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<ImageGalleryModel> ImageGalleries { get; set; }
        public DbSet<DiscountModel> Discounts { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubCategoryModel> SubCategories { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<ProductTagModel> ProductTags { get; set; }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<VariationModel> Variations { get; set; }
        public DbSet<VariantAttributeValuesModel> VariantAttributeValues { get; set; }
        public DbSet<AttributeValuesModel> AttributeValues { get; set; }
        public DbSet<AttributeModel> Attributes { get; set; }

        //user
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<DistrictModel> Districts { get; set; }

        //action
        public DbSet<FavoritedModel> Favoriteds { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<ActivityLogModel> ActivityLogs { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }

        //payment
        public DbSet<PaymentMethodModel> PaymentMethods { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemsModel> OrderItems { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemsModel> CartItems { get; set; }
        public DbSet<InvoiceModel> Invoices { get; set; }
        public DbSet<InvoiceItemsModel> InvoiceItems { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //// Quan hệ 1: Restrict (không cho xóa User nếu còn log)
            //// Quan hệ 2: Cascade (xóa Product thì xóa luôn gallery)
            //ADDRESS

            modelBuilder.Entity<AddressModel>()
                .HasOne(c => c.User)
                .WithMany( c => c.Addresses)
                .HasForeignKey( c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddressModel>()
                .HasOne(c => c.City)
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddressModel>()
                .HasOne(c => c.District)
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddressModel>()
                .HasOne(c => c.Ward)
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.WardId)
                .OnDelete(DeleteBehavior.Restrict);

            //OTHERS

            modelBuilder.Entity<ActivityLogModel>()
                .HasOne(c => c.User)
                .WithMany(c => c.ActivityLogs)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ImageGalleryModel>()
                .HasIndex(g => new { g.ProductId, g.ImageId })
                .IsUnique();

            modelBuilder.Entity<ImageGalleryModel>()
                .HasOne(c => c.Image)
                .WithMany(c => c.ImageGalleries)
                .HasForeignKey(c => c.ImageId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ImageGalleryModel>()
                .HasOne(c => c.Product)
                .WithMany(c => c.ImageGalleries)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //ACTIVITIES
            modelBuilder.Entity<CommentModel>()
               .HasIndex(g => new { g.UserId, g.ProductId })
               .IsUnique();

            modelBuilder.Entity<CommentModel>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommentModel>()
                .HasOne( x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<FavoritedModel>()
               .HasIndex(g => new { g.UserId, g.ProductId })
               .IsUnique();

            modelBuilder.Entity<FavoritedModel>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoritedModel>()
                .HasOne(x => x.User)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //TAGGING
            modelBuilder.Entity<ProductTagModel>()
                .HasIndex(g => new { g.ProductId, g.TagId })
                .IsUnique();

            modelBuilder.Entity<ProductTagModel>()
                .HasOne(t => t.Product)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductTagModel>()
                .HasOne(t => t.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(t => t.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            //CATEGORIES
            modelBuilder.Entity<CategoryModel>()
                .HasOne(t => t.Image)
                .WithMany()
                .HasForeignKey(t => t.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SubCategoryModel>()
                .HasOne(t => t.Category)
                .WithMany(t => t.SubCategories)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //PRODUCT
            modelBuilder.Entity<ProductModel>()
                .HasOne(t => t.Discount)
                .WithMany(t => t.Products)
                .HasForeignKey(t => t.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProductModel>()
                .HasOne(t => t.SubCategory)
                .WithMany(t => t.Products)
                .HasForeignKey(t => t.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //VARIANT
            modelBuilder.Entity<VariationModel>()
                .HasOne(v => v.Product)
                .WithMany( v => v.Variations)
                .HasForeignKey( v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VariationModel>()
                .HasOne(v => v.Image)
                .WithMany()
                .HasForeignKey(v => v.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VariantAttributeValuesModel>()
                .HasIndex(q => new { q.VariationId, q.AttributeValueId })
                .IsUnique();

            modelBuilder.Entity<VariantAttributeValuesModel>()
                .HasOne( n => n.Variation)
                .WithMany( n => n.VariantAttributeValues)
                .HasForeignKey( n => n.VariationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VariantAttributeValuesModel>()
                .HasOne(n => n.AttributeValue)
                .WithMany(n => n.VariantAttributeValues)
                .HasForeignKey(n => n.AttributeValueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttributeValuesModel>()
                .HasOne(n => n.Attribute)
                .WithMany(n => n.AttributeValues)
                .HasForeignKey(n => n.AttributeId)
                .OnDelete(DeleteBehavior.Restrict);

            //Cart
            modelBuilder.Entity<CartModel>()
                .HasIndex(g => new { g.UserId })
                .IsUnique();

            modelBuilder.Entity<CartModel>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemsModel>()
                .HasIndex(g => new { g.VariationId, g.CartId })
                .IsUnique();
            
            modelBuilder.Entity<CartItemsModel>()
                .HasOne(r => r.Cart)
                .WithMany( r => r.CartItems)
                .HasForeignKey(r => r.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItemsModel>()
                .HasOne(r => r.Variation)
                .WithMany(r => r.CartItems)
                .HasForeignKey(r => r.VariationId)
                .OnDelete(DeleteBehavior.Restrict);

            //ORDER
            modelBuilder.Entity<OrderModel>()
                .HasOne(r => r.Address)
                .WithMany(r => r.Orders)
                .HasForeignKey(r => r.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderModel>()
                .HasOne(r => r.PaymentMethod)
                .WithMany(r => r.Orders)
                .HasForeignKey(r => r.MethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderModel>()
                .HasOne(r => r.User)
                .WithMany(r => r.Orders)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItemsModel>()
                .HasIndex(g => new { g.OrderId, g.VariationId })
                .IsUnique();

            modelBuilder.Entity<OrderItemsModel>()
                .HasOne(r => r.Order)
                .WithMany(r => r.OrderItems)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItemsModel>()
                .HasOne(r => r.Variation)
                .WithMany(r => r.OrderItems)
                .HasForeignKey(r => r.VariationId)
                .OnDelete(DeleteBehavior.Restrict);

            //Invoice

            modelBuilder.Entity<InvoiceModel>()
                .HasOne(r => r.Address)
                .WithMany(r => r.Invoices)
                .HasForeignKey(r => r.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceModel>()
                .HasOne(r => r.PaymentMethod)
                .WithMany(r => r.Invoices)
                .HasForeignKey(r => r.MethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceModel>()
                .HasOne(r => r.User)
                .WithMany(r => r.Invoices)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceItemsModel>()
                .HasIndex(g => new { g.InvoiceId, g.VariationId })
                .IsUnique();

            modelBuilder.Entity<InvoiceItemsModel>()
                .HasOne(r => r.Invoice)
                .WithMany(r => r.InvoiceItems)
                .HasForeignKey(r => r.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceItemsModel>()
                .HasOne(r => r.Variation)
                .WithMany(r => r.InvoiceItems)
                .HasForeignKey(r => r.VariationId)
                .OnDelete(DeleteBehavior.Restrict);

            //Banner
            modelBuilder.Entity<BannerModel>()
               .HasIndex(g => new { g.Id, g.ImageId })
               .IsUnique();
            
            modelBuilder.Entity<BannerModel>()
                .HasOne(r => r.Image)
                .WithMany()
                .HasForeignKey(r => r.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    
    }
}
