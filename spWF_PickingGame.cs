using System;
using System.Windows.Forms;
using System.Drawing;


class OBJECT
{
	public int x, y, dx, dy, width, height;

	public OBJECT( int x, int y, int dx, int dy, int width, int height )
	{
		this.x = x;
		this.y = y;
		this.dx = dx;
		this.dy = dy;
		this.width  = width;
		this.height = height;
	}
}


class Program : Form
{
	static readonly int WIDTH  = 600;
	static readonly int HEIGHT = 500;

	int m_score = 0;

	bool m_gameOver = false;

	static Random random = new Random();

	static int m_amount = 5;

	OBJECT 		 m_player = new OBJECT( WIDTH / 2, HEIGHT - 40, 18, 18, 100, 20 );
	OBJECT[] 	 m_enemy  = new OBJECT[ m_amount ];
	OBJECT		 m_evilEnemy = new OBJECT( random.Next( WIDTH ), random.Next( 20 ), 1, 1, 20, 20 );


	Program()
	{
		Text = "";

		ClientSize = new Size( WIDTH, HEIGHT );
		DoubleBuffered = true;
		BackColor = Color.FromArgb( 40, 120, 255 );
		CenterToScreen();

		for ( int i = 0; i < m_enemy.Length; i++ ) {
			m_enemy[ i ] = new OBJECT( random.Next( WIDTH ), random.Next( 20 ), 1, 1, 20, 20 );
		}
	}

	protected override void OnKeyDown( KeyEventArgs e )
	{
		if ( e.KeyCode == Keys.Left  ) m_player.x -= m_player.dx;
		if ( e.KeyCode == Keys.Right ) m_player.x += m_player.dx;

		if ( !m_gameOver ) Invalidate();
	}

	protected override void OnPaint( PaintEventArgs e )
	{
		e.Graphics.FillRectangle( Brushes.Red, m_player.x, m_player.y, m_player.width, m_player.height );

		for ( int i = 0; i < m_enemy.Length; i++ ) {
			e.Graphics.FillRectangle( Brushes.Pink, m_enemy[ i ].x, m_enemy[ i ].y, m_enemy[ i ].width, m_enemy[ i ].height );
			m_enemy[ i ].y += m_enemy[ i ].dy;
		
			if ( m_enemy[ i ].x > m_player.x && m_enemy[ i ].x < m_player.x + m_player.width ) {
				if ( m_enemy[ i ].y > m_player.y && m_enemy[ i ].y < m_player.y + m_player.height ) {
					m_enemy[ i ].y = random.Next( 20 );
					m_enemy[ i ].x = random.Next( WIDTH );
					m_score += 10;
				}
			}

			if ( m_enemy[ i ].y > HEIGHT ) {
				m_enemy[ i ].y = random.Next( 20 );
				m_enemy[ i ].x = random.Next( WIDTH );
			}
		}

		e.Graphics.FillRectangle( Brushes.DimGray, m_evilEnemy.x, m_evilEnemy.y, m_evilEnemy.width, m_evilEnemy.height );

		e.Graphics.DrawString( "SCORE " + m_score, new Font( "MS Gothic", 17 ), Brushes.Orange, 3, 3 );
		m_evilEnemy.y += m_evilEnemy.dy;

		if ( m_evilEnemy.y > HEIGHT ) {
			m_evilEnemy.y = random.Next( 20 );
			m_evilEnemy.x = random.Next( WIDTH );
		}

		if ( m_evilEnemy.x > m_player.x && m_evilEnemy.x < m_player.x + m_player.width ) {
			if ( m_evilEnemy.y > m_player.y && m_evilEnemy.y < m_player.y + m_player.height ) {
				m_evilEnemy.y = random.Next( 20 );
				m_evilEnemy.x = random.Next( WIDTH );
				m_gameOver = true;
			}
		}

		if ( !m_gameOver ) Invalidate();
	}

	static void Main()
	{
		Application.Run( new Program() );
	}
}