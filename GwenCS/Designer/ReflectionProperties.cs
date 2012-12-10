using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    public class ReflectionProperties : Gwen.Control.Base
    {
        public Gwen.Control.Base Target { get; private set; }
        public Gwen.Control.PropertyTree ptree;

        public ReflectionProperties(Gwen.Control.Base parent)
            : base(parent)
        {
            ptree = new Gwen.Control.PropertyTree(this);
            //properties = new Gwen.Control.Properties(this);
            ptree.Dock = Gwen.Pos.Fill;
        }

        public void Setup(Gwen.Control.Base target)
        {
           
            if(Target != null)
             Target.PropertyChanged -= TargetPropertyChanged;
            
            Target = target;

            UpdateProperties();

            target.PropertyChanged += TargetPropertyChanged;

        }

        private void UpdateProperties()
        {
            ptree.RemoveAll();
            var type = Target.GetType();

            var tFields = type.GetFields();
            var tProps = type.GetProperties();
            if (tFields.Count() > 0 || tProps.Count() > 0)
            {
                var props = ptree.Add(type.Name);
                foreach (var f in tFields)
                {
                    var row = props.Add(f.Name, "" + f.GetValue(Target));
                    row.Name = f.Name;
                }
                foreach (var p in tProps)
                {
                    var row = props.Add(p.Name, "" + p.GetValue(Target));
                    row.Name = p.Name;
                }
            }
            
            ptree.ExpandAll();
        }

        private void TargetPropertyChanged(object o, System.ComponentModel.PropertyChangedEventArgs args)
        {
            var prop = ptree.FindChildByName(args.PropertyName, true) as Gwen.Control.PropertyRow;
            if (prop != null)
            {
                var f = Target.GetType().GetField(args.PropertyName);
                if (f != null)
                {
                    prop.Value = f.GetValue(Target).ToString();
                }
                else
                {
                    var p = Target.GetType().GetProperty(args.PropertyName);
                    if (p != null)
                    {
                        prop.Value = p.GetValue(Target).ToString();
                    }
                }
            }
        }
    }
}
