using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using weddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace weddingPlanner.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        HttpContext.Session.Clear();
        return View();
    }

    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            if (_context.Users.Any(a => a.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email Already In Use");
                return View("Index");
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("user", newUser.UserId);
            return RedirectToAction("dashboard");
        }
        else
        {
            return View("Index");
        }
    }

    [HttpPost("login")]
    public IActionResult Login(LogUser logUser)
    {
        if (ModelState.IsValid)
        {
            User userInDb = _context.Users.FirstOrDefault(a => a.Email == logUser.LogEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("LogEmail", "Invalid Login");
                return View("Index");
            }
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            var result = hasher.VerifyHashedPassword(logUser, userInDb.Password, logUser.LogPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid Login");
                return View("Index");
            }
            else
            {
                HttpContext.Session.SetInt32("user", userInDb.UserId);
                return RedirectToAction("dashboard");
            }
        }
        else
        {
            return View("Index");
        }
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        if(HttpContext.Session.GetInt32("user")==null){
            return RedirectToAction("index");
        }
        ViewBag.allWeddings=_context.Weddings.Include(a=>a.Attendees).ThenInclude(b=>b.User).ToList();
        return View("Dashboard");
    }


    [HttpGet("newWedding")]
    public IActionResult NewWedding()
    {
        if(HttpContext.Session.GetInt32("user")==null){
            return RedirectToAction("index");
        }
        return View("WeddingForm");
    }

    [HttpPost("addWedding")]
    public IActionResult AddWedding(Wedding newWedding)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        else
        {
            return View("WeddingForm");
        }
    }

    [HttpGet("/details/{id}")]
    public IActionResult Details(int id)
    {
        ViewBag.OneWedding=_context.Weddings.Include(a=>a.Attendees).ThenInclude(b=>b.User).FirstOrDefault(a=>a.WeddingId==id);
        return View();
    }

    [HttpGet("/rsvp/{id}")]
    public IActionResult Rsvp(int id )
    {
        GuestList newList=new GuestList();
        newList.UserId=(int)HttpContext.Session.GetInt32("user");
        newList.WeddingId=id;
        _context.Add(newList);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    [HttpGet("/unrsvp/{id}")]
    public IActionResult UnRsvp(int id)
    {
        GuestList removeUser=_context.GuestLists.FirstOrDefault(a=>a.WeddingId==id && a.UserId==HttpContext.Session.GetInt32("user"));
        _context.GuestLists.Remove(removeUser);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
