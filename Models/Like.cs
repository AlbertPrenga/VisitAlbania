using System.ComponentModel.DataAnnotations;
namespace VisitAlbania.Models;
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;

public class Like

{
    [Key]
    public int LikeId {get; set;}
    public int UserId {get; set;}
    public int PlaceId {get; set;}
    public Place? PlaceLiked {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}