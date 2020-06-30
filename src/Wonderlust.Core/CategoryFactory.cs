using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class CategoryFactory : ICategoryFactory
    {
        // TODO: Configurable
        public CategoryFactory()
        {
            DirectoryCategory = new Category("Directory", Color.FromArgb(251, 83, 83));
            executableCategory = new Category("Executable", Color.FromArgb(83, 251, 83));
            defaultCategory = new Category("Default", Color.FromArgb(167, 167, 167));
            hiddenCategory = new Category("Hidden", Color.FromArgb(167, 0, 167));
            pictureCategory = new Category("Picture", Color.FromArgb(0, 167, 0));
        }

        public Category DirectoryCategory { get; }
        Category executableCategory;
        Category defaultCategory;
        Category hiddenCategory;
        Category pictureCategory;

        public Category GetCategory(IContainerItem item)
        {
            if (item.Name.StartsWith("."))
            {
                return hiddenCategory;
            }

            if (item.Name.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase))
            {
                return executableCategory;
            }

            if (item.Name.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase))
            {
                return pictureCategory;
            }

            return defaultCategory;
        }
    }
}
