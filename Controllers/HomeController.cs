using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VisitAlbania.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;




namespace VisitAlbania.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment WebHostEnvironment;

    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _context = context;
        WebHostEnvironment = webHostEnvironment;

    }
//Index is the main page
    public IActionResult Index()
    {
                
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Register");
        }
 
    int id = (int)HttpContext.Session.GetInt32("userId");
    ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
    ViewBag.Favorites = _context.Places.Include(e => e.Likes).OrderByDescending(e => e.Likes.Count()).ToList();
        return View();
    }
// shows the page where you will put data in the form to create a place
    [HttpGet("AddPlace")]
    public IActionResult AddPlace()
    {
        
        return View();

    }
    // post method for creating places. When you type subbmit will be executing this method creating a new form
    [HttpPost("PlaceCreate")]
    public IActionResult PlaceCreate(Place marrNgaView)
    {
        if (ModelState.IsValid)
        {
//    string StringFileName = UploadFile(marrNgaView);
            int id = (int)HttpContext.Session.GetInt32("userId");

            if (_context.Places.Any(u => u.PlaceName == marrNgaView.PlaceName))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("PlaceName", "This Place is already created!");
                return View("AddPlace");
                // You may consider returning to the View at this point
            }
            string StringFileName = UploadFile(marrNgaView);
            
            var newPlace = new Place ()
            {
                PlaceName = marrNgaView.PlaceName,
                PlaceDescription =marrNgaView.PlaceDescription,
                PlaceType = marrNgaView.PlaceType,
                Location = marrNgaView.Location,
                Myimage = StringFileName
              
            };
            
            newPlace.UserId = id;
            _context.Places.Add(newPlace);
            _context.SaveChanges();
           
           if(newPlace.PlaceType == "Mountains"){
            return RedirectToAction("Mountains");
            }
            else if (newPlace.PlaceType == "Beaches")
            {
                return RedirectToAction("Beaches");
            }
            else
            {
                return RedirectToAction("Culture");
            }
            
        }
        return View("AddPlace");
    }
    // shows the edit form page
    [HttpGet("EditPlace/{id}")]
    public IActionResult EditPlace(int id)
    {
        Place OldPlace = _context.Places.First(e => e.PlaceId == id);
        return View(OldPlace);
    }
    //post after editing the place
    [HttpPost("Update/{id}")]
    public IActionResult Update(int id, Place EditPlace)
    {
        string StringFileName = UploadFile(EditPlace);
        Place OldPlace = _context.Places.First(e => e.PlaceId == id);
        OldPlace.PlaceName = EditPlace.PlaceName;
        OldPlace.PlaceDescription = EditPlace.PlaceDescription;
        OldPlace.PlaceType = EditPlace.PlaceType;
        OldPlace.Location = EditPlace.Location;
        OldPlace.Myimage = OldPlace.Myimage;
        _context.SaveChanges();
        if (EditPlace.PlaceType == "Mountains")
        {
        return RedirectToAction("Mountains");
        }
        else if (EditPlace.PlaceType == "Beaches")
        {
            return RedirectToAction ("Beaches");
        }
        else if (EditPlace.PlaceType == "Culutre")
        {
            return RedirectToAction ("Culture");
        }
        else
        {
            return RedirectToAction("Favorites");
        }

    }

    //delte places function
    [HttpGet("PlaceDelete/{id}")]
    public IActionResult Delete(int id)
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Register");
        }
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");    
        Place removePlace = _context.Places.First(e => e.PlaceId == id);
        _context.Places.Remove(removePlace);
        _context.SaveChanges();
        if (removePlace.PlaceType == "Mountains")
        {
        return RedirectToAction("Mountains");
        }
        else if (removePlace.PlaceType == "Beaches")
        {
            return RedirectToAction ("Beaches");
        }
        else if (removePlace.PlaceType == "Culutre")
        {
            return RedirectToAction ("Culture");
        }
        else
        {
            return RedirectToAction("Favorites");
        }
    }
    //function for uploading pictures
    public string UploadFile(Place marrNgaView)
    {
       string fileName = null;
       if(marrNgaView.Image != null)
       {
        string Uploaddir = Path.Combine(WebHostEnvironment.WebRootPath,"Images");
        fileName = Guid.NewGuid().ToString() + "-" + marrNgaView.Image.FileName;
        string filePath = Path.Combine(Uploaddir,fileName);
        
        using (var filestream = new FileStream(filePath,FileMode.Create))
        {
                marrNgaView.Image.CopyTo(filestream);
        }
       }
       return fileName;
    }
    [HttpGet("Mountains")]
    public IActionResult Mountains()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Register");
        }
        int id = (int)HttpContext.Session.GetInt32("userId");
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
        ViewBag.Places = _context.Places.Include(e => e.Creator).Include(e => e.Likes)
        .OrderByDescending(e => e.CreatedAt).Where(e => e.PlaceType == "Mountains").ToList();
        return View();
    }
       //Function that shows Beaches page with all elements
    [HttpGet("Beaches")]
    public IActionResult Beaches()
    
    {   
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Register");
        }
        int id = (int)HttpContext.Session.GetInt32("userId");
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
        ViewBag.Places2 = _context.Places.Include(e => e.Creator).Include(e => e.Likes)
        .OrderByDescending(e => e.CreatedAt).Where(e => e.PlaceType == "Beaches").ToList();
        return View();
    }
    //Function that shows Culture page with all elements
    [HttpGet("Culture")]
    public IActionResult Culture()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Register");
        }
        int id = (int)HttpContext.Session.GetInt32("userId");
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
        ViewBag.Places3 = _context.Places.Include(e => e.Creator).Include(e => e.Likes)
        .OrderByDescending(e => e.CreatedAt).Where(e => e.PlaceType == "Culture").ToList();
        return View();
    }
    [HttpGet("Favorites")]
    public IActionResult Favorites()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Register");
        }
    int id = (int)HttpContext.Session.GetInt32("userId");
    ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
    ViewBag.Favorites = _context.Places.Include(e => e.Likes).Include(e => e.Creator).OrderByDescending(e => e.Likes.Count()).Take(3).ToList();
        return View();
    }
// function for like
    [HttpGet("AddLike/{id}")]
    public IActionResult AddLike(int id)
    {
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");
        Like like = new Like()
        {
            UserId = idFromSession,
            PlaceId = id,
        
        };
        
        Place place = _context.Places.First(c=>c.PlaceId == id);
        _context.Likes.Add(like);
        _context.SaveChanges();

        if (place.PlaceType == "Mountains")
        {
        return RedirectToAction("Mountains");
        }
        else if (place.PlaceType == "Beaches")
        {
            return RedirectToAction("Beaches");
        }
        else if (place.PlaceType == "Culture")
        {
            return RedirectToAction("Culture");
        }
        else
        {
            return RedirectToAction("Favorites");
        }
    }
// this function serves to unlike
    [HttpGet("RemoveLike/{id}")]
    public IActionResult RemoveLike(int id)
    {
    
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");
        Like hiqFans = _context.Likes.First(e => e.PlaceId == id && e.UserId == idFromSession);
        _context.Remove(hiqFans);
        _context.SaveChanges();
        Place place = _context.Places.First(c=>c.PlaceId == id);
        if (place.PlaceType == "Mountains")
        {
        return RedirectToAction("Mountains");
        }
        else if (place.PlaceType == "Beaches")
        {
            return RedirectToAction("Beaches");
        }
        else if (place.PlaceType == "Culture")
        {
            return RedirectToAction("Culture");
        }
             else
        {
            return RedirectToAction("Favorites");
        }
    
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {


        if (HttpContext.Session.GetInt32("userId") == null)
        {

            return View();
        }

        return RedirectToAction("Index");

    }
    [HttpPost("Register")]
    public IActionResult Register(User user)
    {
        // Check initial ModelState
        if (ModelState.IsValid)
        {
            // If a User exists with provided email
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("Email", "Email already in use!");

                return View();
                // You may consider returning to the View at this point
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("userId", user.UserId);
           
            return RedirectToAction("Index");
        }
        return View("Register");
    }

    [HttpPost("Login")]
    public IActionResult LoginSubmit(LoginUser userSubmission)
    {
        if (ModelState.IsValid)
        {
            // If initial ModelState is valid, query for a user with provided email
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if (userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Register");
            }

            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();

            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

            // result can be compared to 0 for failure
            if (result == 0)
            {
                ModelState.AddModelError("Password", "Invalid Password");
                return View("Register");
                // handle failure (this should be similar to how "existing email" is handled)
            }
            HttpContext.Session.SetInt32("userId", userInDb.UserId);

            return RedirectToAction("Index");
        }

        return View("Register");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {

        HttpContext.Session.Clear();
        return RedirectToAction("register");
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
