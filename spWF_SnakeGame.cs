using System;
using System.Drawing;
using System.Windows.Forms;


class Program : Form
{
	static readonly int M_TILE = 25;
	static readonly int M_WIDTH  = 30 * M_TILE;
	static readonly int M_HEIGHT = 25 * M_TILE;


	static Random rand = new Random();

	Timer m_timer = new Timer();
	
	static int m_count = 0;

	static int[] m_x = new int[ 100 ];
	static int[] m_y = new int[ 100 ];
	static int m_length = 4;
	static char m_dir = 'r';

	static int m_fruitX, m_fruitY;

	Program()
	{
		Text = "";
		
		ClientSize = new Size( M_WIDTH, M_HEIGHT );
		BackColor  = Color.FromArgb( 0, 0, 0 );
		DoubleBuffered = true;
		CenterToScreen();
		
		m_timer.Tick += new EventHandler( OnMyTimer );
		m_timer.Interval = 70;
		m_timer.Enabled = true;
		
		MakeFruit();
	}
	
	void MakeFruit()
	{
		m_fruitX = rand.Next( M_WIDTH  / M_TILE ) * M_TILE;
		m_fruitY = rand.Next( M_HEIGHT / M_TILE ) * M_TILE;
	}
	
	void OnMyTimer( object sender, EventArgs e )
	{
		m_count++;
		
		for ( int i = m_length; i > 0; i-- ) {
			m_x[ i ] = m_x[ i - 1 ];
			m_y[ i ] = m_y[ i - 1 ];
		}
		
		if ( m_dir == 'r' ) {
			m_x[ 0 ] += M_TILE;
		} else if ( m_dir == 'l' ) {
			m_x[ 0 ] -= M_TILE;
		} else if ( m_dir == 'd' ) {
			m_y[ 0 ] += M_TILE;
		} else if ( m_dir == 'u' ) {
			m_y[ 0 ] -= M_TILE;
		}
		
		if ( m_x[ 0 ] < 0 ) 		m_x[ 0 ] = M_WIDTH;
		if ( m_y[ 0 ] < 0 ) 		m_y[ 0 ] = M_HEIGHT;
		if ( m_x[ 0 ] > M_WIDTH  )  m_x[ 0 ] = 0;
		if ( m_y[ 0 ] > M_HEIGHT )  m_y[ 0 ] = 0;
		
		
		if ( m_x[ 0 ] == m_fruitX && m_y[ 0 ] == m_fruitY ) {
			MakeFruit();
			m_length++;
		}
		
		for ( int i = m_length; i > 0; i-- ) {
			if ( m_x[ 0 ] == m_x[ i ] && m_y[ 0 ] == m_y[ i ] ) {
				m_timer.Stop();
			}
		}
		
		Invalidate();
	}
	
	protected override void OnKeyDown( KeyEventArgs e )
	{		
		if ( e.KeyCode == Keys.Left  ) if ( m_dir != 'r' ) m_dir = 'l';
		if ( e.KeyCode == Keys.Right ) if ( m_dir != 'l' ) m_dir = 'r';
		if ( e.KeyCode == Keys.Down  ) if ( m_dir != 'u' ) m_dir = 'd';
		if ( e.KeyCode == Keys.Up    ) if ( m_dir != 'd' ) m_dir = 'u';
 
		Invalidate();
	}
	
	protected override void OnPaint( PaintEventArgs e )
	{
		e.Graphics.DrawString( "TIME " + m_count, new Font( "MS Gothic", 18 ), new SolidBrush( Color.White ), 3, 3 );

		e.Graphics.FillRectangle( new SolidBrush( Color.Pink ), m_fruitX, m_fruitY, M_TILE, M_TILE );
		
		for ( int i = 0; i < m_length; i++ ) {
			e.Graphics.FillRectangle( new SolidBrush( Color.Blue ), m_x[ i ], m_y[ i ], M_TILE, M_TILE );
		}
	}
	
	static void Main()
	{
		Application.Run( new Program() );
	}
}