using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    class ControlToolbox : Gwen.Control.GroupBox
    {

        public ControlToolbox(Gwen.Control.Base parent)
            : base(parent)
        {
	        Width = 150;
	        Margin = new Gwen.Margin( 5, 5, 5, 5 );
	        SetText( "Controls" );

	        var pTileLayout = new Gwen.Control.Layout.Tile( this );
            pTileLayout.SetTileSize(22, 22);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var controlType in assembly.GetTypes().Where<Type>(type => type.IsSubclassOf(typeof(Gwen.Control.Base))))
	            {

                    if (controlType == typeof(DesignerBase)) continue;

                    var pButton = new Gwen.Control.Button(pTileLayout);
		            pButton.SetSize( 20, 20 );
                    pButton.SetToolTipText(controlType.Name);
                    //pButton.SetImage("img/controls/" + controlType.Name + ".png");
		            pButton.ShouldDrawBackground = true;

		            //
		            // Make drag and droppable. Pass the ControlFactory as the userdata
		            //
                    pButton.DragAndDrop_SetPackage(true, "ControlSpawn", controlType);//,pControlFactory );
	            }

        }
    }
}
