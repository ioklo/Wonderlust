using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    public interface IHistory
    {
        void Add();

        void Back();
        void Forward();
    }
}
