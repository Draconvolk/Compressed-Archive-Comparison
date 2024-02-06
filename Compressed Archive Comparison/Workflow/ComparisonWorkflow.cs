namespace CompressedArchiveComparison.Workflow
{
	public class ComparisonWorkflow(IWorkflowActions workflow) : IComparisonWorkflow
	{
		private readonly object _canDisplayLock = new();
		private readonly Dictionary<DisplayType, bool> _canStartDisplay = new()
		{
			{ DisplayType.Source, true },
			{ DisplayType.Missing, true }
		};

		public StateFlow CurrentState { get; set; } = StateFlow.PreInit;

		public async Task StartWorkflow(string[] args)
		{
			try
			{
				Console.WriteLine("The default config file is [Config.json]. A custom config file can be specified as an argument");
				workflow.SetConfig(args, workflow.Config);
				var loadConfig = Task.Run(workflow.LoadConfig);
				await loadConfig;
				UpdateState();
			}
			catch
			{
				Environment.Exit(-1);
			}
		}

		public async Task Next()
		{
			switch (CurrentState)
			{
				case StateFlow.PreInit:
					await StartWorkflow([]);
					UpdateState();
					break;
				case StateFlow.StartWorkflow:
					await LoadInitialData();
					UpdateState();
					break;
				case StateFlow.LoadInitialData:
					IdentifyMissing();
					UpdateState();
					break;
				case StateFlow.IdentifyMissing:
					await ExportMissing();
					UpdateState();
					break;
				case StateFlow.ExportMissing:
					UpdateState();
					break;
				case StateFlow.Finished:
					break;
				default:
					Console.WriteLine($"Invalid call to Next based on Current State of [{Enum.GetName(typeof(StateFlow), CurrentState)}]");
					break;
			}
		}

		public bool CanDisplay(DisplayType type)
		{
			lock (_canDisplayLock)
			{
				return _canStartDisplay[type] && type switch
				{
					DisplayType.Source => CurrentState >= StateFlow.LoadInitialData,
					DisplayType.Missing => CurrentState >= StateFlow.IdentifyMissing,
					_ => false //should never be hit
				};
			}
		}

		public bool IsFinished() => CurrentState >= StateFlow.Finished;

		public void UpdateState() => CurrentState++;

		public async Task LoadInitialData()
		{
			var loadSource = Task.Run(workflow.LoadCompressedSource);
			workflow.LoadDestination();
			await loadSource;
		}

		public async Task DisplaySource()
		{
			if (CanDisplay(DisplayType.Source))
			{
				lock (_canDisplayLock)
				{
					_canStartDisplay[DisplayType.Source] = false;
				}
				await workflow.DisplaySource();
			}
		}

		public void IdentifyMissing() => workflow.IdentifyMissingFiles();

		public async Task ExportMissing() => await workflow.ExportMissingFiles();

		public async Task DisplayMissing()
		{
			if (CanDisplay(DisplayType.Missing))
			{
				lock (_canDisplayLock)
				{
					_canStartDisplay[DisplayType.Missing] = false;
				}
				await workflow.DisplayMissingFiles();
			}
		}

		public bool IsState(StateFlow state) => CurrentState == state;
	}
}
