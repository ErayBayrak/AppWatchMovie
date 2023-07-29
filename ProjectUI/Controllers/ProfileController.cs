using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {

        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userMail = HttpContext.Session.GetString("UserMail");
            var user = _userService.GetByMail(userMail);
            UserForUpdateDto dto = new UserForUpdateDto();
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            return View(dto);
        }

        [HttpPost]
        public IActionResult Index(UserForUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var userMail = HttpContext.Session.GetString("UserMail");
                var user = _userService.GetByMail(userMail);

                user.FirstName = userUpdateDto.FirstName;
                user.LastName = userUpdateDto.LastName;
                _userService.Update(user);

                return RedirectToAction("Index");
            }

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.Errors.Any())
                {
                    var errorMessage = state.Errors.First().ErrorMessage;
                }
            }

            return View(userUpdateDto);
        }

    }
}
