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

        bool TickStatusGoogle = false;
        bool TickStatusOsm = false;

        object lockerGoogle = new object();
        object lockerOSM = new object();

        HelperDB hdb;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ServiceFinder()
        {
            hdb = new HelperDB();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsGoogleStart"]))
            {
                var TimerGoogleSearcher = new System.Timers.Timer
                {
                    Interval = Convert.ToInt32(ConfigurationManager.AppSettings["GoogleServiceRunIntervalInSeconds"])
                };
                TimerGoogleSearcher.Elapsed += OnTimerTimerGoogleSearcher;
                TimerGoogleSearcher.Start();
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsOSMStart"]))
            {
                var TimerOSMSearcher = new System.Timers.Timer
                {
                    Interval = Convert.ToInt32(ConfigurationManager.AppSettings["OSMRunIntervalInSeconds"])
                };
                TimerOSMSearcher.Elapsed += TimerOSMSearcher_Elapsed;
                TimerOSMSearcher.Start();
            }
        }

        private void TimerOSMSearcher_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (TickStatusOsm)
            {
                _logger.Info("Пропущений тік...");
                return;
            }


            lock (lockerOSM)
            {
                TickStatusOsm = true;
                ProcessPoint point = hdb.GetEpicentrKPointForGeocoding();
                _logger.Info(string.Format("{0} {1} {2}", point.CardId.ToString(), "Address Get DB to OSM:", point.SourceAddress));

                if (point != null)
                {
                    using (OsmPointsSearcher osp = new OsmPointsSearcher())
                    {
                        point = osp.IdentifyCoordinatePoint(point);
                    }
                }
                TickStatusOsm = false;
            }
        }

        protected override void OnStop()
        {

        }

        /*
                    using (OsmPointsSearcher osp = new OsmPointsSearcher())
                    {
                        point = osp.IdentifyCoordinatePoint(point);
                    }
         */

        private void OnTimerTimerGoogleSearcher(object sender, System.Timers.ElapsedEventArgs e)
        {
            /*
             * 1. Беремо адресу покупця епицентра (PointMap)
             * 2. Перевіряємо Osm, 
             *      -> нема Перевіряємо Google
             *          -> нема або закінчилась кількіть спроб -> перевіряємо через Yandex
             */           

            if (TickStatusGoogle)
            {
                _logger.Info("Пропущений тік Google...");
                return;
            }
                

            lock (lockerGoogle)
            {
                this.TickStatusGoogle = true;


                if (hdb.GetRemainingAttemptsCount() <= 0)
                {
                    this.TickStatusGoogle = false;  // важно!
                    return;
                }

                ProcessPoint point = hdb.GetEpicentrKPointForGeocoding();

               if (point == null)
               {
                    return;
               }

                _logger.Info(string.Format("{0} {1} {2}", point.CardId.ToString(), "Address Get DB to Google:", point.SourceAddress));

                if (point != null)
                {
                    //if (point.Coordinate == null)
                    //{
                    using (GooglePointsSearcher cgp = new GooglePointsSearcher(hdb))
                    {
                        cgp.IdentifyCoordinatePoint(point);
                    }
                    //}                    
                }
                this.TickStatusGoogle = false;
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
