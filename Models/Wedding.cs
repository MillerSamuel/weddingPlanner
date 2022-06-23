using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace weddingPlanner.Models;

// public class DateValidation : ValidationAttribute
// {
//     protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//     {
//         var dateNow = DateOnly.FromDateTime(DateTime.Now);
//         System.Console.WriteLine(dateNow);
//         System.Console.WriteLine(value);
//         if (value==null){
//             return ValidationResult.Success;
//         }

//         if ((DateOnly)value<dateNow )
//             return new ValidationResult("Date must be in the Future!");
//         return ValidationResult.Success;
//     }
// }


public class Wedding
{
    [Key]
    public int WeddingId{get;set;}

    [Required]
    public string WedderOne{get;set;}
    [Required]
    public string WedderTwo{get;set;}
    
    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date {get;set;}

    [Required]
    public string Adress{get;set;}

    public List<GuestList> Attendees {get;set;} =new List<GuestList>();
    
    public DateTime CreatedAt {get;set;}=DateTime.Now;
    public DateTime UpdatedAt {get;set;}=DateTime.Now;
}