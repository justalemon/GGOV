using GGO.Shared;
using GTA;
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
}
