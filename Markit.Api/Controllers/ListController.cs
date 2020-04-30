using System.Collections.Generic;
using System.Threading.Tasks;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Markit.Api.Controllers
 {
     [ApiController, Route("list")]
     public class ListController : Controller
     {
         private readonly IListManager _listManager;
         public ListController(IListManager listManager)
         {
             _listManager = listManager;
         }
         
         [HttpGet("{listId}")]
         public IActionResult Get(int listId)
         { 
             return Ok(new ShoppingList
             {
                 Id = listId,
                 Name = "Test List",
                 Description = "This is a hardcoded list",
                 ListTags = new List<ListTag>
                 {
                     new ListTag
                       {
                           Id = 0,
                           Tag = new Tag
                           {
                             Id = 0,
                             Name = "Cat food"
                           },
                           Quantity = 1,
                           Comment = "Meow Meow"
                       },
                       new ListTag
                       {
                           Id = 1,
                           Tag = new Tag
                           {
                               Id = 20,
                               Name = "Doritos"
                           },
                           Quantity = 1,
                       },
                       new ListTag
                       {
                           Id = 2,
                           Tag = new Tag
                           {
                               Id = 6,
                               Name = "Bread"
                           },
                           Quantity = 1,
                       }
                   },
                });
         }
         
         [HttpPost]
         public async Task<IActionResult> Post(ShoppingList list)
         {
             var newList = _listManager.CreateShoppingList(list);
             return Ok(list);
         }

         [HttpPatch("{listId}")]
         public IActionResult Patch(int listId, ListTag tag)
         {
             var list = new ShoppingList
             {
                 Id = listId,
                 Name = "Test List",
                 Description = "This is a hardcoded list",
                 ListTags = new List<ListTag>
                 {
                     new ListTag
                     {
                         Id = 0,
                         Tag = new Tag
                         {
                             Id = 0,
                             Name = "Cat food"
                         },
                         Quantity = 1,
                         Comment = "Meow Meow"
                     },
                     new ListTag
                     {
                         Id = 1,
                         Tag = new Tag
                         {
                             Id = 20,
                             Name = "Doritos"
                         },
                         Quantity = 1,
                     },
                     new ListTag
                     {
                         Id = 2,
                         Tag = new Tag
                         {
                             Id = 6,
                             Name = "Bread"
                         },
                         Quantity = 1,
                     }
                 },
             };
             
             list.ListTags.Add(tag);

             return Ok(list);
         }
         
         [HttpDelete("{listId}")]
         public IActionResult Delete(string listId)
         {
             return Ok();
         }
     }
 }