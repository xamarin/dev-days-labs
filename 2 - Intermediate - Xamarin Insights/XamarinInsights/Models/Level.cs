using System;
using System.Timers;

namespace XamarinInsights
{
	public class Level : BaseModel
	{
		const double StartingTime = 10000;
		int currentLevel = 0;

		public int CurrentLevel {
			get {
				return currentLevel;
			}
			set {
				ProcPropertyChanged (ref currentLevel, value);
			}
		}

		string currentCombination;

		public string CurrentCombination {
			get {
				return currentCombination;
			}
			set {
				ProcPropertyChanged (ref currentCombination, value);
			}
		}

		string combinationSoFar;

		public string CombinationSoFar {
			get {
				return combinationSoFar;
			}
			private set {
				ProcPropertyChanged (ref combinationSoFar, value);
			}
		}

		double timeRemaining;

		public double TimeRemaining {
			get {
				return timeRemaining;
			}
			set {
				if (ProcPropertyChanged (ref timeRemaining, value))
					TimeRemainingDisplay = string.Format ("Time Remaining: {0}", timeRemaining / 1000);
			}
		}

		string timeRemainingDisplay;

		public string TimeRemainingDisplay {
			get {
				return timeRemainingDisplay;
			}
			set {
				ProcPropertyChanged (ref timeRemainingDisplay, value);
			}
		}

		public void PressKey (string key)
		{
			//TODO: Log key press
			Analytics.LogKeyPress (key);
			CombinationSoFar += key;
			if (CombinationSoFar == CurrentCombination)
				NewLevel ();
			if (!CurrentCombination.StartsWith (CombinationSoFar))
				WrongKey ();
		}

		Random random = new Random ();

		public void NewLevel ()
		{
			StopTimer ();
			CurrentLevel++;
			CombinationSoFar = "";
			CurrentCombination = random.Next (0, 9999).ToString ("0000");
			StartTimer ();
			//TODO: Log start level
			Analytics.LogNewLevel (CurrentLevel, CurrentCombination, TimeRemaining);
		}

		Timer timer;

		public void StartTimer ()
		{
			ResetTimer ();
		}

		public void StopTimer ()
		{
			if (timer == null)
				return;
			timer.Stop ();
		}

		const double timerTick = 100;

		void ResetTimer ()
		{
			TimeRemaining = StartingTime - (1000 * CurrentLevel);
			if (timer == null) {
				timer = new Timer (timerTick) {
					AutoReset = true,
				};
				timer.Elapsed += (sender, e) => Tick ();
			}
			timer.Start ();
		}

		void Tick ()
		{
			TimeRemaining -= timerTick;
			if (TimeRemaining <= 0)
				App.RunOnMainThread (OutOfTime);
		}

		void OutOfTime ()
		{
			throw new Exception ("You ran out of time!!!!");
		}

		void WrongKey ()
		{
			throw new Exception ("You pressed a wrong key :(");
		}

	}
}

