using System;
using Konnie.Model.Tasks;
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
			var taskName = jObject[nameof(ISubTask.TaskName)];
			var value = taskName.Value<string>();
			switch (value)
			{
				case (nameof(TransformTask)):
					return jObject.ToObject<TransformTask>();
				case (nameof(SubstitutionTask)):
					return jObject.ToObject<SubstitutionTask>();
				default:
					throw new Exception("Don't know the task :" + value);
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof (ISubTask).IsAssignableFrom(objectType);
		}
	}
}