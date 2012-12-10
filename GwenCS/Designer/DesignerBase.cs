using Gwen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Designer 
{
    class DesignerBase : Gwen.Control.Base
    {

		Gwen.Control.TabControl	m_DocumentHolder;
        ControlToolbox m_ControlBox;

        public DesignerBase(Gwen.Control.Base parent)
            :base(parent)
        {
            base.Dock = Pos.Fill;

	        CreateMenu();
	        CreateToolBar();
	        CreateControlToolbox();
	        CreateDocumentHolder();

	        new Gwen.Control.StatusBar(this);// , "StatusBar" );

	        NewDocument(this);
        }

        void CreateMenu()
        {
            var pStrip = new Gwen.Control.MenuStrip(this );
	        pStrip.Dock = Pos.Top ;

	        // File
	        {
		        var pRoot = pStrip.AddItem( "File" );

		        pRoot.Menu.AddItem( "New", "img/menu/new.png", "Ctrl + N" ).SetAction(new GwenEventHandler(NewDocument));

		        //pRoot.Menu.AddItem( "Open", "img/menu/open.png", "Ctrl + O" ).SetAction(new GwenEventHandler(OpenDocument));
		       // pRoot.Menu.AddItem( "Save", "img/menu/save.png", "Ctrl + S" ).SetAction(new GwenEventHandler(SaveDocument));
		       // pRoot.Menu.AddItem( "Save As", "img/menu/save.png", "Ctrl + Shift + S" ).SetAction(new GwenEventHandler(SaveAsDocument));
                
		        pRoot.Menu.AddItem( "Close", "img/menu/close.png" ).SetAction(new GwenEventHandler(CloseDocument));
	        }
        }

		void CreateToolBar()
        {
           /* var pStrip = new Gwen.Control.TToolBarStrip(this);
	        pStrip.Dock( Pos.Top );

	        pStrip.Add( "New Document", "img/menu/new.png" )->onPress.Add( this, &ThisClass::NewDocument );

	        pStrip.Add( "Open", "img/menu/open.png" )->onPress.Add( this, &ThisClass::OpenDocument );
	        pStrip.Add( "Save", "img/menu/save.png" )->onPress.Add( this, &ThisClass::SaveDocument );

	        // splitter

	        pStrip.Add( "Delete", "img/menu/delete.png" )->onPress.Add( this, &ThisClass::DeleteSelected );
	        pStrip.Add( "Send Back", "img/menu/back.png" )->onPress.Add( this, &ThisClass::SendBack );
	        pStrip.Add( "Bring Forward", "img/menu/forward.png" )->onPress.Add( this, &ThisClass::BringForward );*/
        }
		void CreateDocumentHolder()
        {
            m_DocumentHolder = new Gwen.Control.TabControl( this );
	        m_DocumentHolder.Dock = Pos.Fill;
	        m_DocumentHolder.Margin = new Margin( -1, 2, -1, -1 );
        }
        void CreateControlToolbox() 
        {
            m_ControlBox = new ControlToolbox( this );
	        m_ControlBox.Dock = Pos.Left;
        }

		void NewDocument(Gwen.Control.Base target)
        {
	       var pButton = m_DocumentHolder.AddPage("Untitled Design");
	        pButton.SetImage( "img/document_normal.png" );

            var doc = new Document(pButton.Page, "Document");

	        //doc.Initialize( pButton );

	        //pButton.Press;
        }

		void CloseDocument(Gwen.Control.Base target)
        {
	      /*  Document doc = CurrentDocument();
	        if ( !doc ) return;

	        doc.DelayedDelete();

	        var pButton = m_DocumentHolder.GetCurrentButton();
	        if ( !pButton ) return;

	        m_DocumentHolder.RemovePage( pButton );
	        pButton.DelayedDelete();*/
        }

		/*void OpenDocument(Gwen.Control.Base target);
		void DoOpenDocument( Event::Info info );
		void SaveDocument(Gwen.Control.Base target);
		void SaveAsDocument(Gwen.Control.Base target);

		void DeleteSelected();
		void SendBack();
		void BringForward();

	    Document CurrentDocument();*/

    }
}
