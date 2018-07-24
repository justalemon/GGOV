using GTA;
using System.Drawing;

namespace GGOHud
{
    class Draw
    {
        public static void Text(string Message, Point Position, float FontSize = 0.475f)
        {
            UIText ToDraw = new UIText(Message, Position, FontSize);
            ToDraw.Draw();
        }

        public static void Dummy(Point Position)
        {
            UIText ToDraw = new UIText("-", Position, 0.475f);
            ToDraw.Draw();
        }

        public static void Texture(string Filename, Point Position, Size DrawSize)
        {
            UI.DrawTexture(Filename, 0, 0, 200, Position, DrawSize);
        }

        public static void Rectangle(Point Position, Size DrawSize, Color ShapeColor)
        {
            UIRectangle ToDraw = new UIRectangle(Position, DrawSize, ShapeColor);
            ToDraw.Draw();
        }
    }
}
