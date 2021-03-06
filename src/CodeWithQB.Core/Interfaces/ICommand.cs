﻿using System.Collections.Generic;

namespace CodeWithQB.Core.Interfaces
{
    public interface ICommand<TResponse> 
    {
        string Key { get; }        
        IEnumerable<string> SideEffects { get; }
    }
}
