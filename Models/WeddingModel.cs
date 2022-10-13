#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage = "must be at least 2 characters.")]
    [Display(Name = "Wedder One")]
    public string WedderOne { get; set; }
    

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage = "must be at least 2 characters.")]
    [Display(Name = "Wedder Two")]
    public string WedderTwo { get; set; }


    [Required(ErrorMessage = "is required")]
    [DataType(DataType.DateTime)]
    [FutureDate]
    public DateTime? Date { get; set; }


    [Required(ErrorMessage = "is required")]
    [MinLength(8, ErrorMessage = "must be at least 8 characters")]
    [Display(Name = "Wedding Address")]
    public string WeddingAddress { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public int UserId { get; set; }
    public User? WeddingCreator { get; set; }
    


    public List<Association> WeddingAssociation {get; set; } = new List <Association> ();
}