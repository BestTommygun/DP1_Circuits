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
        private readonly ViewController _viewController;
        private readonly ModelController _modelController;
        private readonly InputController _inputController;
        private readonly ParserFactory _parserFactory;
        private CircuitBuilder _circuitBuilder;

        public MainController()
        {
            _modelController = new ModelController();
            _inputController = new InputController();
            _inputController.SetDefaultSetup(this);
            _parserFactory = new ParserFactory();
            _viewController = new ViewController();
            _viewController.FileOpened += (string file) => LoadFile(file);
            _viewController.RunView();
        }

        public void LoadFile(string file) 
        {
            if(!string.IsNullOrEmpty(file))
            {
                _circuitBuilder = new CircuitBuilder();

                FileStream fileStream = new FileStream(file, FileMode.Open);
                List<ParserData> parserData = _parserFactory.ReturnParser(file).Parse(file, fileStream);
                _modelController.SetCircuit(_circuitBuilder?.BuildCircuit(parserData, ShowErrorPopup));

                _viewController.DrawFrame(_modelController.GetNodes());
                Program.log.Invoke("> Loaded circuit");
            }
        }
        public void ShowErrorPopup(string message)
        {
            _viewController.ShowErrorPopup(message);
        }
        #region inputView_command_methods
        public Tuple<List<Tuple<BaseNode, bool>>, double> RunSimulation()
        {
            var results = _modelController.RunSim();
            _viewController.DrawFrame(_modelController.GetNodes());
            return results;
        }
        public void ResetCircuit()
        {
            _modelController.ResetNodes();
        }
        public List<InputNode> GetInputs()
        {
            return _modelController.GetInputs();
        }
        public string GetCircuitName()
        {
            return _modelController.GetCircuitName();
        }
        public void InsertCircuit()
        {
            _viewController.DrawFrame(_modelController.GetNodes());
        }
        #endregion
    }
}
