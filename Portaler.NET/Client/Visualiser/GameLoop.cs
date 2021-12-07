using System;
using System.Threading.Tasks;

namespace Portaler.NET.Client.Visualiser
{
    public class GameLoop
    {
        private readonly Action<double> _update;
        private readonly Func<Task> _draw;
        private readonly double _targetFrameRate;

        private bool _isRunning;

        private double _deltaTime = 0;
        private double _accumulator = 0;
        private DateTime _lastUpdateTime = DateTime.UtcNow;

        public GameLoop(Action<double> update, Func<Task> draw, int targetFrameRate)
        {
            _update = update;
            _draw = draw;
            _targetFrameRate = 1.0 / targetFrameRate;
        }


        public async Task StartLoop()
        {
            if (_isRunning)
                return;
            _isRunning = true;

            await Start();
        }

        public void StopLoop()
        {
            _isRunning = false;
        }

        private async Task Start()
        {
            _lastUpdateTime = DateTime.UtcNow;
            while(_isRunning)
            {
                _deltaTime = (DateTime.UtcNow - _lastUpdateTime).TotalMilliseconds;
                _lastUpdateTime += TimeSpan.FromMilliseconds(_deltaTime);
                _accumulator += _deltaTime;

                while(_isRunning && _accumulator > _targetFrameRate)
                {
                    _update(_targetFrameRate);
                    _accumulator -= _targetFrameRate;
                    await Task.Yield();
                }

                await _draw();

                await Task.Yield();
            }
        }
    }
}
