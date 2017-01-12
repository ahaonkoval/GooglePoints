using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using Core;
using ServiceDebuggerHelper;
using Core.Searchers;

namespace CoordinateFinder
{
    public partial class ServiceFinder : ServiceBase, IDebuggableService
    {
        int i = 0;
        public ServiceFinder()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var interval = Convert.ToInt32(ConfigurationManager.AppSettings["ServiceRunIntervalInSeconds"]);
            var yandex_interval = Convert.ToInt32(ConfigurationManager.AppSettings["YandexRunIntervalInSeconds"]);

            var timer = new System.Timers.Timer {
                Interval = interval };
            timer.Elapsed += OnTimer;
            timer.Start();

            //var yandex_timer = new System.Timers.Timer { Interval = yandex_interval };
            //yandex_timer.Elapsed += yandex_timer_Elapsed;
            //yandex_timer.Start();
        }

        void yandex_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //CoreYandexPoints YandexFinder = new CoreYandexPoints();
            //YandexFinder.GetYandexCoordinates();
        }

        protected override void OnStop()
        {

        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            CoreGooglePoints cgp = new CoreGooglePoints();
            cgp.GetGooleCoordinates();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        public void StopService()
        {
            OnStop();
        }

        public void Pause()
        {
            OnPause();
        }

        public void Continue()
        {
            OnContinue();
        }

    }
}
