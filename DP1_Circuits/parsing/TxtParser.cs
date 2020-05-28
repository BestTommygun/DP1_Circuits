using Circuits.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.parsing
{
    [Parser(".txt")]
    public class TxtParser : IParser
    {
        public List<ParserData> Parse(string fileName, Stream file)
        {
            List<ParserData> parserData = new List<ParserData>();
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;
                string[] commentTokens = new string[1];
                commentTokens[0] = "#";
                parser.CommentTokens = commentTokens;
                parser.SetDelimiters(";");
                parser.SetDelimiters(":");

                string[] curData = parser.ReadFields();
                //get nodes
                while (!parser.EndOfData)
                {
                    curData[0] = curData[0].Trim(';');
                    if(parserData.Exists(pd => pd.Id.Contains(curData[0]))) break;
                    parserData.Add(new ParserData()
                    {
                        Id = curData[0],
                        Type = curData[1].Trim(';'),
                        Ouputs = new List<string>()
                });
                    curData = parser.ReadFields();
                }

                bool lastLine = false;

                //get node connections
                while (!parser.EndOfData || lastLine)
                {
                    var curParserData = parserData.Find(pd => pd.Id.Equals(curData[0]));
                    curParserData.Ouputs.AddRange(curData.Skip(1).First().Split(',').ToList());

                    List<string> newInputs = new List<string>();
                    curParserData.Ouputs.ForEach(node => newInputs.Add(node.Trim(';')));
                    curParserData.Ouputs = newInputs;

                    int index = parserData.FindIndex(pd => pd.Id.Equals(curParserData.Id));
                    parserData[index] = curParserData;

                    curData = parser.ReadFields();
                    //parser ignores the last line unless we do this
                    if (lastLine) break;
                    if (parser.EndOfData) 
                        lastLine = true;
                }
            }
            ParserData header = new ParserData
            {
                HeaderData = Path.GetFileName(fileName)
            };
            parserData.Insert(0, header);

            return parserData;
        }
    }
}
