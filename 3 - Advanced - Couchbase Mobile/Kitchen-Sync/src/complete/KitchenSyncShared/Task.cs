using System;
using System.Collections.Generic;

using Couchbase.Lite.Util;

namespace KitchenSyncShared
{
    public class Task
    {
        private const string TAG = "Task";

        public string ID { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        public DateTime CreationDate { get; set; }

        public static Task FromDictionary(IDictionary<string, object> properties)
        {
            Task t = new Task();
            try {
                t.ID = properties["_id"] as string;
                t.Text = properties["text"] as string;
                t.Checked = (bool)properties["checked"];
                t.CreationDate = (DateTime)properties["created_at"];
            } catch(KeyNotFoundException) {
                Log.E(TAG, "Invalid dictionary passed to FromDictionary");
                return null;
            } catch(InvalidCastException) {
                Log.E(TAG, "Dictionary contains invalid value(s)");
                return null;
            }

            return t;
        }

        public Dictionary<string, object> ToDictionary()
        {
            var d = new Dictionary<string, object>
            {
                {"text", Text},
                {"created_at", CreationDate},
                {"checked", Checked},
            };

            return d;
        }
    }
}