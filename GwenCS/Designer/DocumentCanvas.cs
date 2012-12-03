using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    class DocumentCanvas : Gwen.Control.Base
    {
        SelectionLayer	m_SelectionLayer;

        public event Designer.SelectionLayer.SelectionLayerEventHandler SelectionChanged {
            add 
            {
                m_SelectionLayer.SelectionChanged += value;
            }
            remove
            {
                m_SelectionLayer.SelectionChanged -= value;
            }
        }

        public event GwenEventHandler	ChildAdded;
	    public event GwenEventHandler	ChildRemoved;
	    public event GwenEventHandler	PropertiesChanged;
	    public event GwenEventHandler	HierachyChanged;

        public DocumentCanvas(Gwen.Control.Base parent)
            : base(parent)
       {
	        ShouldDrawBackground = true;
            
	        m_SelectionLayer = new SelectionLayer( this );
           /* m_SelectionLayer.PropertiesChanged += OnPropertiesChanged;
            m_SelectionLayer.HierachyChanged  +=  OnHierachyChanged;

            ControlFactory::Base* pControlFactory = Gwen::ControlFactory::Find( "DesignerCanvas" );
            UserData.Set( "ControlFactory", pControlFactory );*/
        }

	protected override void Render( Gwen.Skin.Base skin )
    {
	    //skin.DrawGenericPanel( this );
    }

	protected override void PostLayout( Gwen.Skin.Base skin )
    {
	    m_SelectionLayer.BringToFront();
	    m_SelectionLayer.SetBounds( 0, 0, Width, Height);
    }


	public void SelectControls(List<Gwen.Control.Base> list )
    {
	    m_SelectionLayer.ClearSelection();
        m_SelectionLayer.AddSelection(list,true);
    }

	public override bool DragAndDrop_CanAcceptPackage( Gwen.DragDrop.Package pPackage )
    {
	    return pPackage.Name == "ControlSpawn";
    }

    public override bool DragAndDrop_HandleDrop(Gwen.DragDrop.Package pPackage, int x, int y)
    {
	    var pPos = CanvasPosToLocal( new Point( x, y ) );

	    m_SelectionLayer.Hide();
	    var pDroppedOn = GetControlAt( pPos.X, pPos.Y );
	   // pDroppedOn = FindParentControlFactoryControl( pDroppedOn );
	    m_SelectionLayer.Show();

	    if ( pDroppedOn == null ) 
            pDroppedOn = this;

	    pPos = pDroppedOn.CanvasPosToLocal( new Point( x, y ) );
	

	    if ( pPackage.Name == "ControlSpawn" )
	    {
		    //ControlFactory::Base* pControlFactory = static_cast<ControlFactory::Base*>(pPackage->userdata);

            var type = pPackage.data as Type;
            if (type == null)
                return false; //TODO LOG

            var constructor = type.GetConstructor(new Type[] { typeof(Gwen.Control.Base) });
            if (constructor == null)
                return false; //TODO LOG

            var pControl = (Gwen.Control.Base) constructor.Invoke(new object[] { pDroppedOn });
            if (pControl == null)
                return false; //TODO LOG

            pControl.SetPosition(pPos.X, pPos.Y);
		    pControl.MouseInputEnabled = true ;

            if (ChildAdded != null)
		        ChildAdded.Invoke(pControl);

		    return true;
	    }
        
	    return false;
    }

/*
	void Command( const Gwen::String& str );


    void OnPropertiesChanged( Event::Info info )
    {
	    PropertiesChanged.Call( this, info );
    }

    void OnHierachyChanged( Event::Info info )
    {
	    HierachyChanged.Call( this, info );
    }*/
		
    }
}
