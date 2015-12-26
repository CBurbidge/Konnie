using System;
using System.Linq;
using System.Reflection;
using Konnie.Model.Tasks;
using Konnie.Model.Tasks.SubTasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Konnie
{
	public class SubTaskJsonConverter : JsonConverter
	{
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
			
			return SubTypeToObject(value, jObject);
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