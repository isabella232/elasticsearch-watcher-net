﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Nest.Watcher.Serialization;
using Newtonsoft.Json;

namespace Nest
{
	public interface IExecuteWatchRequest : IIdPath<ExecuteWatchRequestParameters>
	{

		/// <summary>
		/// This structure will be parsed as a trigger event and used for the watch execution.
		/// </summary>
		[JsonProperty("trigger_event")]
		TriggerEventContainer TriggerEvent { get; set; }

		/// <summary>
		/// If this is set to true the watch execution will use the Always Condition.
		/// </summary>
		[JsonProperty("ignore_condition")]
		bool? IgnoreCondition { get; set; }

		/// <summary>
		/// If this is set to true the WatchRecord generated by executing the watch will be persisted 
		/// to the .watch_history index for the current time. 
		/// Also the Watch.Status will be updated, possbily throttling subsequent executions.
		/// </summary>
		[JsonProperty("record_execution")]
		bool? RecordExecution { get; set; }

		/// <summary>
		/// If this is set to true the watch execution will ignore throttling. 
		/// </summary>
		[JsonProperty("ignore_throttle")]
		bool? IgnoreThrottle { get; set; }

		/// <summary>
		/// If this structure is present, the watch will not execute it’s own input but 
		/// will use these fields as a payload instead. 
		/// </summary>
		[JsonProperty("alternative_input")]
		[JsonConverter(typeof(DictionaryKeysAreNotPropertyNamesJsonConverter))]
		IDictionary<string, object> AlternativeInput { get; set; }

		[JsonProperty("action_modes")]
		[JsonConverter(typeof(DictionaryKeysAreNotPropertyNamesJsonConverter))]
		IDictionary<string, ActionExecutionMode> ActionModes { get; set; }

		/// <summary>
		///This field can either be a list of action ids or the string _all. 
		/// If _all is set or an action that is executed by the watch appears in this list 
		/// it will be executed in simulated mode. 
		/// </summary>
		[JsonProperty("simulated_actions")]
		SimulatedActions SimulatedActions { get; set; }
	}

	public partial class ExecuteWatchRequest : WatchIdPathBase<ExecuteWatchRequestParameters>, IExecuteWatchRequest
	{
		public ExecuteWatchRequest(string watchId) : base(watchId) { }

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<ExecuteWatchRequestParameters> pathInfo)
		{
			ExecuteWatchInfo.Update(pathInfo, this);
		}

		public TriggerEventContainer TriggerEvent { get; set; }

		public bool? IgnoreCondition { get; set; }

		public bool? RecordExecution { get; set; }

		public bool? IgnoreThrottle { get; set; }

		public IDictionary<string, object> AlternativeInput { get; set; }

	    public IDictionary<string, ActionExecutionMode> ActionModes { get; set; }

	    public SimulatedActions SimulatedActions { get; set; }
	}

	internal static class ExecuteWatchInfo
	{
		public static void Update(ElasticsearchPathInfo<ExecuteWatchRequestParameters> pathInfo, IExecuteWatchRequest request)
		{
			pathInfo.HttpMethod = PathInfoHttpMethod.POST;
		}
	}

	[DescriptorFor("WatcherExecuteWatch")]
	public partial class ExecuteWatchDescriptor : WatchIdPathDescriptor<ExecuteWatchDescriptor, ExecuteWatchRequestParameters>, IExecuteWatchRequest
	{
		private IExecuteWatchRequest Self { get { return this; }}

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<ExecuteWatchRequestParameters> pathInfo)
		{
			ExecuteWatchInfo.Update(pathInfo, this);
		}

		TriggerEventContainer IExecuteWatchRequest.TriggerEvent { get; set; }
		bool? IExecuteWatchRequest.IgnoreCondition { get; set; }
		bool? IExecuteWatchRequest.RecordExecution { get; set; }
		bool? IExecuteWatchRequest.IgnoreThrottle { get; set; }
		IDictionary<string, object> IExecuteWatchRequest.AlternativeInput { get; set; }
	    IDictionary<string, ActionExecutionMode> IExecuteWatchRequest.ActionModes { get; set; }
	    SimulatedActions IExecuteWatchRequest.SimulatedActions { get; set; }

		public ExecuteWatchDescriptor TriggerEvent(Func<TriggerEventDescriptor, TriggerEventContainer> selector)
		{
			Self.TriggerEvent = selector == null ? null : selector(new TriggerEventDescriptor());
			return this;
		}

		public ExecuteWatchDescriptor IgnoreCondition(bool ignore = true)
		{
			Self.IgnoreCondition = ignore;
			return this;
		}

		public ExecuteWatchDescriptor RecordExecution(bool record = true)
		{
			Self.RecordExecution = record;
			return this;
		}

		public ExecuteWatchDescriptor IgnoreThrottle(bool ignore = true)
		{
			Self.IgnoreThrottle = ignore;
			return this;
		}
		
		public ExecuteWatchDescriptor ActionModes(Func<FluentDictionary<string, ActionExecutionMode>, IDictionary<string, ActionExecutionMode>> selector)
		{
			Self.ActionModes = selector == null ? null : selector(new FluentDictionary<string, ActionExecutionMode>());
			return this;
		}

		public ExecuteWatchDescriptor AlternativeInput(Func<FluentDictionary<string, object>, IDictionary<string, object>> selector)
		{
			Self.AlternativeInput = selector == null ? null : selector(new FluentDictionary<string, object>());
			return this;
		}

		public ExecuteWatchDescriptor SimulatedActions(SimulatedActions simulatedActions)
		{
			Self.SimulatedActions = simulatedActions;
			return this;
		}

	}
}
