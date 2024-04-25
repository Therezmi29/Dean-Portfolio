using Dean_Resume.Data;
using Dean_Resume.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dean_Resume.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DeanContext _context;
        public HomeController(DeanContext context)
        {
            _context = context;
        }


        #region Registration
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Redirect("/Home/PortfolioIndex");
            //}
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Wrong Info!");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),

            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);


            await HttpContext.SignInAsync(principal);

            return RedirectToAction(nameof(PortfolioIndex));
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home/Login");
        }
        #endregion

        #region Portfolio
        public IEnumerable<Product> Product { get; set; }
        public IActionResult PortfolioIndex()
        {
            Product = _context.Products.ToList();
            return View(Product);
        }


        [BindProperty]
        public AddEditViewModel AddEdit { get; set; }
        public IActionResult AddPortfolio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPortfolio(AddEditViewModel AddEdit)
        {
            if (!ModelState.IsValid)
                return View();

            var pro = new Product()
            {
                Name = AddEdit.Name,
                Note = AddEdit.Note,
            };
            _context.Add(pro);
            _context.SaveChanges();

            if (AddEdit.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "assets",
                    "img",
                    "portfolio",
                    pro.ProductId + Path.GetExtension(AddEdit.Picture.FileName).ToLower());

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    AddEdit.Picture.CopyTo(stream);
                }
            }


            return RedirectToAction("PortfolioIndex");
        }
        #endregion

        #region DeletePortfolio

        public IActionResult Delete(int id)
        {
            var products = _context.Products.FirstOrDefault(s => s.ProductId == id);
            return View(products);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteItem(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot",
                                "assets",
                                "img",
                                "portfolio",
                                product.ProductId + ".jpg");

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction(nameof(PortfolioIndex));
        }
        #endregion

        #region EditPortfolio

        public IActionResult Edit(int id)
        {
            var addEdit = _context.Products.Where(s => s.ProductId == id)
                .Select(s => new AddEditViewModel()
                {
                    Id = s.ProductId,
                    Note = s.Note,
                    Name = s.Name,
                }).FirstOrDefault();

            return View(addEdit);
        }

        [HttpPost]
        public IActionResult Edit()
        {
            if (!ModelState.IsValid)
                return View();

            var addEdit = _context.Products.Find(AddEdit.Id);

            addEdit.Name = AddEdit.Name;
            addEdit.Note = AddEdit.Note;

            _context.SaveChanges();

            if (AddEdit.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "assets",
                    "img",
                    "portfolio",
                    addEdit.ProductId + Path.GetExtension(AddEdit.Picture.FileName));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    AddEdit.Picture.CopyTo(stream);
                }
            }

            return RedirectToAction(nameof(PortfolioIndex));

            #endregion
        }
    }
}

