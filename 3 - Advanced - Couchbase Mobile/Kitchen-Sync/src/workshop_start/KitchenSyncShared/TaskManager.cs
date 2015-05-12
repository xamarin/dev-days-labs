using System;
using System.Collections.Generic;

using Couchbase.Lite;
using Couchbase.Lite.Util;

namespace KitchenSyncShared
{
    public class TaskManager
    {
        private readonly Database _db;
        private readonly LiveQuery _query;
        private Replication _pull;
        private Replication _push;

        public event EventHandler<IList<Task>> TasksUpdated;

        public TaskManager ()
        {
            //Step 1 - Get reference to the database

            //Step 2 - Setup view for querying

            //Step 3 - Setup live query
        }

        public void StartSync(string syncUrl)
        {
            //Step 10 - Create sync objects
        }
            
        public void SaveTask (Task item)
        {
            //Step 4 - Create save logic
        }

        public void DeleteTask (Task task)
        {
            //Step 5 - Create delete logic
        }
            
    }
}