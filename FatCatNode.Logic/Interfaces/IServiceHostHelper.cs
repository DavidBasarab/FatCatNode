﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatNode.Logic.Interfaces
{
    public interface IServiceHostHelper
    {
        void OpenServiceHost(INode nodeInstance, Uri baseAddress);
    }
}
