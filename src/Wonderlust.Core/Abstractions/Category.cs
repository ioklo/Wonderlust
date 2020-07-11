using System.Drawing;

namespace Wonderlust.Core.Abstractions
{
    public class Category
    {
        public int Priority { get; }
        public int DefOrder { get; }
        public string Name { get; }
        public Color Color { get; }

        public Category(int priority, int defOrder, string name, Color color)
        {
            Priority = priority;
            DefOrder = defOrder;

            Name = name;
            Color = color;
        }
    }
}