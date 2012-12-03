using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    class DocumentInner : Gwen.Control.ScrollControl
    {
	
        public DocumentInner(Gwen.Control.Base parent )
            : base(parent)
	    {
	    }

	    protected override void Render(Gwen.Skin.Base skin)
	    {
		    skin.Renderer.DrawColor = Color.FromArgb( 255, 255, 255, 255 );
		    skin.Renderer.DrawFilledRect( RenderBounds );
	    }

	    protected override void RenderOver(Gwen.Skin.Base skin)
	    {
            skin.Renderer.DrawColor = Color.FromArgb(255, 90, 90, 90);
            skin.Renderer.DrawLinedRect( RenderBounds);
	    }
};
}
