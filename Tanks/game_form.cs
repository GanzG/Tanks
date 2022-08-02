using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
//using static System.Console;

namespace Tanks
{
    public partial class game_form : Form
    {
        int bulletSize; //размер пули
        int speedBullet; //скорость пули
        int bulletVector = 1;

        int bulX; //= -10; //позиция пули моя X
        int bulY; //= -10; //позиция пули моя Y
        int enemyBulX, enemyBulY, myAlive;
        bool recharge = false, ChangeAlive = false, ChangeSpawnMedKit = false, shoot = false;

        //Картинки:
        static Graphics g;
        static Image medKit = null;
        static Image Boom = null;
        static Image Heart = null;
        static Image Bullet = null;
        static Image[] myTankImage = new Image[4];
        static Image[] enemyTankImage = new Image[4];
        static Image[] RocketImage = new Image[4];


        //Границы для танка
        int[] acceptField = new int[4];

        //Пуля:
        //Thread BulletMovingThread;

        UdpClient enemy_net_udp = new UdpClient();

        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static Socket enemy_net = socket;
        static int iamX, iamY, myVector;
        static int enemyX, enemyY, enemyVector = 4, AliveEnemy;
        static int medX = -30, medY = -30, medkitspawned;
        static Random rnd = new Random();
        IPAddress enemy;

        //Танк:
        int tank_size; //размер танка
        int muzzleSize = 6; //размер дула
        int speedTank = 4; //скорость танк
        Thread ControlThread;

        private void join_bt_Click(object sender, EventArgs e)
        {
            //console_rtb.Text += "join_bt - start" + Environment.NewLine;
            //IPAddress enemy;
            join_bt.Enabled = false;
            create_bt.Enabled = false;
            if (IPAddress.TryParse(ipaddr_tb.Text, out enemy))
            {
                try
                {
                    socket.Connect(enemy, 779);
                    enemy_net = socket;
                    //console_rtb.Text += "join_bt - connected to " + enemy + Environment.NewLine;
                    game_pb.Focus();
                    ControlThread = new Thread(ControlPlayer);
                    ControlThread.Start();
                    Task.Factory.StartNew(send_data);
                    Task.Factory.StartNew(receive_data);
                    Task.Factory.StartNew(iamAlive);
                    Task.Factory.StartNew(G_Engine);
                }
                catch
                {
                    //console_rtb.Text += "join_bt - catch" + Environment.NewLine;
                    if (socket.IsBound == false) MessageBox.Show("Данный IP-адрес недоступен для подключения.");
                    else MessageBox.Show("Соединение уже установлено.");
                    join_bt.Enabled = true;
                    create_bt.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Некорректно введено значение IP-адреса оппонента.");
                join_bt.Enabled = false;
                create_bt.Enabled = false;
            }
        }

        public void receive_data()
        {
            try
            {
                if (socket.IsBound == true)
                {
                    //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - waiting receive" + Environment.NewLine; }));
                    byte[] data_receive = new byte[8];
                    enemy_net.Receive(data_receive);
                    //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - receive data from " + IPAddress.Parse(((IPEndPoint)enemy_net.RemoteEndPoint).Address.ToString()) + " - Data: " + Encoding.ASCII.GetString(data_receive) + Environment.NewLine; console_rtb.SelectionStart = console_rtb.TextLength; console_rtb.ScrollToCaret(); }));
                    long data_rec = Convert.ToInt64(Encoding.ASCII.GetString(data_receive));

                    //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - data from enemy -> " + data_rec + Environment.NewLine; }));

                    if (data_rec % 10 == 1)
                    {
                        data_rec /= 10;
                        enemyVector = Convert.ToInt32(data_rec % 10);
                        if (enemyVector < 3) enemyVector += 2;
                        else enemyVector -= 2;
                        data_rec /= 10;
                        enemyY = Convert.ToInt32(game_pb.Height - (data_rec % 1000));
                        data_rec /= 1000;
                        enemyX = Convert.ToInt32(game_pb.Width - (data_rec % 1000));

                        data_receive = null;
                        data_rec = 0;
                    }

                    if (data_rec % 10 == 2)
                    {
                        data_rec /= 10;
                        myAlive = Convert.ToInt32(data_rec % 10);
                        data_rec /= 10;
                        enemyBulY = Convert.ToInt32(game_pb.Height - (data_rec % 1000));
                        data_rec /= 1000;
                        enemyBulX = Convert.ToInt32(game_pb.Width - (data_rec % 1000));
                        //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - my serdeshko = " + myAlive + Environment.NewLine; console_rtb.SelectionStart = console_rtb.TextLength; console_rtb.ScrollToCaret();}));
                        data_receive = null;
                        data_rec = 0;
                    }

                    if (data_rec % 10 == 3)
                    {
                        data_rec /= 10;
                        medkitspawned = Convert.ToInt32(data_rec % 10);
                        data_rec /= 10;
                        medY = Convert.ToInt32(game_pb.Height - (data_rec % 1000));
                        data_rec /= 1000;
                        medX = Convert.ToInt32(game_pb.Width - (data_rec % 1000));
                        //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - medkit at (" + medX + ";" + medY + Environment.NewLine; }));
                        data_receive = null;
                        data_rec = 0;
                    }

                    if (data_rec % 10 == 4)
                    {
                        data_rec /= 10;
                        AliveEnemy = Convert.ToInt32(data_rec % 10);
                        data_rec /= 10;
                        myAlive = Convert.ToInt32(data_rec % 10);
                        data_receive = null;
                        data_rec = 0;
                    }

                    //Drawing();

                    //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - data from enemy, drawing completed" + Environment.NewLine; }));
                    receive_data();
                }
                else MessageBox.Show("Соединение оборвано.");
            }
            catch
            {

                //this.Invoke(new Action(() => { console_rtb.Text += "receive_data - catch, receive is not completed. New attempt." + Environment.NewLine; console_rtb.SelectionStart = console_rtb.TextLength; console_rtb.ScrollToCaret(); }));
                receive_data();

            }
        }

        public void send_data()
        {
            if (socket.IsBound == true)
            {
                string data_to_send;
                data_to_send = "1";
                data_to_send = Convert.ToString(myVector) + data_to_send;
                data_to_send = iamY + data_to_send;
                while (data_to_send.Length <= 4) data_to_send = "0" + data_to_send;
                data_to_send = iamX + data_to_send;
                while (data_to_send.Length <= 7) data_to_send = "0" + data_to_send;
                //this.Invoke(new Action(() => { console_rtb.Text += "send_data - data to send is created" + Environment.NewLine; }));
                byte[] data_send = Encoding.ASCII.GetBytes(Convert.ToString(data_to_send));
                enemy_net.Send(data_send);
                data_to_send = null;
                if (recharge == true || ChangeAlive == true)
                {
                    data_to_send = "2";
                    data_to_send = Convert.ToString(AliveEnemy) + data_to_send;
                    data_to_send = bulY + data_to_send;
                    while (data_to_send.Length <= 4) data_to_send = "0" + data_to_send;
                    data_to_send = bulX + data_to_send;
                    while (data_to_send.Length <= 7) data_to_send = "0" + data_to_send;
                    data_send = Encoding.ASCII.GetBytes(Convert.ToString(data_to_send));
                    enemy_net.Send(data_send);
                    //this.Invoke(new Action(() => { console_rtb.Text += "send_data - " + data_to_send + Environment.NewLine; console_rtb.SelectionStart = console_rtb.TextLength; console_rtb.ScrollToCaret(); }));
                    data_to_send = null;
                }

                if (ChangeSpawnMedKit == true)
                {
                    ChangeSpawnMedKit = false;
                    data_to_send = "3";
                    data_to_send = Convert.ToString(medkitspawned) + data_to_send;
                    data_to_send = medY + data_to_send;
                    while (data_to_send.Length <= 4) data_to_send = "0" + data_to_send;
                    data_to_send = medX + data_to_send;
                    while (data_to_send.Length <= 7) data_to_send = "0" + data_to_send;
                    data_send = Encoding.ASCII.GetBytes(Convert.ToString(data_to_send));
                    enemy_net.Send(data_send);
                    data_to_send = null;
                    //this.Invoke(new Action(() => { console_rtb.Text += "send_data - medkit at (" + medX + ";" + medY + Environment.NewLine; }));
                }

                if (ChangeAlive == true)
                {
                    ChangeAlive = false;
                    data_to_send = "4";
                    data_to_send = Convert.ToString(myAlive) + data_to_send;
                    data_to_send = Convert.ToString(AliveEnemy) + data_to_send;
                    data_send = Encoding.ASCII.GetBytes(Convert.ToString(data_to_send));
                    enemy_net.Send(data_send);
                    data_to_send = null;
                }
                //this.Invoke(new Action(() => { console_rtb.Text += "send_data - data sending is completed" + Environment.NewLine; }));
            }
            else MessageBox.Show("Соединение оборвано.");
        }


        public void ConnectNListen()
        {
            //this.Invoke(new Action(() => { console_rtb.Text += "ConnectNListen - start" + Environment.NewLine; }));
            if (socket.IsBound == false)
            {
                //this.Invoke(new Action(() => { console_rtb.Text += "ConnectNListen - enemy is not connected yet" + Environment.NewLine;}));
                socket.Bind(new IPEndPoint(IPAddress.Any, 779));
                //this.Invoke(new Action(() => { console_rtb.Text += "ConnectNListen - socket bind" + Environment.NewLine; }));
                socket.Listen(10);
                //this.Invoke(new Action(() => { console_rtb.Text += "ConnectNListen - socket listen" + Environment.NewLine; }));
                enemy_net = socket.Accept();
                send_data();
                //this.Invoke(new Action(() => { console_rtb.Text += "ConnectNListen - socket accept" + Environment.NewLine; }));
                ////
                this.Invoke(new Action(() => { game_pb.Focus(); }));
                ControlThread = new Thread(ControlPlayer);
                ControlThread.Start();
                Task.Factory.StartNew(iamAlive);
                Task.Factory.StartNew(G_Engine);
                Task.Factory.StartNew(genmedkit);

                //Task.Factory.StartNew(detectmedkit);

                //this.Invoke(new Action(() => { console_rtb.Text += "join_bt - ControlPlayer was started" + Environment.NewLine; }));
                ////
            }
            else
            {

                socket = null;
                ConnectNListen();
            }

            Task.Factory.StartNew(receive_data);
        }

        public void G_Engine()
        {
            Image myTank = myTankImage[myVector - 1];
            Image enemyTank = enemyTankImage[enemyVector - 1];
            Image MyBullet;
            Image EnemyBullet = Bullet;
            if (recharge == true) MyBullet = Bullet;
            else MyBullet = RocketImage[myVector - 1];

            if (shoot == true)
            {
                switch (recharge)
                {
                    case true:
                        if (bulletVector == 1) bulX -= speedBullet;
                        if (bulletVector == 2) bulY -= speedBullet;
                        if (bulletVector == 3) bulX += speedBullet;
                        if (bulletVector == 4) bulY += speedBullet;
                        break;
                    case false:
                        if (myVector == 1) bulX -= speedBullet;
                        if (myVector == 2) bulY -= speedBullet;
                        if (myVector == 3) bulX += speedBullet;
                        if (myVector == 4) bulY += speedBullet;
                        break;
                }
            }

            this.Invoke(new Action(() => {
                game_pb.Refresh();

                if (medkitspawned == 1 && medX > 1 && medY > 1) g.DrawImage(medKit, medX - tank_size / 2, medY - tank_size / 2, tank_size, tank_size);

                g.DrawImage(myTank, iamX - (tank_size / 2), iamY - (tank_size / 2), tank_size, tank_size);
                g.DrawImage(enemyTank, enemyX - (tank_size / 2), enemyY - (tank_size / 2), tank_size, tank_size);
            }));

            
                if (shoot == true && bulX <= game_pb.Width && bulX >= 1 && bulY >= 1 && bulY <= game_pb.Height) this.Invoke(new Action(() => {
                    g.DrawImage(MyBullet, bulX - bulletSize / 2, bulY - bulletSize / 2, bulletSize, bulletSize);
            }));
                else
                {
                    recharge = false;
                    shoot = false;
                }
            
                if (enemyBulX != 0 && enemyBulY != 0) this.Invoke(new Action(() => {
                    g.DrawImage(EnemyBullet, enemyBulX - bulletSize / 2, enemyBulY - bulletSize / 2, bulletSize, bulletSize);
                }));
            if (myAlive > 0 && AliveEnemy > 0)
                {
                this.Invoke(new Action(() => {
                    g.DrawImage(Heart, tank_size / 2, tank_size / 2, tank_size / 2, tank_size / 2);
                    g.DrawString("x" + myAlive, new Font("Times New Roman", 14), new SolidBrush(Color.Black), tank_size, tank_size / 2);
                    g.DrawImage(Heart, Convert.ToInt32(game_pb.Width - 1.5 * tank_size), tank_size / 2, tank_size / 2, tank_size / 2);
                    g.DrawString("x" + AliveEnemy, new Font("Times New Roman", 14), new SolidBrush(Color.Black), game_pb.Width - tank_size, tank_size / 2);
                }));
            }
            



            
            
            if (Math.Abs(enemyX - bulX) <= ((tank_size / 2) + bulletSize / 2) && Math.Abs(enemyY - bulY) <= ((tank_size / 2) + bulletSize / 2)) 
            {
                this.Invoke(new Action(() =>
                {
                    g.DrawImage(Boom, bulX, bulY, Convert.ToInt32(0.75 * tank_size), Convert.ToInt32(0.75 * tank_size)); //не забыть сделать корректировку взрыва

                }));
                AliveEnemy--;
                bulX = 0;
                bulY = 0;
                shoot = false;
                Thread.Sleep(50);
                recharge = false;
            }
            send_data();
            G_Engine();
        }

        private void create_bt_Click(object sender, EventArgs e)
        {

            //console_rtb.Text += "create_bt - click" + Environment.NewLine;
            join_bt.Enabled = false;
            create_bt.Enabled = false;
            Task.Factory.StartNew(ConnectNListen);
            //this.Invoke(new Action(() => { console_rtb.Text += "Server is created." + Environment.NewLine; }));
        }

        //public void Drawing()
        //{
        //    myTank = myTankImage[myVector - 1];
        //    enemyTank = enemyTankImage[enemyVector - 1];

        //    if (recharge == true) MyBullet = Bullet;
        //    else MyBullet = RocketImage[myVector-1];
            
        //    EnemyBullet = Bullet;

        //    this.Invoke(new Action(() => { 
        //        //console_rtb.Text += "Drawing - start. I am at (" + iamX + ";" + iamY + "), enemy at (" + enemyX + ";" + enemyY + ")" + Environment.NewLine; console_rtb.SelectionStart = console_rtb.TextLength; console_rtb.ScrollToCaret();
        //        game_pb.Refresh();

        //        if (medkitspawned == 1 && medX > 1 && medY > 1) g.DrawImage(medKit, medX - tank_size/2, medY - tank_size/2, tank_size, tank_size);

        //        g.DrawImage(myTank, iamX - (tank_size / 2), iamY - (tank_size / 2), tank_size, tank_size);
        //        g.DrawImage(enemyTank, enemyX - (tank_size / 2), enemyY - (tank_size / 2), tank_size, tank_size);


        //        if (bulX <= game_pb.Width && bulX >= 1 && bulY >= 1 && bulY <= game_pb.Height) g.DrawImage(MyBullet, bulX - bulletSize / 2, bulY - bulletSize / 2, bulletSize, bulletSize);
        //        else recharge = false;

        //        if (enemyBulX != 0 && enemyBulY != 0) g.DrawImage(EnemyBullet, enemyBulX - bulletSize / 2, enemyBulY - bulletSize / 2, bulletSize, bulletSize);


        //        if (myAlive > 0 && AliveEnemy > 0)
        //        {
        //            g.DrawImage(Heart, tank_size / 2, tank_size / 2, tank_size / 2, tank_size / 2);
        //            g.DrawString("x" + myAlive, new Font("Times New Roman", 14), new SolidBrush(Color.Black), tank_size, tank_size / 2);
        //            g.DrawImage(Heart, Convert.ToInt32(game_pb.Width - 1.5 * tank_size), tank_size / 2, tank_size / 2, tank_size / 2);
        //            g.DrawString("x" + AliveEnemy, new Font("Times New Roman", 14), new SolidBrush(Color.Black), game_pb.Width - tank_size, tank_size / 2);
        //        }


        //    }));
        //    //Task.Factory.StartNew(send_data);
        //}

        private void game_form_Load(object sender, EventArgs e)
        {
            g = game_pb.CreateGraphics();

            tank_size = Convert.ToInt32((((game_pb.Height / 20) + (game_pb.Width / 20)) / 2) * 1.25);
            bulletSize = Convert.ToInt32(tank_size / 2.5);
            speedBullet = bulletSize / 2;
            //console_rtb.Text += "form_Load - iam spawn" + Environment.NewLine;
            
            iamX = game_pb.Width / 2;
            iamY = game_pb.Height - tank_size;
            myVector = 2;

            myAlive = 3;
            AliveEnemy = 3;

            if (map_cb.Text == "Winter map") game_pb.BackgroundImage = Image.FromFile("resources\\BG_winter.png");
            //game_bmp = new Bitmap(game_pb.Image);

            acceptField[0] = (int)muzzleSize + tank_size / 2;
            acceptField[1] = (int)muzzleSize + tank_size / 2;
            acceptField[2] = (int)game_pb.Width - tank_size / 2 - muzzleSize * 2;
            acceptField[3] = (int)game_pb.Height - tank_size / 2 - muzzleSize * 2;

            medKit = Image.FromFile("resources\\medkit.png");
            medkitspawned = 0;

            Boom = Image.FromFile("resources\\boom.png");
            Heart = Image.FromFile("resources\\heart.png");
            Bullet = Image.FromFile("resources\\bullet.png");

            myTankImage[0] = Image.FromFile("resources\\green left normal.png");
            myTankImage[1] = Image.FromFile("resources\\green up normal.png");
            myTankImage[2] = Image.FromFile("resources\\green right normal.png");
            myTankImage[3] = Image.FromFile("resources\\green down normal.png");

            enemyTankImage[0] = Image.FromFile("resources\\grey left normal.png");
            enemyTankImage[1] = Image.FromFile("resources\\grey up normal.png");
            enemyTankImage[2] = Image.FromFile("resources\\grey right normal.png"); 
            enemyTankImage[3] = Image.FromFile("resources\\grey down normal.png"); 

            RocketImage[0] = Image.FromFile("resources\\rocket_left.png");
            RocketImage[1] = Image.FromFile("resources\\rocket_up.png");
            RocketImage[2] = Image.FromFile("resources\\rocket_right.png");
            RocketImage[3] = Image.FromFile("resources\\rocket_down.png");

            //this.Width =  System.Windows.SystemParameters.PrimaryScreenWidth;
        }

        public void iamAlive()
        {
            while (myAlive > 0 && AliveEnemy > 0)
            {

            }
            if (MessageBox.Show("Хотите попробовать снова?", "Конец", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Invoke(new Action(() =>
                {
                    ControlThread?.Abort();
                    game_pb.KeyDown -= Program_KeyDown;
                    enemy_net.Close();
                    socket.Close();
                }));
                Application.Restart();
                //ConnectNListen();
            }
            else System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        

        private void Program_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {

                case Keys.W:
                    myVector = 2;
                    if (iamY <= acceptField[1])
                        iamY = acceptField[1];
                    else if (!((Math.Abs(enemyX - iamX) <= (tank_size)) && (Math.Abs(enemyY - iamY - speedTank + tank_size/2) <= (tank_size)))) iamY -= speedTank;
                    
                        break;
                case Keys.D:
                    myVector = 3;
                    if (iamX >= acceptField[2])
                        iamX = acceptField[2];
                    else if (!(Math.Abs(enemyX - iamX + speedTank - tank_size/2) <= (tank_size) && Math.Abs(enemyY - iamY) <= (tank_size))) iamX += speedTank;
                    break;
                case Keys.S:
                    myVector = 4;
                    if (iamY >= acceptField[3])
                        iamY = acceptField[3];
                    else if (!(Math.Abs(enemyX - iamX) <= (tank_size) && Math.Abs(enemyY - iamY + speedTank - tank_size/2) <= (tank_size))) iamY += speedTank;
                    break;
                case Keys.A:
                    myVector = 1;
                    if (iamX <= acceptField[0])
                        iamX = acceptField[0];
                    else if (!(Math.Abs(enemyX - iamX - speedTank + tank_size/2) <= (tank_size) && Math.Abs(enemyY - iamY) <= (tank_size))) iamX -= speedTank;
                    break;
                case Keys.Space:
                    if (recharge == false)
                    {
                        recharge = true;
                        shoot = true;
                        switch (myVector)
                        {
                            case 1:
                                bulX = iamX - tank_size / 2 - muzzleSize - bulletSize;
                                bulY = iamY - bulletSize / 2;
                                break;
                            case 2:
                                bulX = iamX - bulletSize / 2;
                                bulY = iamY - tank_size / 2 - muzzleSize - bulletSize;
                                break;
                            case 3:
                                bulX = iamX + tank_size / 2 + muzzleSize;
                                bulY = iamY - bulletSize / 2;
                                break;
                            case 4:
                                bulX = iamX - bulletSize / 2;
                                bulY = iamY + tank_size / 2 + muzzleSize;
                                break;
                        }
                        bulletVector = myVector;
                        //Task.Factory.StartNew(Shoot); 
                    }
                    break;
                case Keys.E:
                    shoot = true;
                    switch (myVector)
                    {
                        case 1:
                            bulX = iamX - tank_size / 2 - muzzleSize - bulletSize;
                            bulY = iamY - bulletSize / 2;
                            break;
                        case 2:
                            bulX = iamX - bulletSize / 2;
                            bulY = iamY - tank_size / 2 - muzzleSize - bulletSize;
                            break;
                        case 3:
                            bulX = iamX + tank_size / 2 + muzzleSize;
                            bulY = iamY - bulletSize / 2;
                            break;
                        case 4:
                            bulX = iamX - bulletSize / 2;
                            bulY = iamY + tank_size / 2 + muzzleSize;
                            break;
                    }
                    //Task.Factory.StartNew(Shoot);
                    break;
                case Keys.Escape:
                    ControlThread.Abort();
                    game_pb.KeyDown -= Program_KeyDown;
                    //this.Invoke(new Action(() => { console_rtb.Text += "Drawing. ControlThread was aborted." + Environment.NewLine; console_rtb.SelectionStart = console_rtb.TextLength; console_rtb.ScrollToCaret(); }));
                    break;

            }
            //string data_to_send = iamX + iamY + myVector;
            //byte[] data_send = Encoding.ASCII.GetBytes(data_to_send);
            Task.Factory.StartNew(send_data);
            //Drawing();
        }

        private void ControlPlayer()
        {
            game_pb.KeyDown += Program_KeyDown;
            ControlThread.Join();
        }


        //Выстрел
        //private void Shoot()
        //{
        //    while ((bulX <= game_pb.Width && bulX >=0) && (bulY >= 0 && bulY <= game_pb.Height))
        //    {
        //        if (Math.Abs(enemyX - bulX) <= ((tank_size/2)+ bulletSize/2) && Math.Abs(enemyY - bulY) <= ((tank_size / 2) + bulletSize/2))
        //        {
        //            this.Invoke(new Action(() =>
        //            {
        //                g.DrawImage(Boom, bulX, bulY, Convert.ToInt32(0.75 * tank_size), Convert.ToInt32(0.75 * tank_size));
        //                //MessageBox.Show("");
        //                AliveEnemy--;
        //                bulX = 0;
        //                bulY = 0;
        //                send_data();
        //            }));
        //            Thread.Sleep(50);
        //            Drawing();
        //            recharge = false;
        //            break;
        //        }
        //        switch(recharge)
        //        {
        //            case true:
        //                if (bulletVector == 1) bulX -= speedBullet;
        //                if (bulletVector == 2) bulY -= speedBullet;
        //                if (bulletVector == 3) bulX += speedBullet;
        //                if (bulletVector == 4) bulY += speedBullet;
        //                break;
        //            case false:
        //                if (myVector == 1) bulX -= speedBullet;
        //                if (myVector == 2) bulY -= speedBullet;
        //                if (myVector == 3) bulX += speedBullet;
        //                if (myVector == 4) bulY += speedBullet;
        //                break;
        //        }
               

                
        //        send_data();
        //        Drawing();
        //    }
        //}

        //Генерация МедКита
        public void genmedkit()
        {
            if (medkitspawned == 0)
            {
                medkitspawned = 1;
                ChangeSpawnMedKit = true;
                int time = rnd.Next(20000, 30000);
                Thread.Sleep(time);
                medX = rnd.Next(tank_size, game_pb.Width - tank_size);
                medY = rnd.Next(tank_size, game_pb.Height - tank_size);
                //Drawing();
                send_data();
                detectmedkit();
            }
        }

        public bool detectZone(int coordX, int coordY, int targetX, int targetY)
        {
            //MessageBox.Show("DetectZone - start");
            if ((Math.Abs(targetX - coordX) <= (tank_size)) && (Math.Abs(targetY - coordY) <= (tank_size))) return true;
            else return false;
        }

        //Подбор МедКита
        public void detectmedkit()
        {
            while (ChangeSpawnMedKit == false)
            {
                
                if (detectZone(enemyX, enemyY, medX, medY) == true && AliveEnemy < 9)
                {
                    AliveEnemy++;
                    ChangeAlive = true;
                }
                else if (detectZone(iamX, iamY, medX, medY) == true && myAlive < 9)
                {
                    myAlive++;
                    ChangeAlive = true;
                }

                if (ChangeAlive == true)
                {
                    medX = 0;
                    medY = 0;
                    medkitspawned = 0;
                    ChangeSpawnMedKit = true;
                    //Drawing(); //поменял эту строку и следующую местами
                    send_data();
                    genmedkit();
                }
            }
        }


        public game_form()
        {
            InitializeComponent();
            //Task.Factory.StartNew(Console);
        }

        private void close_bt_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
