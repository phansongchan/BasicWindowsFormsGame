using System;
using System.Windows.Forms;
using System.Drawing;


class OBJECT
{
	public int x, y, dx, dy, size;

	public OBJECT( int x, int y, int dx, int dy, int size )
	{
		this.x = x;
		this.y = y;
		this.dx = dx;
		this.dy = dy;
		this.size = size; 
	}
}


class Program : Form
{
	static int WIDTH  = 700;
	static int HEIGHT = 500;

	Timer 	m_timer = new Timer();
	Random 	random  = new Random();


	static OBJECT m_player = new OBJECT( 50, HEIGHT / 2, 20, 20, 30 );
	static OBJECT m_bullet = new OBJECT( m_player.x, m_player.y, 25, 25, 15 );

	static OBJECT[] m_enemy = new OBJECT[ 8 ];

	string m_bulletState = "ready";

	int m_count = 0;
	int m_score = 0;


	Program()
	{
		Text = "";

		ClientSize = new Size( WIDTH, HEIGHT );
		DoubleBuffered = true;
		BackColor = Color.Black;
		CenterToScreen();

		m_timer.Tick += new EventHandler( OnMyTimer );
		m_timer.Interval = 20;
		m_timer.Enabled = true;

		for ( int i = 0; i < m_enemy.Length; i++ ) {
			m_enemy[ i ] = new OBJECT( WIDTH, random.Next( HEIGHT ), 8, 10, m_player.size );
		}
	}

	bool Touched( Rectangle r1, Rectangle r2 )
	{
		return r1.IntersectsWith( r2 );
	}

	void OnMyTimer( object sender, EventArgs e )
	{
		m_count++;
		m_bulletState = "fire";

		for ( int i = 0; i < m_enemy.Length; i++ ) {
			m_enemy[ i ].x -= m_enemy[ i ].dx;
			m_enemy[ i ].y -= random.Next( m_enemy[ i ].dy + 11 );
			m_enemy[ i ].y += random.Next( m_enemy[ i ].dy + 10 );
		}

		if ( m_bulletState == "fire" ) {
			m_bullet.x += m_bullet.dx;

			if ( m_bullet.x > WIDTH ) {
				m_bulletState = "ready";
			}
		}

		if ( m_bulletState == "ready" ) {
			m_bullet.x = m_player.x;
			m_bullet.y = m_player.y;
		}


		for ( int i = 0; i < m_enemy.Length; i++ ) {
			if ( Touched( new Rectangle( m_player.x, m_player.y, m_player.size, m_player.size ), new Rectangle( m_enemy[ i ].x, m_enemy[ i ].y, m_enemy[ i ].size, m_enemy[ i ].size ) ) ) {
				m_timer.Enabled = false;
			}

			if ( Touched( new Rectangle( m_bullet.x, m_bullet.y, m_bullet.size, m_bullet.size ), new Rectangle( m_enemy[ i ].x, m_enemy[ i ].y, m_enemy[ i ].size, m_enemy[ i ].size ) ) ) {
				m_enemy[ i ].y = random.Next( HEIGHT );
				m_enemy[ i ].x = WIDTH;
				m_bulletState = "ready";
				m_score += 10;
			}

			if ( m_enemy[ i ].x < 0 ) {
				m_enemy[ i ].y = random.Next( HEIGHT );
				m_enemy[ i ].x = WIDTH;
			}
		}
		
		Invalidate();
	}

	protected override void OnKeyDown( KeyEventArgs e )
	{
		if ( e.KeyCode == Keys.Left  ) m_player.x -= m_player.dx;
		if ( e.KeyCode == Keys.Right ) m_player.x += m_player.dx;
		if ( e.KeyCode == Keys.Down  ) m_player.y += m_player.dy;
		if ( e.KeyCode == Keys.Up    ) m_player.y -= m_player.dy;
		// if ( e.KeyCode == Keys.Space );

		// Invalidate();
	}

	protected override void OnPaint( PaintEventArgs e )
	{
		e.Graphics.FillRectangle( Brushes.Green, m_player.x, m_player.y, m_player.size, m_player.size );

		if ( m_bulletState == "fire" ) {
			e.Graphics.FillRectangle( Brushes.White, m_bullet.x, m_bullet.y, m_bullet.size, m_bullet.size );
		}

		for ( int i = 0; i < m_enemy.Length; i++ ) {
			e.Graphics.FillRectangle( Brushes.Red, m_enemy[ i ].x, m_enemy[ i ].y, m_enemy[ i ].size, m_enemy[ i ].size );
		}

		e.Graphics.DrawString( "TIME "  + m_count, new Font( "MS Gothic", 20 ), Brushes.White, 3, 3 );
		e.Graphics.DrawString( "SCORE " + m_score, new Font( "MS Gothic", 20 ), Brushes.White, 3, 40 );
	}

	static void Main()
	{
		Application.Run( new Program() );
	}
}