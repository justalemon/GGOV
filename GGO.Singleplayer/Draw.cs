using GGO.Shared;
using GTA;
using System.Drawing;

namespace GGO.Singleplayer
{
    public class Draw : Toolkit
    {
        public Draw(Configuration Config) : base(Config) { }

        public override void Image(string File, PointF Position, SizeF Sizes)
        {
            UI.DrawTexture(File, 0, 0, 100, LiteralPoint(Position), LiteralSize(Sizes));
        }

        public override void Rectangle(PointF Position, SizeF Sizes, Color Colour)
        {
            UIRectangle Rect = new UIRectangle(LiteralPoint(Position), LiteralSize(Sizes), Colour);
            Rect.Draw();
        }

        public override void Text(string Text, PointF Position, float Scale, int Font = 0, bool Center = false)
        {
            UIText Name = new UIText(Text, LiteralPoint(Position), Scale, Color.White, (GTA.Font)Font, Center);
            Name.Draw();
        }

        public Point LiteralPoint(PointF Relative)
        {
            return new Point((int)(Relative.X * UI.WIDTH), (int)(Relative.Y * UI.HEIGHT));
        }

        public Size LiteralSize(SizeF Relative)
        {
            return new Size(LiteralPoint(new PointF(Relative.Width, Relative.Height)));
        }
    }
}
