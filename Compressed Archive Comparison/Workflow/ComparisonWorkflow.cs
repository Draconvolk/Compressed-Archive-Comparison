using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Workflow
{
	public class ComparisonWorkflow(IExceptionList exceptionList, IWorkflowActions workflow) : IComparisonWorkflow
	{
		private static readonly string CW = "ComparisonWorkflow";
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
			catch (Exception ex)
			{
				exceptionList.Add(ex, "Error trying to load the Config", $"{CW}\\StartWorkflow", [.. args]);
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
					var currentState = Enum.GetName(typeof(StateFlow), CurrentState);
					exceptionList.Add(new Exception("Invalid Call"), $"Invalid call to Next based on Current State of {currentState}", $"{CW}\\Next");
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
					_ => false //change if additional types added
				};
			}
		}

		public bool IsFinished() => CurrentState >= StateFlow.Finished;

		public void UpdateState() => CurrentState++;

		public async Task LoadInitialData()
		{
			try
			{
				var loadSource = Task.Run(workflow.LoadCompressedSource);
				workflow.LoadDestination();
				await loadSource;
			}
			catch(Exception ex)
			{
				exceptionList.Add(ex, $"Something went wrong while loading the initial data", $"{CW}\\LoadInitialData");
			}
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
