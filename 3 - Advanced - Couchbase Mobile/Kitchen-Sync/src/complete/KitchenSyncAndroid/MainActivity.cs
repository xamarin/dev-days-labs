using System;
using System.Collections;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using KitchenSyncShared;

namespace KitchenSyncAndroid {
   
	[Activity (Label = "KitchenSync", MainLauncher = true, Icon="@drawable/icon")]			
    public class MainActivity : Activity, View.IOnKeyListener, AdapterView.IOnItemClickListener, 
            AdapterView.IOnItemLongClickListener {
        
		private KitchenSyncListAdapter _listAdapter;
		private ListView _itemListView;
        private EditText _addItemEditText;
        private readonly TaskManager _taskMgr = new TaskManager ();
        private const string SYNC_URL = "http://192.168.11.2:4984/kitchen-sync";

        private void InitItemListAdapter()
        {
            _listAdapter = new KitchenSyncListAdapter(ApplicationContext, Resource.Layout.list_item, 
                Resource.Id.label, new List<Task>());

            _itemListView.Adapter = _listAdapter;
        }

        private Task CreateNewListItem(string text)
        {
            var task = new Task() { 
                Text = text, 
                Checked = false, 
                CreationDate = DateTime.UtcNow 
            };

            var taskManager = new TaskManager();
            taskManager.SaveTask(task);

            return task;
        }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            SetContentView(Resource.Layout.main);

            _itemListView = FindViewById<ListView>(Resource.Id.itemListView);
            _itemListView.OnItemClickListener = this;
            _itemListView.OnItemLongClickListener = this;

            _addItemEditText = FindViewById<EditText>(Resource.Id.newItemText);
            _addItemEditText.SetOnKeyListener(this);
            _addItemEditText.RequestFocus();

            InitItemListAdapter();
            _taskMgr.TasksUpdated += (sender, e) => {
                _listAdapter.Clear();
                _listAdapter.AddAll((ICollection)e);
                _listAdapter.NotifyDataSetChanged();
            };

            _taskMgr.StartSync(SYNC_URL);
		}

        public bool OnKey(View v, Keycode keyCode, KeyEvent e)
        {
            if(e.Action == KeyEventActions.Down && keyCode == Keycode.Enter) {
                if(_addItemEditText.Text.Length == 0) {
                    return true;
                }

                string inputText = _addItemEditText.Text;
                CreateNewListItem(inputText);
                Toast.MakeText(ApplicationContext, "Created new list item!", ToastLength.Long);
                _addItemEditText.Text = "";

                return true;
            }

            return false;
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Task task = _listAdapter.GetItem(position);
            task.Checked = !task.Checked;
            _taskMgr.SaveTask(task);
        }

        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            Task task = _listAdapter.GetItem(position);

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alert = builder.SetTitle("Delete Item?").
                SetMessage("Are you sure you want to delete \"" + task.Text + "\"?").
                SetPositiveButton("Yes", (sender, e) => _taskMgr.DeleteTask(task)).
                SetNegativeButton("No", (sender, e) => {}).Create();

            alert.Show();

            return true;
        }
	}
}