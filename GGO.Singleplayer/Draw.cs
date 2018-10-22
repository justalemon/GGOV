using GGO.Common;
using GGO.Common.Properties;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Drawing;

namespace GGO.Singleplayer
{
    public class Draw : Drawing
    {
        public Draw(Configuration Config) : base(Config) { }

        public override void Image(string File, Point Position, Size Sizes)
        {
            UI.DrawTexture(File, 0, 0, 100, Position, Sizes);
        }

        public override void Rectangle(Point Position, Size Sizes, Color Colour)
        {
            UIRectangle Rect = new UIRectangle(Position, Sizes, Colour);
            Rect.Draw();
        }

        public override void Text(string Text, Point Position, float Scale, int Font = 0, bool Center = false)
        {
            UIText Name = new UIText(Text, Position, Scale, Color.White, (GTA.Font)Font, Center);
            Name.Draw();
        }
    }

    public class OldDraw
    {
        /// <summary>
        /// Draws the dead marker on top of dead peds heads.
        /// </summary>
        /// <param name="Config">The mod settings.</param>
        /// <param name="Character">The ped to get the information.</param>
        /// <param name="Count">The dead ped count.</param>
        public static void DeadMarker(Configuration Config, Ped Character)
        {
            // Get the coordinates for the head of the dead ped.
            Vector3 HeadCoord = Character.GetBoneCoord(Bone.SKEL_Head);

            // Calculate the distance between player and dead ped's head.
            Size MarkerSize = Calculations.GetMarkerSize(Config, Vector3.Distance(Game.Player.Character.Position, HeadCoord));

            // Offset the marker by half width to center, and full height to put on top.
            Point MarkerPosition = UI.WorldToScreen(HeadCoord);
            MarkerPosition.Offset(-MarkerSize.Width / 2, -MarkerSize.Height);

            // Finally, draw the dead marker.
            UI.DrawTexture(Images.ResourceToPNG(Resources.DeadMarker, "DeadMarker" + Character.GetHashCode()), 0, 0, 100, MarkerPosition, MarkerSize);
        }
    }
}
