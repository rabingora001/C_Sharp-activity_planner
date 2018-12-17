using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DojoActivity.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DojoActivity.Controllers
{
    public class DojoActivityController : Controller
    {
        private UserContext dbContext;
        public DojoActivityController(UserContext context)
        {
            dbContext = context;
        }

// GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<User> allUsers = dbContext.UsersTable.ToList();
            ViewBag.AllUsers = allUsers;
            return View();   // this is same as return View("Index")
        }
        
//Registration process
        [HttpPost]
        [Route("/registration")]
        public IActionResult RegistrationProcess(User user)
        {
            List<User> allUsers=dbContext.UsersTable.ToList();
            ViewBag.AllUsers = allUsers;

            if (ModelState.IsValid)
            {
                if(dbContext.UsersTable.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use. use other email!!");
                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                dbContext.Add(user);
                dbContext.SaveChanges();

                //setting user's firstname in session
                HttpContext.Session.SetString("firstName", user.FirstName);
                HttpContext.Session.SetString("lastName", user.LastName);
                HttpContext.Session.SetInt32("UserId", user.UserId);

                

                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

//Login process
        [HttpPost]
        [Route("/login")]
        public IActionResult LoginProcess(LoginUser userSubmission)
        {
            List<User> allUsers = dbContext.UsersTable.ToList();
            ViewBag.AllUsers = allUsers;

            if(ModelState.IsValid)
            {
                var userInDb = dbContext.UsersTable.SingleOrDefault(u => u.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email!! Maybe you need to register first!!");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);

                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid password. it did not match with database password!!");
                    return View("Index");
                }
                HttpContext.Session.SetString("firstName", userInDb.FirstName);
                HttpContext.Session.SetString("lastName", userInDb.LastName);
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);


                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

//delete process
        [HttpGet]
        [Route("/delete/{deleteId}")]
        public IActionResult DeleteUser(int deleteId)
        {
            User deleteOne = dbContext.UsersTable.SingleOrDefault(r => r.UserId == deleteId);
            dbContext.UsersTable.Remove(deleteOne);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

//logout process
        [HttpGet]
        [Route("/logout")]
        public IActionResult Logout()
        {
            
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    

//Registration/Login success process and routes to Home = Dashboard
        [HttpGet("/home")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");

            int? UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userId = HttpContext.Session.GetInt32("UserId");

            List<Activity> activitywithparticipant = dbContext.ActivityTable.Include(w => w.participant).Include(w => w.Creator).ToList();

            return View(activitywithparticipant);
        }

//render new Activity page
        [HttpGet]
        [Route("/new")]
        public IActionResult NewMethod()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");

            int? UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");

            return View("NewPage");
        }

//create method
        [HttpPost]
        [Route("/createRoute")]
        public IActionResult CreateMethod(Activity schedule)
        {

            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");

            
            if(ModelState.IsValid)
            {
                if(schedule.Date <= DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Date must be set to the future date!!");
                    return View("NewPage");
                }

                if(schedule.Time <= DateTime.Today)
                {
                    ModelState.AddModelError("Time", "Time must be set to the future Time!!");
                    return View("NewPage");
                }
                schedule.UserId = (int)HttpContext.Session.GetInt32("UserId");

                dbContext.Add(schedule);
                dbContext.SaveChanges();
                
                System.Console.WriteLine("################################################");
                System.Console.WriteLine("################################################");
                System.Console.WriteLine("################################################");
                System.Console.WriteLine("################################################");
                System.Console.WriteLine("################################################");
                System.Console.WriteLine("################################################");

                return RedirectToAction("Dashboard");
            }
            return View("NewPage");
        }

//show One method
        [HttpGet]
        [Route("/show/{showId}")]
        public IActionResult ShowMethod(int showId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.firstname = HttpContext.Session.GetString("firstName");
            ViewBag.lastname = HttpContext.Session.GetString("lastName");

            int? UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.userid = HttpContext.Session.GetInt32("UserId");

            Activity showOne = dbContext.ActivityTable.Include(w => w.participant).ThenInclude(w => w.User).SingleOrDefault(h =>h.ActivityId == showId);
            ViewBag.ShowOne = showOne;

            return View();
        }

//delete one
        [HttpGet]
        [Route("/deleteOne/{deleteOneId}")]
        public IActionResult DeleteOne(int deleteOneId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            Activity Current = dbContext.ActivityTable.SingleOrDefault( d => d.ActivityId == deleteOneId);
            dbContext.ActivityTable.Remove(Current);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

//rsvp method
    [HttpGet]
        [Route("//rsvp/{wId}")]
        public IActionResult RSVP(int wId)
        {
            
            //check user in session
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            //get the session id 
            User currentUser = dbContext.UsersTable.SingleOrDefault( c => c.UserId == HttpContext.Session.GetInt32("UserId"));

            Activity currAct = dbContext.ActivityTable.Include( w => w.participant).ThenInclude(w => w.User).SingleOrDefault(r => r.ActivityId == wId);
            Participant thisparticipant = dbContext.ParticipantTable.Where(j => j.ActivityId == wId && j.UserId == currentUser.UserId).SingleOrDefault();
            
            if(thisparticipant != null)
            {
                currentUser.participant.Remove(thisparticipant);
            }
            else{
                Participant newParticipant = new Participant
                {
                    UserId = currentUser.UserId,
                    ActivityId = currAct.ActivityId,
                };
                currAct.participant.Add(newParticipant);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        
    }
}
