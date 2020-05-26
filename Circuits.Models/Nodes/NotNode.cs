using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class NotNode : BaseNode //TODO: check if this is even allowed to have multiple inputs
    {
        public NotNode(string id) : base(id)
        { }
    }
}
