namespace Snake
{
    public partial class Form1 : Form
    {
        private PictureBox fruit;
        private PictureBox[] snake = new PictureBox[100];
        private int rI, rJ;
        private int dirX, dirY;
        private int _width = 400;
        private int _height = 400;
        private int _indent = 20;
        private int score = 0;

        public Form1()
        {
            InitializeComponent();

            this.Width = _width;
            this.Height = _height;

            dirX = 1;
            dirY = 0;

            snake[0] = new PictureBox();
            snake[0].Location = new Point(_width / 2, _height / 2);
            snake[0].Size = new Size(_indent, _indent);
            snake[0].BackColor = Color.IndianRed;
            this.Controls.Add(snake[0]);

            fruit = new PictureBox();
            fruit.BackColor = Color.GreenYellow;
            fruit.Size = new Size(_indent, _indent);

            _mapGeneration();
            _generateFruit();

            timer.Tick += new EventHandler(_update);
            timer.Interval = 150;
            timer.Start();

            this.KeyDown += new KeyEventHandler(OKE);
        }

        private void _generateFruit()
        {
            Random random = new Random();
            rI = random.Next(0, _width);
            rJ = random.Next(0, _width);

            int tempI = rI % _indent;
            rI -= tempI;

            int tempJ = rJ % _indent;
            rJ -= tempJ;

            for (int i = 0; i < snake.Count(); i++)
            {
                if (snake[i] != null && snake[i].Location == new Point(rI, rJ))
                {
                    rI = random.Next(0, _width - _indent);
                    tempI = rI % _indent;
                    rI -= tempI;

                    rJ = random.Next(0, _height - _indent);
                    tempJ = rJ % _indent;
                    rJ -= tempJ;

                    i = 0;
                }
            }

            fruit.Location = new Point(rI, rJ);
            this.Controls.Add(fruit);
        }

        private void _checkBorders()
        {
            if (snake[0].Location.X < 0)
            {
                snake[0].Location = new Point(_width, snake[0].Location.Y);

                for (int i = score; i >= 1; i--)
                {
                    snake[i].Location = snake[i - 1].Location;
                }
            }
            else if (snake[0].Location.X > _width)
            {
                snake[0].Location = new Point(0, snake[0].Location.Y);

                for (int i = score; i >= 1; i--)
                {
                    snake[i].Location = snake[i - 1].Location;
                }
            }
            else if (snake[0].Location.Y < 0)
            {
                snake[0].Location = new Point(snake[0].Location.X, _height);

                for (int i = score; i >= 1; i--)
                {
                    snake[i].Location = snake[i - 1].Location;
                }
            }
            else if (snake[0].Location.Y > _height)
            {
                snake[0].Location = new Point(snake[0].Location.X, 0);

                for (int i = score; i >= 1; i--)
                {
                    snake[i].Location = snake[i - 1].Location;
                }
            }
        }

        private void _eatItself()
        {
            for (int i = 1; i < score; i++)
            {
                if (snake[0].Location == snake[i].Location)
                {
                    this.Close();
                }
            }
        }

        private void _eatFruit()
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                ++score;
                
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + _indent * dirX, snake[score - 1].Location.Y + _indent * dirY);
                snake[score].Size = new Size(_indent, _indent);
                snake[score].BackColor = Color.Indigo;

                this.Controls.Add(snake[score]);

                _generateFruit();
            }
        }

        private void _mapGeneration()
        {
            for (int i = 0; i < _width / _indent; i++)
            {
                PictureBox line_vert = new PictureBox();
                PictureBox line_hor = new PictureBox();

                line_vert.BackColor = Color.Black;
                line_hor.BackColor = Color.Black;

                line_vert.Location = new Point(0, _indent * i);
                line_hor.Location = new Point(_indent * i, 0);

                line_vert.Size = new Size(_width, 1);
                line_hor.Size = new Size(1, _height);

                this.Controls.Add(line_vert);
                this.Controls.Add(line_hor);
            }
        }

        private void _moveSnake()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }

            snake[0].Location = new Point(snake[0].Location.X + dirX * _indent, snake[0].Location.Y + dirY * _indent);

            _eatItself();
        }

        private void _update(object obj, EventArgs evntargs)
        {
            _checkBorders();
            _eatFruit();
            _moveSnake();
        }

        private void OKE(object sender, KeyEventArgs evnt)
        {
            int dirXX = dirX;
            int dirYY = dirY;

            switch(evnt.KeyCode.ToString())
            {
                case "Right":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Down":
                    dirX = 0;
                    dirY = 1;
                    break;
                case "Up":
                    dirX = 0;
                    dirY = -1;
                    break;
            }

            for (int i = score; i >= 1; i--)
            {
                if (snake[i].Location == new Point(snake[0].Location.X + dirX * _indent, snake[0].Location.Y + dirY * _indent))
                {
                    dirX = dirXX;
                    dirY = dirYY;
                    return;
                }
            }
        }
    }
}