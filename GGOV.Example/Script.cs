using GGO.API;
using GTA;

namespace GGO.Example
{
    public class ExampleScript : Script
    {
        public ExampleScript()
        {
            // When the script is booting up, add the player field
            Hud.AddField(new HealthExample(), FieldSection.Player);
            // And the squad field
            Hud.AddField(new HealthExample(), FieldSection.Squad);
        }
    }

    public class HealthExample : Field
    {
        public override bool DataShouldBeShown()
        {
            // This determines if the weapon data should be shown (ammo count and image)
            throw new System.NotImplementedException();
        }

        public override float GetCurrentValue()
        {
            // This is the current value for the health bar
            return 75;
        }

        public override FieldType GetFieldType()
        {
            // The type of field to use
            return FieldType.Text;
        }

        public override string GetFirstText()
        {
            // Text of the top part of the field
            return "An Example";
        }

        public override string GetIconName()
        {
            // The name for the field icon
            // In this example, "Alive" becomes "IconAlive.png"
            return "Alive";
        }

        public override float GetMaxValue()
        {
            // The maximum value for the health bar
            return 100;
        }

        public override string GetSecondText()
        {
            // The text to replace the health bar
            // If you really want to show the health bar, set this to null or string.Empty
            return "of the fields";
        }

        public override string GetWeaponImage()
        {
            // The weapon image, this does not needs to be implemented in the health field
            throw new System.NotImplementedException();
        }

        public override bool ShouldBeShown()
        {
            // If this field should be shown during the next game tick
            return true;
        }
    }
}
