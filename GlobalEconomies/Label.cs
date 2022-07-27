using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEconomies
{
    public class Label
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public Label? parent { get; private set; }
        public List <Label> children { get; private set; }

        public Label(string name, string value)
        {
            Name = name;
            Value = value;
            children = new List<Label>();
        }

        public Label(string name)
        {
            Name = name;
            children = new List<Label>();
        }

        public void AddChild(string name, string value)
        {
            Label lbl = new Label(name, value);
            lbl.parent = this;
            children.Add(lbl);
        }

        public void AddChild(Label l)
        {
            l.parent = this;
            children.Add(l);
        }

        public Label FindLabelByName(string labelName) 
        {
            if (Name == labelName)
            {
                return this;
            }
            else
            {
                foreach (Label l in children)
                {
                    Label found = l.FindLabelByName(labelName);
                    if (!string.IsNullOrEmpty(found.Name))
                    {
                        return found;
                    }
                }
            }
            return new Label("");
        }

        public Label FindLabelByValue(string labelValue)
        {
            if (Value == labelValue)
            {
                return this;
            }
            else
            {
                foreach (Label l in children)
                {
                    Label found = l.FindLabelByValue(labelValue);
                    if (!string.IsNullOrEmpty(found.Name))
                    {
                        return found;
                    }
                }
            }
            return new Label("");
        }
    }
}
