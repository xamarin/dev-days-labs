using System;
using System.Collections.Generic;
using System.Linq;

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
            _db = Manager.SharedInstance.GetDatabase("kitchen-sync");
            View view = _db.GetView("viewItemsByDate");
            view.SetMap((doc, emit) => {
                if(doc.ContainsKey("created_at") && doc["created_at"] is DateTime) {
                    emit(doc["created_at"], null);
                }
            }, "1");

            _query = view.CreateQuery().ToLiveQuery();
            _query.Descending = true;
            _query.Changed += (sender, e) => {
                if(TasksUpdated != null) {
                    var tasks = from row in e.Rows
                        select Task.FromDictionary(row.Document.Properties);
                    TasksUpdated(this, tasks.ToList());
                }
            };

            _query.Start();
        }

        public void StartSync(string syncUrl)
        {
            Uri uri;
            try {
                uri = new Uri(syncUrl);
            } catch(UriFormatException) {
                Log.E("TaskManager", "Invalid URL {0}", syncUrl);
                return;
            }

            if(_pull == null) {
                _pull = _db.CreatePullReplication(uri);
                _pull.Continuous = true;
                _pull.Start();
            }

            if(_push == null) {
                _push = _db.CreatePushReplication(uri);
                _push.Continuous = true;
                _push.Start();
            }
        }

        public void SaveTask (Task item)
        {
            if (item.ID == null) {
                Document doc = _db.CreateDocument ();
                doc.PutProperties (item.ToDictionary ());
                item.ID = doc.Id;
            } else {
                Document doc = _db.GetDocument (item.ID);
                doc.Update (newRevision => {
                    newRevision.SetUserProperties(item.ToDictionary());
                    return true;
                });
            }
        }

        public void DeleteTask (Task task)
        {
            var doc = _db.GetExistingDocument (task.ID);
            if(doc != null) {
                doc.Delete();
            }
        }

    }
}