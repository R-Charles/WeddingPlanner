// Using statements
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers;
    
public class UserController : Controller
{
    private WPContext db;

    //? tests whether users can login or not
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }

    //? see if someone is logged in or not and if they are do something if not do something else
    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }

    // here we can "inject" our context service into the constructor
    public UserController(WPContext context)
    {
        db = context;
    }

    //! renders the register and login page
    [HttpGet("")]
    public IActionResult Index()
    {
        if (loggedIn)
        {
            return RedirectToAction("Weddings", "Wedding");
        }
        return View("Index");
    }

    //! POST registers new user
    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {

        if (ModelState.IsValid)
        {
            if(db.Users.Any(user => user.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "is taken");
            }
        }

        if (ModelState.IsValid == false)
        {
            return Index();
        }

        // now we hash our passwords
        PasswordHasher<User> hashBrowns = new PasswordHasher<User>();
        newUser.Password = hashBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        // now that we've run SaveChanges() we have access to the UserId from our SQL db
        HttpContext.Session.SetInt32("UUID", newUser.UserId); 
        return RedirectToAction("Weddings", "Wedding");
    }

    //! rendering an all page
    [HttpGet("/all")]
    public IActionResult All()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        return RedirectToAction("Weddings", "Wedding");
    }

    //! POST login
    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {


        if (ModelState.IsValid == false)
        {
            return Index();
        }
        // does email in db already exist
        User? dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if (dbUser == null)
        {
            // normally login validations should be more generic to avoid phishing
            // but we wern't using specific error messages for testing
            ModelState.AddModelError("LoginEmail", "not found");
            return Index();
        }

        PasswordHasher<LoginUser> hashBrowns = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hashBrowns.VerifyHashedPassword
        (loginUser, dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "is not correct");
            return Index();
        }
        
        // no returns, therefore no errors
        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        return RedirectToAction("Weddings", "Wedding");
    }

    //! POST Logout
    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

}