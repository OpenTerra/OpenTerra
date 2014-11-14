using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace OpenTerra
{
    internal class Game
    {
        private bool shouldRun;

        public static Game Instance { get; private set; }

        public double FramePeriod { get; private set; }

        public double FramesPerSecond { get; private set; }

        public double TickPeriod { get; private set; }

        public double TicksPerSecond { get; private set; }

        public GameWindow Window { get; private set; }

        public Game(Vector2 windowSize)
        {
            GraphicsMode mode = new GraphicsMode(new ColorFormat(32), 24, 0, 8);
            this.Window = new GameWindow((int)windowSize.X, (int)windowSize.Y, mode);
            if (Game.Instance != null)
            {
                Game.Instance.Stop();
                Game.Instance = null;
            }
            Game.Instance = this;
        }

        public void Run(int tickRate, int maxFrameRate)
        {
            this.shouldRun = true;

            this.Window.UpdateFrame += OnTick;
            this.Window.RenderFrame += OnRender;
            this.Window.Resize += OnResize;
            this.Window.Load += OnLoad;
            this.Window.KeyDown += Window_KeyDown;
            this.Window.VSync = VSyncMode.Off;
            this.Window.Visible = true;
            this.Window.Context.MakeCurrent(null);

            Thread tickThread = new Thread(new ParameterizedThreadStart(TickThread)) { IsBackground = true, Name = "Tick Thread" };
            Thread renderThread = new Thread(new ParameterizedThreadStart(RenderThread)) { IsBackground = true, Name = "Render Thread" };
            tickThread.Start(tickRate);
            renderThread.Start(maxFrameRate);

            while (tickThread.IsAlive || renderThread.IsAlive)
            {
                this.Window.Title = string.Format("OpenTerra (FPS: {0} TPS: {1})", this.FramesPerSecond.ToString("0"), this.TicksPerSecond.ToString("0"));
                this.Window.ProcessEvents();
                Thread.Sleep(1);
                if (this.Window.IsExiting)
                    this.shouldRun = false;
            }

            this.Window.Exit();
        }

        public void Stop()
        {
            this.shouldRun = false;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            OnResize(null, null);
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Window.Width, this.Window.Height, 0, 0.0, 10.0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Render here
        }

        private void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, this.Window.Width, this.Window.Height);
        }

        private void OnTick(object sender, FrameEventArgs e)
        {
            Thread.Sleep(20);
        }

        private void RenderThread(object maxFrameRateObj)
        {
            if (!(maxFrameRateObj is int))
                return;

            var maxFrameRate = (int)maxFrameRateObj;
            var maxFrameRatePlusOne = maxFrameRate + 1d;
            FramesPerSecond = maxFrameRate;

            var targetTimePerFrame = (int)(1000d / (double)maxFrameRate);
            FrameEventArgs e = new FrameEventArgs();
            Window.MakeCurrent();

            var timer = new Stopwatch();
            var correction = 0d;

            while (shouldRun)
            {
                timer.Restart();
                {
                    OnRender(this, e);
                    Window.SwapBuffers();
                }
                timer.Stop();

                var delay = targetTimePerFrame - (int)timer.ElapsedMilliseconds + (int)correction;
                if (delay > 0)
                {
                    timer.Start();
                    {
                        Thread.Sleep(delay);
                    }
                    timer.Stop();

                    correction = ((double)(correction * 100d - ((int)timer.ElapsedMilliseconds - delay)) / 101d);
                }

                FramePeriod = timer.ElapsedMilliseconds;
                FramesPerSecond = ((FramesPerSecond * maxFrameRate) + (1000d / timer.ElapsedMilliseconds)) / maxFrameRatePlusOne;
            }
        }

        private void TickThread(object tickRateObj)
        {
            if (!(tickRateObj is int))
                return;

            var tickRate = (int)tickRateObj;
            var tickRatePlusOne = tickRate + 1d;
            TicksPerSecond = tickRate;

            var targetTimePerUpdate = (int)(1000d / (double)tickRate);
            FrameEventArgs e = new FrameEventArgs();

            var timer = new Stopwatch();
            var correction = 0d;

            while (shouldRun)
            {
                timer.Restart();
                {
                    OnTick(this, e);
                }
                timer.Stop();

                var delay = targetTimePerUpdate - (int)timer.ElapsedMilliseconds + (int)correction;
                if (delay > 0)
                {
                    timer.Start();
                    {
                        Thread.Sleep(delay);
                    }
                    timer.Stop();

                    correction = ((double)(correction * 100d - ((int)timer.ElapsedMilliseconds - delay)) / 101d);
                }

                TickPeriod = timer.ElapsedMilliseconds;
                TicksPerSecond = ((TicksPerSecond * tickRate) + (1000d / timer.ElapsedMilliseconds)) / tickRatePlusOne;
            }
        }

        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Stop();
        }
    }
}