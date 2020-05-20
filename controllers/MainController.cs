using Circuits.Models;
using Circuits.Models.Nodes;
using DP1_Circuits.builders;
using DP1_Circuits.parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DP1_Circuits.controllers
{
    public class MainController
    {
        private ViewController _viewController;
        private ModelController _modelController;
        private InputController _inputController;
        private ParserFactory _parserFactory;
        private CircuitBuilder _circuitBuilder;

        public MainController()
        {
            _modelController = new ModelController();
            _inputController = new InputController();
            _inputController.SetDefaultSetup(this);
            _parserFactory = new ParserFactory();
            _viewController = new ViewController();
            _viewController.FileOpened += (string file) => loadFile(file);
            _viewController.RunView();
        }

        public void loadFile(string file) 
        {
            if(!string.IsNullOrEmpty(file))
            {
                _circuitBuilder = new CircuitBuilder();
                FileStream fileStream = new FileStream(file, FileMode.Open);
                List<ParserData> parserData = _parserFactory.returnParser(file).parse(fileStream);
                _modelController.setCircuit(_circuitBuilder?.BuildCircuit(parserData, showErrorPopup));
                _viewController.DrawFrame(_modelController.getNodes());
                Program.log.Invoke("> Loaded circuit");
            }
        }
        public void showErrorPopup(string message)
        {
            _viewController.ShowErrorPopup(message);
        }
        #region inputView_command_methods
        public Tuple<List<Tuple<BaseNode, bool>>, double> runSimulation()
        {
            var results = _modelController.runSim();
            _viewController.DrawFrame(_modelController.getNodes());
            return results;
        }
        public void resetCircuit()
        {
            _modelController.resetNodes();
        }
        public List<InputNode> getInputs()
        {
            return _modelController.getInputs();
        }
        public void insertCircuit(string circuitId)
        {

            _viewController.DrawFrame(_modelController.getNodes());
        }
        #endregion
    }
}
