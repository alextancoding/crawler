﻿using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private static CloudQueue queue;

        [WebMethod]
        public string CalculateSumUsingWorkerRole(int a, int b, int c)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                 CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("myurls");
            queue.CreateIfNotExists();

            //add message
            CloudQueueMessage message = new CloudQueueMessage(Numbers.encode(a, b, c));
            queue.AddMessage(message);
            return "done";
        }


        [WebMethod]
        public List<Numbers> ReadSumFromTableStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("sum");
            TableQuery<Numbers> rangeQuery = new TableQuery<Numbers>();

            List<Numbers> result = table.ExecuteQuery(rangeQuery).ToList();

            return result;
        }


        [WebMethod]
        public string insertQ()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("test");
            queue.CreateIfNotExists();

            //add message
            CloudQueueMessage message = new CloudQueueMessage("http://www.cnn.com/index.html");
            queue.AddMessage(message);
            return "done";
        }

        [WebMethod]
        public string readQ()
        {
            // remove message
            CloudQueueMessage message2 = queue.GetMessage(TimeSpan.FromMinutes(5));
            queue.DeleteMessage(message2);
            return "" + message2.AsString;
        }

    }
}