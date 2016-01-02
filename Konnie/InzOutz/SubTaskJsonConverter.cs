using System;
using System.Linq;
using Konnie.Model.Tasks;
using Konnie.Runner.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Konnie.InzOutz
{
	public class SubTaskJsonConverter : JsonConverter
	{
		private readonly ILogger _logger;

		public SubTaskJsonConverter(ILogger logger)
		{
			_logger = logger;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var jObject = JObject.Load(reader);
			var propertyName = nameof(ISubTask.Type);
			var taskName = jObject[propertyName];
			var value = taskName.Value<string>();

			var subTypeObject = SubTypeToObject(value, jObject);

			// Not very pretty...
			var toAddLoggerTo = subTypeObject as ISubTask;
			toAddLoggerTo.Logger = _logger;
			return toAddLoggerTo;
		}

		private static object SubTypeToObject(string value, JObject jObject)
		{
			var allSubTaskTypes = 
				AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(t => typeof (ISubTask).IsAssignableFrom(t));

			var subTaskType = allSubTaskTypes.Where(t => t.Name == value);

			return jObject.ToObject(subTaskType.Single());
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof (ISubTask).IsAssignableFrom(objectType);
		}
	}
}