using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<IdentityUser> _userManager;
        
        // Constructor
        public TodoController(ITodoItemService todoItemService,
                            UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Todo Page");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            // Console.WriteLine("currentUser: " + currentUser);
            // Console.WriteLine("User: " + User);

            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

            var model = new TodoViewModel()
            {
                Items = items
            };
            return View(model);
        }

        
        /* 
            'Model Binding' looks at the data in request 
            and match the incoming fileds with model properties.
            Any input from browser that doesnt match with model properties will ignored.
            Frontend include @model TodoItem

            Grab info from the form and store it to newItem var
        */
        // [ValidateAntiForgeryToken]: verify the request to prevent CSRF attacks
        // where user submitting data from malicious data


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            // if missing Title (required) attribute will be invalid
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalid input, missing Title");
                // Return to the current page
                return RedirectToAction("Index");
            } 
            
            //var successful = await _todoItemService.AddItemAsync(newItem);
            
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            //var successful = true;
            var successful = await _todoItemService.AddItemAsync(newItem, currentUser);


            if (!successful)
            {
                return BadRequest("Could not add item.");
            } 
            
            // Return to the current page
            return RedirectToAction("Index");
        }


        // Incoming request includes a field clled 'id'
        // ASP.Net will try to parse it as a Guid
        // As there is no model binding as above, thus having no ModelState, 
        // thus checking its value

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            } 
            
            //var successful = await _todoItemService.MarkDoneAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

            
            
            if (!successful)
            {
                return BadRequest("Could not mark item as done.");
            } 
            
            return RedirectToAction("Index");
        }
    }
}