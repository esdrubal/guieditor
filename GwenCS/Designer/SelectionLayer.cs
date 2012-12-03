using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    class SelectionLayer : Gwen.Control.Base
    {
        List<Gwen.Control.Base> m_Selected;

        public List<Gwen.Control.Base> Selected
        {
            get{
                return m_Selected;
            }
        }

        public delegate void SelectionLayerEventHandler(List<Gwen.Control.Base> selection);

        public event SelectionLayerEventHandler SelectionChanged;

        public SelectionLayer(Gwen.Control.Base parent)
            : base(parent)
        {
            m_Selected = new List<Gwen.Control.Base>();
            ShouldDrawBackground = true;
        }

        protected override void OnMouseClickedLeft(int x, int y, bool bDown)
        {
	        if ( !bDown ) 
                return;

	        var pPos = Parent.CanvasPosToLocal( new Point( x, y ) );
	
	        MouseInputEnabled = false;

	        var pChild = Parent.GetControlAt( pPos.X, pPos.Y );

	        MouseInputEnabled = true;

	        bool bPanelsWereSelected = !(m_Selected.Count > 0);

            //TODO shift control

             ClearSelection();
             AddSelection( pChild, true );
        }

	    public void ClearSelection()
        {
	        m_Selected.Clear();
	        DeleteAllChildren();
	        Redraw();
        }

        public void AddSelection(List<Gwen.Control.Base> pControls, bool fireEvent)
        {
            foreach (var control in pControls){
                AddSelection(control, false);
            }

            if (fireEvent && SelectionChanged != null)
                SelectionChanged.Invoke(m_Selected);

            Redraw();
        }

        public void AddSelection(Gwen.Control.Base pControl, bool fireEvent)
        {
	        Cage pCage = new Cage( this );
            pCage.Setup(pControl);
	        pCage.Moving += OnCageMoving;

	        m_Selected.Add( pControl );

            if (fireEvent && SelectionChanged != null)
                SelectionChanged.Invoke(m_Selected);

	        Redraw();
        }

        public void RemoveSelection(Gwen.Control.Base pControl)
        {
	        
            foreach (var s in m_Selected)
	        {
		        Cage pCage = s as Cage;
		        if  ( pCage == null ) 
                    continue;

		        if ( pCage.Target == pControl )
			        pCage.DelayedDelete();
	        }

            m_Selected.Remove(pControl);

	        Redraw();
        }

        void OnCageMoving(Cage cage, Point position )
        {
	        if ( cage.Target == Parent || cage.Target == this)
		        return;

	        var pPos = Parent.CanvasPosToLocal( position );

            cage.Target.SetPosition(pPos.X, pPos.Y);

        }

        void SwitchCage( Gwen.Control.Base pControl, Gwen.Control.Base pTo )
        {
	        foreach (var child in  Children)
	        {
		        Cage pCage = child as Cage;
		        if  ( pCage == null ) 
                    continue;

		        if ( pCage.Target == pControl )
		        {
			        pCage.Setup( pTo );
		        }
	        }
        }

		
    }
}
