#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;
public class Association
{
    public int AssociationId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // foreign keys
    public int UserId { get; set; }
    public int WeddingId { get; set; }
    

    // classes below
    public User? User { get; set; }
    public Wedding? Wedding { get; set; }
}

