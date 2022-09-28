using System.ComponentModel.DataAnnotations;
namespace VisitAlbania.Models;
#pragma warning disable CS8618
using System.ComponentModel;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


public class Place
{
    [Key]
    public int PlaceId { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public string PlaceName { get; set; }
    [Required]
    public string PlaceDescription { get; set; }
    [Required]
    public string PlaceType { get; set; }
    [Required]
    [Display(Name = "Search in google map the location and copy paste the link.")]
    public string Location { get; set; }
    
    [Display(Name = "Choose the cover photo of your place.")]
    public string? Myimage { get;set; }
    [NotMapped]
    public IFormFile Image { get; set; }
    public User? Creator { get; set; }
    public List<Like>? Likes = new List<Like>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}