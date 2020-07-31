using System.Threading;
using System.Threading.Tasks;

using BindingDemo.ViewModel;

namespace BindingDemo
{
    public static class Update
    {
        private static MainWindowViewModel ViewModel = ModelFactory.Get(typeof(MainWindowViewModel), new object[] { }) as MainWindowViewModel;

        public static void UpdateVal()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    ViewModel.IsRunningProcess = !ViewModel.IsRunningProcess;
                    Thread.Sleep(1000);
                }
            });
        }
    }
}