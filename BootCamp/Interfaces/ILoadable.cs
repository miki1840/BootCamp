using System.Collections;
using System.Collections.Generic;

namespace BootCamp.Interfaces
{
    public interface ILoadable
    {
        IEnumerable load(string path);
    }
}