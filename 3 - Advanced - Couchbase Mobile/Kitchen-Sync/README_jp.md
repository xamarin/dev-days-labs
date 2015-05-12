Kitchen Sync
============

## 目的

短時間で初めてのCouchbase Mobileアプリを開発しましょう！既存のアプリを改善させ、保存と同期を入れましょう！


![Application Architecture](https://raw.githubusercontent.com/couchbaselabs/mini-hacks/master/kitchen-sync/topology.png "Typical Couchbase Mobile Architecture")

## 準備

Xamarinのインストールなどに関しては[こちら](http://dev.classmethod.jp/smartphone/xamarin-start/)をご参考にしてください。

### アーキテクチャーの説明

The architecture and project structure of the KitchenSync application is shown in the following diagram:

![architecture](https://raw.githubusercontent.com/couchbaselabs/mini-hacks/master/kitchen-sync/xamarin/project/images/architecture.png)

- User Interface - 画面、コントロール、表示などの部分になります。それぞれのプラットフォームのネイティブUIライブラリーをラッピングしますのでネイティブアプリと同様に動作します。
- App Layer - Business LayerとUser Interfaceをつなげるロジックです。多くはプラットフォーム固有の機能を使います。
- Business Layer - データモデルや共通ロジック
- Data Access Layer - Data LayerとBusiness Layerをつなげるロジックです。
- Data Layer - データの保存と取得です。KitchenSyncはCouchbase Liteを使います。

### チュートリアル

このチュートリアルで共通部分並びにiOS・Android専用ロジックと実装します。各手順はworkshop_startプロジェクトにコメントがあります（プラットフォーム固有の部分は[番号]A (Android)と[番号]B (iOS)になっています）。

- (1) データベースを取得します。. `TaskManager`初期化にこれを追加しましょう。
```c#
_db = Manager.SharedInstance.GetDatabase("kitchen-sync");
```
- (2) これから速いクエリを作るためにインデックスを作成します。Couchbase LiteはC#関数でクエリを作ることができるMapReduceクエリを使います。それにドキュメントの変換や集計の計算などもできますが、こちらの場合は単純に日付によってのインデックスを作ります。
```c#
View view = _db.GetView("viewItemsByDate");
view.SetMap((doc, emit) => {
    if(doc.ContainsKey("created_at") && doc["created_at"] is DateTime) {
        emit(doc["created_at"], null);
    }
}, "1");
```
- (3) 次にLiveQueryを開始します。普通のQueryのように２番で作ったインデックスをフィルターやソートをかけることができます。しかしその上にクエリが終わった後のデータについても通知されます。そしてTaskオブジェクトを作成してUIに通知する必要があります。
```c#
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
```
- (4) これからUIの作成と更新によって`Task`を保存する機能を入れます。作成と更新のロジックは似ていますが作成は`PutProperties`関数を使い、更新は`Update`関数を使います。新規`Task`のIDはまだ入っていないのでそれで区別できます。以下のソースを`SaveTask`に入れましょう。
```c#
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
```
- (5) 保存があれば削除も必要です。既存のドキュメントを求めてnullでない場合は`Delete`関数を呼ぶ簡単な行動です。以下を`DeleteTask`に入れましょう。
```c#
 var doc = _db.GetExistingDocument (task.ID);
 if(doc != null) {
     doc.Delete();
 }
```
- (6A) UIに通知しなければデータの取得は無意味でしょう。Androidの場合はデータを表示する`ListView`の`Adapter`プロパティを指定することによって行います。このプロジェクトの`KitchenSyncListAdapter`クラスがその役割をします。すでに作ってありますので、作成さえすればよいです。`MainActivity`の`InitItemListAdapter`に以下を追加しましょう。
```c#
 _listAdapter = new KitchenSyncListAdapter(ApplicationContext, Resource.Layout.list_item, Resource.Id.label, new List<Task>());

 _itemListView.Adapter = _listAdapter;
```
- (6B) iOSの場合は似ています。Androidは`KitchenSyncListAdapter`と言えばiOSは`DataSource`クラスを使います。`DataSource`は`HomeScreenController`にあり、小さい変更で準備ができます（データが変わる際にテーブルビューを更新します）。`DataSource`の初期化にこれを入れましょう。
```c#
 taskMgr.TasksUpdated += (sender, e) => {
     _rows = e;
     controller.tableView.ReloadData();
 };
```
- (7A) チェックボックスをトグルする方法を入れましょう。最も自然なやり方は`ListView`のタップで行うことでしょう。`OnItemClick`に以下を追加しましょう。
```c#
 Task task = _listAdapter.GetItem(position);
 task.Checked = !task.Checked;
 _taskMgr.SaveTask(task);
```
- (7B) iOSの方もタップでトグル処理をします。`TableViewDelegate`の`RowSelected`に以下を追加しましょう。  
```c#
 var task = _source.Tasks[indexPath.Row];
 task.Checked = !task.Checked;
 var manager = new TaskManager();
 manager.SaveTask(task);
```
- (8A) 削除に関してはAndroidの場合は長押しが一番自然でしょう。`OnItemLongClick`に以下を追加しましょう。
```c#
 Task task = _listAdapter.GetItem(position);

 AlertDialog.Builder builder = new AlertDialog.Builder(this);
 AlertDialog alert = builder.SetTitle("Delete Item?").
 SetMessage("Are you sure you want to delete \"" + task.Text + "\"?").
 SetPositiveButton("Yes", (sender, e) => _taskMgr.DeleteTask(task)).
 SetNegativeButton("No", (sender, e) => {}).Create();

 alert.Show();
```
- (8B) しかしiOSに関してはユーザーはスワイプで削除処理を行うように慣れているでしょう。`DataSource`の`CommitEditingStyle`に以下を追加しましょう。
```c#
 if (editingStyle == UITableViewCellEditingStyle.Delete) {
     taskMgr.DeleteTask (_rows[indexPath.Row]);
 }
```
- 端末で実行してみましょう。リストの項目が保存されるはずです。
- (9A) 同期を追加しましょう！MainActivityの上側に同期のURLを定義しま す。端末で行っている場合はパソコンのWi-Fi IPアドレスを使います（つまりlocalhost>ではない）。エミュレーターを使っている場合はlocalhostが10.0.2.2になります。以下を`MainActivity`に追加しましょう。
```c#
private const string SYNC_URL = "http://<YOUR_WIFI_OR_ETHERNET_IP>:4984/kitchen-sync";
```
- (9B) 同期を追加しましょう！一旦AppDelegate.mに戻って、上側に同期のURLを定義します。端末で行っている場合はパソコンのWi-Fi IPアドレスを使います（つまりlocalhostではない）。シミュレーターを使っている場合はlocalhostでも大丈夫です。以下を`HomeScreenController`に追加しましょう。
```c#
private const string SYNC_URL = "http://<YOUR_WIFI_OR_ETHERNET_IP>:4984/kitchen-sync";
```
- (10) 一番難しいところが無事に済みました。継続的にローカルとリモートの変更を同期するStartSync関数を入れましょう
```c#
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
```
- (11A) 後は呼び出すだけです。`MainActivity`の`OnCreate`に以下を追加しましょう。
```c#
 _taskMgr.StartSync(SYNC_URL);
```
- (11B) そして`HomeScreenController`の`ViewDidLoad`にも。
```c#
 _taskMgr.StartSync(SYNC_URL);
```
- (12) 実行しましょう！アイテムを追加するか、チェックボックスを変更する際にコンソールにログが見れるはずです。アイテムがリストに追加されていることを確認しましょう。
- (13) Sync Gateway管理者コンソールで結果を見てみましょう。ブラウザーで[http://localhost:4985/_admin/](http://localhost:4985/_admin/)を開いて[kitchen-sync](http://localhost:4985/_admin/db/kitchen-sync)リンクを押します。**Documents**ページが出>てきて、全てのドキュメントをリストアップします。ドキュメントのIDを押すとそのドキ
ュメントの詳細を見ることができます。
- (14) 最後に、皆さんのご意見をお伺いしたいです。 [アンケートにご協力をお願いします
](http://goo.gl/forms/AH8sIlFOiO)!