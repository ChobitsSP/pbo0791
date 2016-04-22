namespace PokemonBattle.RoomClient
{
    using NetworkLib;
    using System;
    using System.Threading;
    using System.Timers;

    internal class BroadcastController
    {
        private int _counter;
        private System.Timers.Timer _timer = new System.Timers.Timer(5000.0);

        public event VoidFunctionDelegate OnCounterChanged;

        public BroadcastController()
        {
            this._timer.Elapsed += new ElapsedEventHandler(this._timer_Elapsed);
            this._timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this._counter > 0)
            {
                this._counter--;
                if (this._counter < 10)
                {
                    this.HandleCounterChangedEvent();
                }
            }
        }

        private void HandleCounterChangedEvent()
        {
            if (this.OnCounterChanged != null)
            {
                this.OnCounterChanged();
            }
        }

        public void Stop()
        {
            this._timer.Stop();
        }

        public void Tick()
        {
            this._counter++;
            this.HandleCounterChangedEvent();
            if (this._counter == 10)
            {
                this._counter = 40;
            }
        }

        public int Counter
        {
            get
            {
                return this._counter;
            }
        }
    }
}

