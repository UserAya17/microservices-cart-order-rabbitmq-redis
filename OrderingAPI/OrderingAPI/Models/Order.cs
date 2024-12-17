namespace OrderingAPI.Models
{
    public class Order
    {
        public int Id { get; set; }             // Identifiant unique de la commande
        public string UserName { get; set; }    // Nom ou identifiant de l'utilisateur ayant passé la commande
        public decimal TotalPrice { get; set; } // Montant total de la commande
        public DateTime OrderDate { get; set; } // Date de la commande
        public string Status { get; set; }      // Statut de la commande (exemple : "On Shipping", "Delivered")
        public string Address { get; set; }     // Adresse de livraison
        public string PaymentMethod { get; set; } // Méthode de paiement utilisée
        public List<OrderItem> Items { get; set; } // Liste des articles dans la commande
    }


    public class OrderItem
    {
        public int Id { get; set; }                // Identifiant unique de l'article
        public string ProductName { get; set; }    // Nom du produit
        public decimal Price { get; set; }         // Prix unitaire du produit
        public int Quantity { get; set; }          // Quantité de ce produit dans la commande
        public string ImageUrl { get; set; }       // URL de l'image du produit
        public int OrderId { get; set; }           // Clé étrangère pour associer à une commande
        public Order Order { get; set; }           // Relation avec la commande parent
    }

}
