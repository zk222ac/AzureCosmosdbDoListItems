using AzureCosmosdb.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosdb.Repository
{
    public class ToDoRepository
    {
        private CloudTable todoTable = null;
        public ToDoRepository()
        {
            string connString = "DefaultEndpointsProtocol=https;AccountName=account001;AccountKey=4lYvnKWu97qxNhygfFVN2pmUhRIxWCgxwjeKI7cc9KNJonfYQAU3c5ZtJcGhLvVKvtglmadrbPeQIT7vUjIN3A==;TableEndpoint=https://account001.table.cosmos.azure.com:443/;";
            var storageAccount = CloudStorageAccount.Parse(connString);
            var tableClient = storageAccount.CreateCloudTableClient();
            todoTable = tableClient.GetTableReference("Todo");
            // shared access signature 
           var sas = todoTable.GetSharedAccessSignature(new SharedAccessTablePolicy
            {
                Permissions = SharedAccessTablePermissions.Query,
                SharedAccessStartTime = DateTime.Now.AddDays(-1),
                SharedAccessExpiryTime = DateTime.Now.AddDays(2)
           }); 
            // create new one if not exist 
            todoTable.CreateIfNotExists();
        }

        // Retrieve all Data from Azure Cosmos Db
        public IEnumerable<TodoEntity> All()
        {
            // return queries with out filteration 
            var query = new TableQuery<TodoEntity>();
            
            var entities = todoTable.ExecuteQuery(query);            
            return entities;
        }
        public IEnumerable<TodoEntity> GetByCompletedByVacation()
        {
            // return queries with out filteration 
            //  var query = new TableQuery<TodoEntity>();

            // return queries with  filteration : Iscompleted filter
            var isCompleted = TableQuery.GenerateFilterConditionForBool(nameof(TodoModel.Completed), QueryComparisons.Equal, true);

            // return queries with  filteration : Vacation filter
            var isVacation = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "vacation");

            // combine filter 
            var query = new TableQuery<TodoEntity>().Where(TableQuery.CombineFilters(isCompleted, TableOperators.And, isVacation));
            var entities = todoTable.ExecuteQuery(query);
            return entities;
        }

        // Create Todo Item List 
        public void CreateOrUpdate(TodoEntity entity)
        {
            // perform insert and update operation in Db
            var operation = TableOperation.InsertOrReplace(entity);
            todoTable.Execute(operation);
        }

        // Delete Item
        public void Delete(TodoEntity entity)
        {
            // perform insert and update operation in Db
            var operation = TableOperation.Delete(entity);
            todoTable.Execute(operation);
        }

        // Get PartitionKey and Rowkey
        public TodoEntity Get(string partitionKey , string rowKey)
        {
            var operation = TableOperation.Retrieve<TodoEntity>(partitionKey , rowKey);
            var result = todoTable.Execute(operation);
            return result.Result as TodoEntity; 
        }

    }
}
