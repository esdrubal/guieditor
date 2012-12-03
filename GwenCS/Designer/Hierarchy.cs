using Gwen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    class Hierarchy : Gwen.Control.Base
    {

        DocumentCanvas			    m_pCanvas;
		Gwen.Control.TreeControl	m_Tree;
        Gwen.Control.Base           targetControl;

        public Hierarchy( Gwen.Control.Base parent )
            :base(parent)
        {
	        SetSize( 200, 200 );

	        m_Tree = new Gwen.Control.TreeControl( this );
	        m_Tree.Dock = Pos.Fill;
        }

		public void WatchCanvas( DocumentCanvas pCanvas )
        {
	        m_pCanvas = pCanvas;
	        m_pCanvas.ChildAdded += OnCanvasChildAdded;
	        m_pCanvas.SelectionChanged += OnCanvasSelectionChanged;

	        CompleteRefresh();
        }

		void SelectNodeRepresentingControl( Gwen.Control.Base pControl, Gwen.Control.TreeNode pNode = null )
        {
	        if ( pNode == null ) pNode = m_Tree;

            if (pNode.UserData.ContainsKey("TargetControl") && pNode.UserData.Get<Gwen.Control.Base>("TargetControl") == pControl)
            {
                pNode.SetSelected(true, false);

            }
            else
            {
                foreach (var child in pNode.Children)
                {
                    var pChildNode = child as Gwen.Control.TreeNode;
                    if (pChildNode == null) continue;

                    SelectNodeRepresentingControl(pControl, pChildNode);
                }

            }

        }

		public void CompleteRefresh()
        {
	        m_Tree.RemoveAll();

	        UpdateNode( m_Tree, m_pCanvas );

	        m_Tree.ExpandAll();
        }

		
		void UpdateNode( Gwen.Control.TreeNode pNode, Gwen.Control.Base pControl )
        {
            if (pControl.GetType() == typeof(SelectionLayer))
                return;

		    var strName = pControl.Name;
		    if ( strName == null || strName == "" ) 
                strName = "[" + pControl.GetType().Name + "]";

		    var pChildNode = pNode.AddNode( strName );
            //pChildNode.SetImage("img/controls/" + pControl.GetType().Name + ".png");
		    pChildNode.Selected += OnNodeSelected;
            pChildNode.UserData.Add("TargetControl", pControl);

            foreach (var child in pControl.Children)
            {
		        UpdateNode( pChildNode, child );
	        }
        }

        void OnCanvasChildAdded(Gwen.Control.Base caller)
        {
            CompleteRefresh();
        }

        void OnCanvasSelectionChanged(List<Gwen.Control.Base> controllers)
        {
	        m_Tree.UnselectAll();

	       foreach (var controller in controllers){
               SelectNodeRepresentingControl(controller, m_Tree);
	       }
        }

		void OnNodeSelected(Gwen.Control.Base caller)
        {
            var ctrl = caller.UserData.Get<Gwen.Control.Base>("TargetControl");
            if (ctrl == null) 
                return;  //TODO log

            var list = new List<Gwen.Control.Base>();
            list.Add(ctrl);

            m_pCanvas.SelectControls(list);
        }
		

    }
}
