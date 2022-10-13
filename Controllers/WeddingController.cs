// Using statements
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlannerControllers;
    
public class WeddingController : Controller
{

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

    private WPContext db;

    // here we can "inject" our context service into the constructor
    public WeddingController(WPContext context)
    {
        db = context;
    }
    // ! Displaying New Wedding Page
    [HttpGet("/weddings/new")]
    public IActionResult NewWedding()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        return View("NewWedding");
    }

    // ! POST creating a wedding
    [HttpPost("/weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "User");
        }
        if (ModelState.IsValid == false)
        {
            return NewWedding();
        }

        newWedding.UserId = (int)uid;
        db.Weddings.Add(newWedding);
        db.SaveChanges();

        return RedirectToAction("Weddings");
    }

    // ! Displaying the VIEW All Weddings Page
    [HttpGet("/weddings")]
    public IActionResult Weddings()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        List<Wedding> allWeddings = db.Weddings
        .Include(w => w.WeddingCreator)
        .Include(w => w.WeddingAssociation) //QUESTIONSQUESTIONSQUESTIONS
        .ToList();

        return View("AllWeddings", allWeddings);
    }

    // ! Display VIEW ONE Wedding
    [HttpGet("/weddings/{weddingId}")]
    public IActionResult oneWedding(int weddingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        Wedding? oneWedding = db.Weddings
        .Include(w => w.WeddingAssociation)
        .ThenInclude(w => w.User)
        .FirstOrDefault(wedding => wedding.WeddingId == weddingId);

        if (oneWedding == null)
        {
            return RedirectToAction("Weddings");
        }

        return View("OneWedding", oneWedding);
    }

    //! Delete Wedding
    [HttpPost("/weddings/{weddingId}/delete")]
    public IActionResult DeleteWedding( int weddingId)
    {
        if ( !loggedIn ||  uid == null)
        {
            return RedirectToAction("Index", "User");
        }
        if (ModelState.IsValid == false)
        {
            return Weddings();
        }

        Wedding? weddingToDelete = db.Weddings.FirstOrDefault( w => w.WeddingId == weddingId);
        if (weddingToDelete == null || weddingToDelete.UserId != (int)uid)
        {
            return RedirectToAction("Weddings");
        }

        db.Remove(weddingToDelete);
        db.SaveChanges();
        return RedirectToAction("Weddings");
    }

    [HttpPost("/weddings/{weddingId}/participate")]
    public IActionResult Participate(int weddingId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "User");
        }

        Association? existingParticipation = db.Associations.FirstOrDefault(a => a.UserId == (int)uid && a.WeddingId == weddingId);

        if ( existingParticipation != null)
        {
            db.Remove(existingParticipation);
        }
        else
        {
            Association newParticipation = new Association()
            {
                UserId = (int)uid,
                WeddingId = weddingId
            };  
            db.Associations.Add(newParticipation);
        }

        db.SaveChanges();

        return RedirectToAction("Weddings");
    }
}
