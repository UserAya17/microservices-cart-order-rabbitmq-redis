using Microsoft.EntityFrameworkCore;
using OrderingAPI.Models;

namespace OrderingAPI.Data
{
    public class OrderingContext : DbContext
    {
        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurer la relation entre Order et OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items) // Une commande a plusieurs articles
                .WithOne(i => i.Order) // Chaque article appartient à une commande
                .HasForeignKey(i => i.OrderId) // Clé étrangère
                .OnDelete(DeleteBehavior.Cascade); // Supprimer les articles si la commande est supprimée

            // Configurer le type de données pour les colonnes décimales
            modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(i => i.Price).HasColumnType("decimal(18,2)");
        }
    }
}
