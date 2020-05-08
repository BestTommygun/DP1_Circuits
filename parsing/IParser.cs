using Circuits.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.parsing
{
    public interface IParser
    {
        /// <summary>
        /// Returns the data from an input in ParserEntity format
        /// </summary>
        /// <param name="file">The Stream</param>
        List<ParserData> parse(Stream file);
    }
}
