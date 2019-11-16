using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureCosmosdb.Repository;
using AzureCosmosdb.Models;

namespace AzureCosmosdb.Controllers
{
    public class TodoController : Controller
    {
        public ToDoRepository repository = new ToDoRepository();
        public IActionResult Index()
        {
            var entities = repository.All();
            var models = entities.Select(x => new TodoModel
            {
                Id = x.RowKey,
                Group = x.PartitionKey,
                Content = x.Content,
                Due = x.Due,
                Completed = x.Completed,
                TimeStamp = x.Timestamp
            });

            return View(models);
        }

        [HttpPost]
        public ActionResult Create(TodoModel model )
        {
            repository.CreateOrUpdate(new TodoEntity
            {
                PartitionKey = model.Group,
                RowKey = Guid.NewGuid().ToString(),
                Content = model.Content,
                Due = model.Due,
                Timestamp = DateTime.Now
            }) ;
            // redirect to page index 
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ConfirmDelete(string id , string group)
        {
            var item = repository.Get(group, id);
            return View( "Delete",new TodoModel {
                Id = item.RowKey,
                Group = item.PartitionKey,
                Content = item.Content,
                Due = item.Due,
                Completed = item.Completed,
                TimeStamp = item.Timestamp

            });
        }

        [HttpPost]
        public ActionResult Delete(string id , string group)
        {
            var item = repository.Get(group, id);
            repository.Delete(item);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id, string group)
        {
            var item = repository.Get(group, id);
            return View(new TodoModel
            {
                Id = item.RowKey,
                Group = item.PartitionKey,
                Content = item.Content,
                Due = item.Due,
                Completed = item.Completed,
                TimeStamp = item.Timestamp

            });
        }

        [HttpPost]
        public ActionResult Edit(TodoModel model)
        {
            var item = repository.Get(model.Group, model.Id);
            if(item !=null)
            {
                item.Content = model.Content;
                item.Completed = model.Completed;
                item.Due = model.Due;                
            }
            repository.CreateOrUpdate(item);
            return RedirectToAction("Index");
        }

        public ActionResult FilterByCompletetedByVacation()
        {
            var entities = repository.GetByCompletedByVacation();
            var models = entities.Select(x => new TodoModel
            {
                Id = x.RowKey,
                Group = x.PartitionKey,
                Content = x.Content,
                Due = x.Due,
                Completed = x.Completed,
                TimeStamp = x.Timestamp
            });

            return View("Index",models);
        }
    }
}