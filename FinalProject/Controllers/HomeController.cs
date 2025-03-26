using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Repository;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FinalProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly IUser _users;
		private readonly IComposition _compositions;
		private readonly IComment _comments;

		public HomeController(IUser users, IComposition compositions, IComment comments)
		{
			_users = users;
			_compositions = compositions;
			_comments = comments;
		}
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(User model)
		{

			var user = _users.GetAllUsers().FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

			if (user != null)
			{
				HttpContext.Session.SetString("UserId", user.Id);
				HttpContext.Session.SetString("UserName", user.UserName);
				HttpContext.Session.SetString("UserEmail", user.Email);
				_users.SetLoggedUser(user);

				return RedirectToAction("Profile", "Home");
			}
			else
			{
				ModelState.AddModelError("", "Невірне ім'я користувача або пароль.");
			}


			return View(model);
		}


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(User model)
		{
			ModelState.Remove("Comments");
			ModelState.Remove("Compositions");


			if (ModelState.IsValid)
			{
				var existingUser = _users.GetUser(model.UserName);
				if (existingUser == null)
				{
					_users.AddUser(model);
					_users.SetLoggedUser(model);
					return RedirectToAction("Profile", "Home");
				}
				ViewBag.ErrorMessage = "Користувач із таким іменем вже існує.";
			}
			return View(model);
		}

		//Разобраться с этим действием
		public IActionResult Profile()
		{

			var userName = HttpContext.Session.GetString("UserId") as string;
			if (string.IsNullOrEmpty(userName))
			{
				return RedirectToAction("Login", "Home");
			}

			var user = _users.GetUser(userName);
			var comments = _comments.GetAllComments();

			if (comments.Any())
			{
				user.Comments=comments.Where(x => x.UserId==user.Id).ToList();

			}
			else
			{
				user.Comments=new List<Comment>();
			}
			if (user == null)
			{
				return NotFound();
			}
			else
			{
				user.Comments=_comments.GetCommentsByUser(user.Id).ToList();
				return View(user);
			}

		}

		public IActionResult Exit()
		{
			_users.SetLoggedUser();
			HttpContext.Session.SetString("UserId", "");
			HttpContext.Session.SetString("UserName", "");
			HttpContext.Session.SetString("UserEmail", "");
			return RedirectToAction("Login", "Home");
		}

		[HttpPost]
		public IActionResult UpdateProfile(User model)
		{
			var user = _users.GetUser(model.Id);

			if (user==null)
			{
				return NotFound();
			}
			else
			{
				_users.UpdateUser(model);
			}
			return RedirectToAction("Profile", "Home");
		}

		[HttpGet]
		public IActionResult Publish()
		{
			if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return View(new Composition());
			}
			return RedirectToAction(nameof(Login));
		}

		[HttpPost]
		public IActionResult Publish(Composition model)
		{
			model.Comments=new List<Comment>();
			if (ModelState.IsValid)
			{
				_compositions.AddComposition(model);
				return RedirectToAction(nameof(Profile));
			}
			return View(model);
		}

		[HttpPost]
		public IActionResult DeleteUser(string id)
		{
			var userId = HttpContext.Session.GetString("UserId");
			if (!string.IsNullOrEmpty(userId))
			{
				Exit();
				return RedirectToAction("Login", "Home");
			}

			return NotFound();
		}

		public IActionResult Index(string searchAuthor, string searchTitle, string searchGenre, string filterType)
		{
			var compositions = _compositions.GetAllCompositions()
				.AsQueryable();


			if (!string.IsNullOrWhiteSpace(searchAuthor))
			{
				compositions = compositions.Where(c => c.User.UserName.Contains(searchAuthor));
			}

			if (!string.IsNullOrWhiteSpace(searchTitle))
			{
				compositions = compositions.Where(c => c.Name.Contains(searchTitle));
			}

			if (!string.IsNullOrWhiteSpace(searchGenre))
			{
				compositions = compositions.Where(c => c.Genre.ToString() == searchGenre);
			}

			decimal rating = 0;
			foreach (var composition in compositions)
			{
				var ratings = _comments.GetCommentsByComposition(composition.Id);
				if (ratings.Count()!=0)
				{
					rating = ratings.Sum(c => c.Rating)/ratings.Count();
				}
				composition.Rating=rating;
			}

			switch (filterType)
			{
				case "ratingDesc":
					compositions = compositions.OrderByDescending(c => c.Rating);
					break;
				case "ratingAsc":
					compositions = compositions.OrderBy(c => c.Rating);
					break;
			}

			ViewData["searchAuthor"] = searchAuthor;
			ViewData["searchTitle"] = searchTitle;
			ViewData["searchGenre"] = searchGenre;
			ViewData["filterType"] = filterType;

			return View(compositions.ToList());
		}



		[HttpGet]
		public IActionResult Composition(string id, int page = 1)
		{
			var composition = _compositions.GetComposition(id);
			if (page==0)
			{
				page=1;
			}

			if (composition == null)
				return NotFound();


			int pageSize = 1000;
			int totalPages = (int)Math.Ceiling((double)composition.Text.Length / pageSize);


			int startIndex = (page - 1) * pageSize;
			int length = Math.Min(pageSize, composition.Text.Length - startIndex);
			string pageText = composition.Text.Substring(startIndex, length);
			var allComments = _comments.GetAllComments().Where(x => x.CompositionId== id).ToList();
			int averageRate = 0;
			if (allComments.Any())
			{
				averageRate = (int)allComments.Average(x => x.Rating);
			}

			var model = new CompositionDetailsViewModel
			{
				Id = composition.Id,
				Name = composition.Name,
				Genre = composition.Genre,
				PublishDate = composition.PublishDate,
				Rating = averageRate,
				Text = pageText,
				UserName = composition.User.UserName,
				CurrentPage = page,
				TotalPages = totalPages,
				Comments=allComments
			};
			var isAlreadyRated = false;
			int rate = 0;
			string comment = "";
			var comments = _comments.GetCommentsByUser(HttpContext.Session.GetString("UserId"));

			if (comments.Any(x => x.CompositionId==id))
			{
				rate = comments.FirstOrDefault(x => x.CompositionId==id).Rating;
				comment = comments.FirstOrDefault(x => x.CompositionId==id).Text;
				isAlreadyRated = true;
			}
			ViewBag.IsAlreadyRated=isAlreadyRated;
			ViewBag.Rate=rate;
			ViewBag.Text=comment;
			return View(model);
		}

		public IActionResult AddComment(string id)
		{
			var composition = _compositions.GetComposition(id);
			if (composition == null)
			{
				return NotFound();
			}
			var userId = HttpContext.Session.GetString("UserId");
			var viewModel = new AddCommentViewModel
			{
				CompositionId = composition.Id,
				Name = composition.Name,
				Genre = composition.Genre,
				PublishDate = composition.PublishDate,
				UserId=HttpContext.Session.GetString("UserId")
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult SubmitComment(AddCommentViewModel model)
		{
			var comment = new Comment
			{
				UserId = model.UserId,
				CompositionId = model.CompositionId,
				Text = model.CommentText,
				Rating = model.Rating
			};

			_comments.AddComment(comment);

			return RedirectToAction("Composition", "Home", new { id = model.CompositionId });
		}



	}
}
