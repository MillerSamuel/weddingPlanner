using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace weddingPlanner.Models;

public class GuestList
{
    [Key]
    public int GuestListId {get;set;}

    public int UserId {get;set;}
    public User? User {get;set;}

    public int WeddingId {get;set;}
    public Wedding? Wedding {get;set;}
}
