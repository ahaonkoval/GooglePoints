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
using Core.Helpers;
using Core.Objects;
using NLog;

namespace GeoCoordinateSearcher
{
    public partial class ServiceFinder : ServiceBase, IDebuggableService
    {
        int i = 0;

        HelperDB hdb;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ServiceFinder()
        {
            hdb = new HelperDB();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsEpicentrKStart"]))
            {
                var TimerEpicentrK = new System.Timers.Timer
                {
                    Interval = Convert.ToInt32(ConfigurationManager.AppSettings["ServiceRunIntervalInSeconds"])
                };
                TimerEpicentrK.Elapsed += OnTimerEpicentrK;
                TimerEpicentrK.Start();
            }

        }

        protected override void OnStop()
        {

        }

        private void OnTimerEpicentrK(object sender, System.Timers.ElapsedEventArgs e)
        {
            /*
             * 1. Беремо адресу покупця епицентра (PointMap)
             * 2. Перевіряємо Osm, 
             *      -> нема Перевіряємо Google
             *          -> нема або закінчилась кількіть спроб -> перевіряємо через Yandex
             */
            
            ProcessPoint point = hdb.GetEpicentrKPointForGeocoding();
            _logger.Info(string.Format("{0} {1} {2}", point.CardId.ToString(), "Address Get DB:", point.SourceAddress));

            if (point != null)
            {
                using (OsmPointsSearcher osp = new OsmPointsSearcher())
                {
                    point = osp.IdentifyCoordinatePoint(point);
                }

                if (point.Coordinate == null)
                {
                    using (GooglePointsSearcher cgp = new GooglePointsSearcher(hdb))
                    {
                        cgp.IdentifyCoordinatePoint(point);
                    }
                }
            }
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
