using Gwen;
using Gwen.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer
{
    class Document : Gwen.Control.Base 
    {
        Gwen.Control.TabButton	m_pTab;
		DocumentCanvas		    m_pCanvas;
		Hierarchy				m_pHierarchy;
        ReflectionProperties    m_pPropreties;
		string			        m_strFilename;

		//ImportExport.Base*		m_Exporter;
		//ImportExport::Base*		m_Importer;

        public Document(Gwen.Control.Base parent, string name) 
            : base(parent)
        {
            Dock = Pos.Fill;
	        Padding = new Padding( 1, 1, 1, 1 );

	        // The main horizontal splitter separates the document from the tree/properties
            var pSplitter = new Gwen.Control.VerticalSplitter(this);
	        pSplitter.Dock = Pos.Fill;
	        //pSplitter.SetScaling( true, 200 );

            // The white background
            DocumentInner pInner = new DocumentInner(pSplitter);
            pInner.Dock = Pos.Fill;

	        // The vertical splitter on the right containing the tree/properties
            var pRightSplitter = new Gwen.Control.HorizontalSplitter(pSplitter);
	        pRightSplitter.Dock = Pos.Fill;
	        pRightSplitter.SetSize( 200, 200 );
	        //pRightSplitter.SetScaling( false, 200 );



	        pSplitter.SetPanel(0,pInner);
            pSplitter.SetPanel(1,pRightSplitter);

	
	        // The actual canvas onto which we drop controls
		    m_pCanvas = new DocumentCanvas( pInner );
		    m_pCanvas.Dock = Pos.Fill;
		    m_pCanvas.HierachyChanged += OnHierachyChanged;
            m_pCanvas.SelectionChanged += OnSelectionChanged;


	        // The controls on the right
		    m_pHierarchy = new Hierarchy( pRightSplitter );
		    m_pHierarchy.WatchCanvas( m_pCanvas );
		    m_pHierarchy.Dock = Pos.Fill;

            m_pPropreties = new ReflectionProperties(pRightSplitter); //new Properties( pRightSplitter );
            m_pPropreties.Setup(m_pCanvas);
            m_pPropreties.Dock = Pos.Fill;

            pRightSplitter.SetPanel(0, m_pHierarchy);
            pRightSplitter.SetPanel(1, m_pPropreties);
	        
        }

        /*virtual void Initialize( Gwen.Control.TabButton pTab );

        virtual void DoSaveAs( ImportExport::Base* exporter );
        virtual void DoSave( ImportExport::Base* exporter );
        virtual void LoadFromFile( const Gwen::String& str, ImportExport::Base* exporter );
		
        virtual void Command( const Gwen::String& str );


        void DoSaveFromDialog( Event::Info info );*/
        void OnHierachyChanged(Gwen.Control.Base  caller)
        {
            m_pHierarchy.CompleteRefresh();
        }

        void OnSelectionChanged(List<Gwen.Control.Base> selection)
        {
            if (selection.Count > 0)
                m_pPropreties.Setup(selection[0]);
        }

		


    }
}
