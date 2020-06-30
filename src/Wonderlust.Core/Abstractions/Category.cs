using System.Drawing;

namespace Wonderlust.Core.Abstractions
{
    public class Category
    {
        public string Name { get; }
        public Color Color { get; }

        public Category(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}