using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using KitchenSyncShared;

namespace KitchenSyncIos
{
    partial class HomeScreenController : UIViewController
    {
        private DataSource _source;
        private readonly TaskManager _taskMgr = new TaskManager();
        private const string SYNC_URL = "http://localhost:4984/kitchen-sync";

        public HomeScreenController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad();

            Title = "KitchenSync";

            tableView.Source = _source = new DataSource(this);
            textField.Delegate = new TextFieldDelegate();
            tableView.Delegate = new TableViewDelegate(_source);

            _taskMgr.StartSync(SYNC_URL);
        }

        class TableViewDelegate : UITableViewDelegate 
        {
            private readonly DataSource _source;

            public TableViewDelegate(DataSource dataSource)
            {
                _source = dataSource;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var task = _source.Tasks[indexPath.Row];
                task.Checked = !task.Checked;
                var manager = new TaskManager();
                manager.SaveTask(task);
            }
        }
            
        class TextFieldDelegate : UITextFieldDelegate
        {
            public override bool ShouldReturn(UITextField textField)
            {
                textField.ResignFirstResponder();
                return textField.Text.Length > 0;
            }

            public override void EditingEnded(UITextField textField)
            {
                if(textField.Text.Length == 0) {
                    return;
                }

                var task = new Task() { 
                    Text = textField.Text, 
                    Checked = false, 
                    CreationDate = DateTime.UtcNow 
                };

                var taskManager = new TaskManager();
                taskManager.SaveTask(task);
            }
        }

        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifier = new NSString ("TaskCell");
            private readonly HomeScreenController controller;
            private IList<Task> _rows;
            private readonly TaskManager taskMgr = new TaskManager ();

            public IList<Task> Tasks
            {
                get { return _rows; }
            }

            public DataSource (HomeScreenController controller)
            {
                this.controller = controller;
                taskMgr.TasksUpdated += (sender, e) => {
                    _rows = e;
                    controller.tableView.ReloadData();
                };
            }

            public override nint RowsInSection (UITableView tableview, nint section)
            {
                return _rows == null ? 0 : _rows.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell (CellIdentifier);
                if(cell == null) {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                }
                var task = _rows[indexPath.Row];
                cell.TextLabel.Text = task.Text;
                cell.Accessory = task.Checked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

                return cell;
            }

            public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle == UITableViewCellEditingStyle.Delete) {
                    taskMgr.DeleteTask (_rows[indexPath.Row]);
                }
            }
        }
            
    }
}