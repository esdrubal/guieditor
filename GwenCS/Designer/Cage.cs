using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Designer
{
    class Cage : Gwen.Control.Button
    {
        bool                m_bDragged;
		Gwen.Control.Base	m_Control;
		int					m_iBorder;

        Point               m_DragOffset;

        public Gwen.Control.Base Target
        {
            get
            {
                return m_Control;
            }
        }

        public delegate void CagePositionEventHandler(Cage control, Point position);

        public event CagePositionEventHandler Moving;
   
        

        public Cage(Gwen.Control.Base parent)
            : base(parent)
        {
	        m_Control = null;
	        m_iBorder = 5;
            m_bDragged = false;
        }


		public void Setup( Gwen.Control.Base pControl )
        {
	        m_Control = pControl;
        }

		protected override void Render( Gwen.Skin.Base skin )
        {
            var bounds = new Rectangle(0,0,Width,Height);

	        //skin.Renderer.DrawColor = Color.FromArgb(100, 255, 255, 255);
            //skin.Renderer.DrawFilledRect(bounds);

	        skin.Renderer.DrawColor = Color.FromArgb(150, 0, 0, 0);
            skin.Renderer.DrawLinedRect(bounds);
        }

        protected override void PostLayout(Gwen.Skin.Base skin)
        {
	        if ( m_Control == null ) 
                return;

            Point canvaspos = m_Control.LocalPosToCanvas(Point.Empty);
            Point parentpos = Parent.CanvasPosToLocal(canvaspos);

	        parentpos.X -= m_iBorder;
	        parentpos.Y -= m_iBorder;

	        SetPosition(parentpos.X,parentpos.Y);

	        var width = m_Control.Width + m_iBorder * 2;
	        var height =  m_Control.Height + m_iBorder * 2;

	        SetSize( width, height );
        }
        
		protected override void OnMouseMoved( int x, int y, int deltaX, int deltaY )
        {
            if (IsDepressed)
            {
                if (!m_bDragged)
                {
                    var pPos = Parent.CanvasPosToLocal(new Point(x,y));
                    m_DragOffset = new Point(pPos.X - m_Control.X, pPos.Y - m_Control.Y);
                    m_bDragged = true;
                }

                Moving.Invoke(this, new Point(x - m_DragOffset.X, y - m_DragOffset.Y));

            } else {
                m_bDragged = false;
            } 
        }
    }
}
