using System;
using System.Windows.Forms;
using System.Drawing;


class PIPE
{
	public int x, y, dx, width, height;

	public PIPE( int x, int y, int dx, int width, int height ) 
	{
		this.x = x;
		this.y = y;
		this.dx = dx;
		this.width  = width;
		this.height = height;
	}
}


class Program : Form
{
	static readonly int WIDTH  = 400;
	static readonly int HEIGHT = 600;

	static int m_x = 40, m_y = HEIGHT / 2, m_size = 25, m_dy = 0;

	static Random random = new Random();

	Timer m_timer = new Timer();

	PIPE[] m_pipe = new PIPE[ 3 ];

	int distance = 190;

	int m_score = 0;


	Program()
	{
		Text = "";

		ClientSize = new Size( WIDTH, HEIGHT );
		BackColor = Color.Black;
		DoubleBuffered = true;
		CenterToScreen();

		m_timer.Tick += new EventHandler( OnTimer );
		m_timer.Interval = 20;
		m_timer.Enabled = true;

		for ( int i = 0; i < m_pipe.Length; i++ ) {
			m_pipe[ i ] = new PIPE( 300 * ( i + 1 ), 0, 8, 40, random.Next( HEIGHT - 200 ) );
		}
	}

	void OnTimer( object sender, EventArgs e )
	{
		m_dy++;
		m_y += m_dy;
		m_score++;

		if ( m_y < 0 || m_y > HEIGHT ) m_timer.Enabled = false;

		for ( int i = 0; i < m_pipe.Length; i++ ) {
			m_pipe[ i ].x -= m_pipe[ i ].dx;

			if ( m_score % 200 == 0 ) m_pipe[ i ].dx++;
			
			if ( m_pipe[ i ].x < -WIDTH ) {
				m_pipe[ i ].x = WIDTH + 100;
				m_pipe[ i ].height = random.Next( HEIGHT - 200 );
			}

			if ( new Rectangle( m_pipe[ i ].x, m_pipe[ i ].y, m_pipe[ i ].width, m_pipe[ i ].height ).IntersectsWith( 
				 new Rectangle( m_x, m_y, m_size, m_size ) ) ) {
				m_timer.Enabled = false;
			}

			if ( new Rectangle( m_pipe[ i ].x, m_pipe[ i ].y + distance + m_pipe[ i ].height, m_pipe[ i ].width, HEIGHT - distance - m_pipe[ i ].height ).IntersectsWith( 
				 new Rectangle( m_x, m_y, m_size, m_size ) ) ) {
				m_timer.Enabled = false;
			}
		}

		Invalidate();
	}

	protected override void OnKeyDown( KeyEventArgs e )
	{
		m_dy = -12;
	}

	protected override void OnPaint( PaintEventArgs e )
	{
		e.Graphics.FillRectangle( Brushes.Blue, m_x, m_y, m_size, m_size );

		for ( int i = 0; i < m_pipe.Length; i++ ) {
			e.Graphics.FillRectangle( Brushes.Pink, m_pipe[ i ].x, m_pipe[ i ].y, m_pipe[ i ].width, m_pipe[ i ].height );
		}

		for ( int i = 0; i < m_pipe.Length; i++ ) {
			e.Graphics.FillRectangle( Brushes.Pink, m_pipe[ i ].x, distance + m_pipe[ i ].height + m_pipe[ i ].y, m_pipe[ i ].width, HEIGHT - distance - m_pipe[ i ].height );
		}

		e.Graphics.DrawString( "SCORE " + m_score, new Font( "MS Gothic", 18 ), Brushes.White, 3, 3 );
	}

	static void Main()
	{
		Application.Run( new Program() );
	}
}