using Circuits.Models;
using DP1_Circuits.builders;
using DP1_Circuits.parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.controllers
{
    public class MainController
    {
        private ViewController _viewController;
        private ModelController _modelController;
        private ParserFactory _parserFactory;
        private CircuitBuilder _circuitBuilder;

        public MainController()
        {
            _modelController = new ModelController();
            _parserFactory = new ParserFactory();
            _viewController = new ViewController();
            _viewController.fileOpened += (string file) => loadFile(file);

            _viewController.runView();
        }

        public void loadFile(string file) //TODO: never fires the first click for some reason? maybe remove viewcontroller event
        {
            if(!string.IsNullOrEmpty(file))
            {
                _circuitBuilder = new CircuitBuilder();
                FileStream fileStream = new FileStream(file, FileMode.Open);
                List<ParserData> parserData = _parserFactory.returnParser(file).parse(fileStream);
                _modelController.setCircuit(_circuitBuilder.buildCircuit(parserData));
            }
        }
    }
}
