using System;
using System.Threading;
using DirectXOverlay.Native;

namespace DirectXOverlay.Utilities
{
    public static class ThreadHelper
    {
        public static int ThreadDpiContext = 3;

        public static Thread RunGuarded(Action threadHandler, Action<Exception> exceptionHandler, bool isBackground = false, ThreadPriority priority = ThreadPriority.Normal)
        {
            var thread = new Thread(() =>
            {
                User32.SetThreadDpiAwarenessContext(ref ThreadDpiContext);

                try
                {
                    threadHandler();
                }
                catch (Exception ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
            })
            {
                IsBackground = isBackground,
                Priority = priority
            };

            thread.Start();

            return thread;
        }

        public static void SafeJoin(params Thread[] threads)
        {
            ValidateHelper.ValidateNull(threads, nameof(threads));

            foreach (var thread in threads)
            {
                if (thread != null && thread.IsAlive)
                {
                    thread.Join();
                }
            }
        }
    }
}