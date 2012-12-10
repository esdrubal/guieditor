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

	        AddControlNode( m_Tree, m_pCanvas );

	        m_Tree.ExpandAll();
        }

		
		void AddControlNode( Gwen.Control.TreeNode pNode, Gwen.Control.Base pControl )
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
            //pChildNode.ShouldDrawBackground = true;
            pChildNode.DragAndDrop_SetPackage(true, "ControlHierarchy", pChildNode);
            pChildNode.DragAndDropCanAcceptPackage += new Func<Gwen.Control.Base, Gwen.DragDrop.Package, bool>(
                delegate(Gwen.Control.Base c, Gwen.DragDrop.Package p)
                {
                    return p.Name == "ControlHierarchy";
                });
            pChildNode.DragAndDropHandleDrop += new Func<Gwen.Control.Base, Gwen.DragDrop.Package, int, int, bool>(
                delegate(Gwen.Control.Base control, Gwen.DragDrop.Package p, int x, int y)
                {
                    var childNode = p.data as Gwen.Control.TreeNode;
                    var node = control as Gwen.Control.TreeNode;
                    if (childNode == null || node == null)
                        return false;
                    node.AddNode(childNode);
                    return true;
                });

            foreach (var child in pControl.Children)
            {
		        AddControlNode( pChildNode, child );
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
