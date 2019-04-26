using GGO.API;
using GTA;

namespace GGO.Example
{
    public class ExampleScript : Script
    {
        public ExampleScript()
        {
            // When the script is booting up, add the player field
            GGO.AddField(new TextExample(), FieldSection.Player);
            // And the squad field
            GGO.AddField(new TextExample(), FieldSection.Squad);
        }
    }

    public class TextExample : IText
    {
        public bool Visible => true;

        public string Icon => "Alive";

        public string Title => "An Example";

        public string Text => "of the fields";
    }
}
