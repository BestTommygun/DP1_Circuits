using Circuits.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DP1_Circuits.parsing
{
    public class ParserFactory
    {
        private readonly Dictionary<string, Func<IParser>> _parsers = typeof(ParserFactory).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IParser)))
            .Where(t => t.IsClass)
            .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
            .SelectMany(t => t.GetCustomAttributes<ParserAttribute>()
                .Select(a => new Tuple<string, Func<IParser>>(a.Name, () =>
                    { return (IParser)t.GetConstructors().Single(c => c.GetParameters().Length == 0).Invoke(new object[0]); })))
            .ToDictionary(t => t.Item1, t => t.Item2);

        public IParser ReturnParser(string file)
        {
            if(!string.IsNullOrEmpty(file))
            {
                return _parsers[Path.GetExtension(file)]();
            }
            else throw new ArgumentNullException();
        }
    }
}
