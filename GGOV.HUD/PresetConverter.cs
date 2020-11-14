using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GGO
{
    /// <summary>
    /// Converter for the presets.
    /// </summary>
    public class PresetConverter : JsonConverter<PresetMenu>
    {
        public override PresetMenu ReadJson(JsonReader reader, Type objectType, PresetMenu value, bool hasValue, JsonSerializer serializer)
        {
            JObject @object = JObject.Load(reader);
            return new PresetMenu((int)@object["sx"], (int)@object["sy"], (int)@object["px"], (int)@object["py"], (string)@object["name"]);
        }

        public override void WriteJson(JsonWriter writer, PresetMenu value, JsonSerializer serializer)
        {
            JObject @object = new JObject
            {
                ["name"] = value.Subtitle,
                ["sx"] = value.SquadX.SelectedItem,
                ["sy"] = value.SquadY.SelectedItem,
                ["px"] = value.PlayerX.SelectedItem,
                ["py"] = value.PlayerY.SelectedItem
            };
            @object.WriteTo(writer);
        }
    }
}
