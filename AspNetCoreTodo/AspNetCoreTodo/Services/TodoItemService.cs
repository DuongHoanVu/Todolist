using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;
        
        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user)
        {
            // _context point to Items table
            // Where(): called LINQ (language integrated query)
            // It is a task, thus adding 'await'

            Console.WriteLine("User id1: " + user );
            //source: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.identityuser?view=aspnetcore-1.1
            Console.WriteLine("User id2: " + user.Id );


            // return await _context.Items
            //         .Where(x => x.IsDone == false)
            //         .ToArrayAsync();

            var li = await _context.Items
                    .Where(x => x.IsDone == false && x.UserId == user.Id)
                    .ToArrayAsync();
            Console.WriteLine("User id4: " + li.Length );
            //Console.WriteLine("User id4: " + li[0].Title );

            var li1 = await _context.Items
                    .Where(x => x.IsDone == false || x.IsDone == true)
                    .ToArrayAsync();
            
            Console.WriteLine("User id4: " + li1.Length );

            for (int i = 0; i < li1.Length; i++) 
            {
                Console.WriteLine(li1[i].UserId);
                Console.WriteLine(li1[i].Title);
            }

            return await _context.Items
                    .Where(x => x.IsDone == false && x.UserId == user.Id)
                    .ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            newItem.UserId = user.Id;

            Console.WriteLine("User id3: " + user.Id );

            // Add a new item to Item table
            _context.Items.Add(newItem);
            
            // save a new item to database
            var saveResult = await _context.SaveChangesAsync();
            
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                                .Where(x => x.Id == id && x.UserId == user.Id)
                                .SingleOrDefaultAsync();
            if (item == null) return false;
            
            item.IsDone = true;
            
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }
    }
}