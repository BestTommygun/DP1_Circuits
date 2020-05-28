using Circuits.Models.Nodes.NodeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Circuits.Models.Nodes
{
    public abstract class BaseNode
    {
        public string Id { get; }
        public Tuple<bool, double> SavedOutput {
            get
            {
                if (components.OfType<VisualComponent>().Any())
                    return components.OfType<VisualComponent>().FirstOrDefault().SavedOutput;
                else return null;
            }
            set
            {
                if (components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().SavedOutput = value;
            }
        }
        public int X {
            get
            {
                if (components.OfType<VisualComponent>().Any())
                    return components.OfType<VisualComponent>().FirstOrDefault().X;
                else throw new InvalidOperationException();
            }
            set
            {
                if (components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().X = value;
            }
        }
        public int Y
        {
            get
            {
                if (components.OfType<VisualComponent>().Any())
                    return components.OfType<VisualComponent>().FirstOrDefault().Y;
                else throw new InvalidOperationException();
            }
            set
            {
                if (components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().Y = value;
            }
        }

        protected List<Component> components;
        #region Constructors
        public BaseNode()
        {
            inputs = new List<BaseNode>();
            components = new List<Component>();
            this.Delay = 15;
            this.X = 0;
            this.Y = 1;
            Id = "";
        }
        public BaseNode(string Id)
        {
            inputs = new List<BaseNode>();
            components = new List<Component>();
            this.Delay = 15;
            this.X = 0;
            this.Y = 1;
            this.Id = Id;
        }
        public BaseNode(string Id, List<BaseNode> inputs, double delay)
        {
            this.inputs = inputs;
            components = new List<Component>();
            this.Delay = delay;
            this.X = 0;
            this.Y = 1;
            this.Id = Id;
        }
        public BaseNode(string Id, List<BaseNode> inputs)
        {
            this.inputs = inputs;
            components = new List<Component>();
            this.Delay = 15;
            this.X = 0;
            this.Y = 1;
            this.Id = Id;
        }
        public BaseNode(string Id, List<Component> components, double delay)
        {
            this.components = components;
            this.inputs = new List<BaseNode>();
            this.Delay = delay;
            this.X = 0;
            this.Y = 1;
            this.Id = Id;
        }
        public BaseNode(string Id, List<Component> components)
        {
            this.components = components;
            this.inputs = new List<BaseNode>();
            this.Delay = 15;
            this.X = 0;
            this.Y = 1;
            this.Id = Id;
        }
        public BaseNode(string Id, List<Component> components, List<BaseNode> inputs, double delay)
        {
            this.components = components;
            this.inputs = inputs;
            this.Delay = delay;
            this.X = 0;
            this.Y = 1;
            this.Id = Id;
        }
        #endregion

        protected List<BaseNode> inputs;
        public double Delay { get; set; }

        public virtual Tuple<bool, double> CalcOutput() {

            if (components.OfType<VisualComponent>().Any() && components.OfType<VisualComponent>().FirstOrDefault().SavedOutput != null)
                return components.OfType<VisualComponent>().FirstOrDefault().SavedOutput;
            else if (components.OfType<CalcComponent>().Any())
            {
                var savedOutput = components.OfType<CalcComponent>().FirstOrDefault().calcOutput();
                if(components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().SavedOutput = savedOutput;
                return savedOutput;
            }

            else
                return new Tuple<bool, double>(false, 15);
        }

        public void Reset()
        {
            SavedOutput = null;
            foreach (var input in inputs)
            {
                input.Reset();
            }
        }
        public void AddComponent(Component component) 
        {
            if (components == null) components = new List<Component>();
            components.Add(component);
        }
        public void RemoveComponent(Component component)
        {
            if (components != null)
            {
                components.Remove(component);
            }

        }
        public virtual List<BaseNode> GetInputs()
        {
            return inputs;
        }
        public virtual void SetInputs(List<BaseNode> newInputs)
        {
            inputs = newInputs;
        }
        public virtual void AddInput(BaseNode newInput)
        {
            if (inputs == null) inputs = new List<BaseNode>();
            inputs.Add(newInput);
        }
        public bool ContainsComponent(Component component)
        {
            return components.Contains(component);
        }
        public VisualComponent GetVisualComponent()
        {
            if (components.OfType<VisualComponent>().Any())
                return components.OfType<VisualComponent>().FirstOrDefault();
            return null;
        }
        public void InsertCircuit(Circuit circuit, List<BaseNode> outputs, Action<string> showErrorPopup)
        {
            var cInputs = circuit.GetInputs();
            if(IsValid(circuit, outputs))
            {
                for (int i = 0; i < outputs.Count; i++)
                {
                    var curOutput = outputs[i];
                    curOutput.SetInputs(circuit.EndNodes[i].GetInputs());
                }
                inputs = circuit.EndNodes;
                for (int i = 0; i < cInputs.Count; i++)
                {
                    //var curInput = outputs
                }
            }
            else
            {
                showErrorPopup.Invoke("Circuit is invalid, inputs or outputs don't match up");
            }

        }
        /// <summary>
        /// checks if the circuit is suitable to be inserted
        /// </summary>
        /// <param name="circuit">the circuit that will be inserted</param>
        /// <returns>if the circuit is valid or not</returns>
        private bool IsValid(Circuit circuit, List<BaseNode> outputs)
        {
            if (inputs.Count != circuit.GetInputs().Count) return false;
            if (outputs.Count != circuit.EndNodes.Count) return false;
            return true;
        }
    }
}
