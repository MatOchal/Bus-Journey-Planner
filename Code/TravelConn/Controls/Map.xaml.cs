using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TravelConn.Controls
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        List<CBusStop> busStops;
        List<CLink> links;

        public Map()
        {
            InitializeComponent();
        }

        public void Initialise(List<CBusStop> busstops, List<CLink> links)
        {
            this.busStops = busstops;
            this.links = links;
            IniBusStopsOnMap();
        }

        ///Initialises the bus stops on the map 
        private void IniBusStopsOnMap()
        {
            ///All bus stops must fit into the map
            ///According to the most extreme values of latitude and longitude the distances are scaled appropiatly

            ///Extreme values for lat and long
            ///Latitude is always from -90 to +90 degrees
            double minLat = 99;
            double maxLat = -99;
            ///Longitude is from -180 to +180 degrees
            double minLong = 199;
            double maxLong = -199;

            ///Gets the most extreme values for lat and long 
            foreach (CBusStop busStop in busStops){
                if (busStop.latitude > maxLat) maxLat = busStop.latitude;
                if (busStop.latitude < minLat) minLat = busStop.latitude;
                if (busStop.longitude > maxLong) maxLong = busStop.longitude;
                if (busStop.longitude < minLong) minLong = busStop.longitude;
            }

            ///Height and width of the map control are taken
            ///The extra amount taken from the size is to allow for the correction of the size of the bus stop control
            double mapHeight = canvas.Height - 50;
            double mapWidth = canvas.Width - 50;

            ///relativepos - a point with coordinates between 0 and 1 in relation to other bus stops
            Point relativepos = new Point();

            ///After obtaining the relative point, bus stops controls are positioned in the correct location on the screen 
            foreach (CBusStop busStop in busStops)
            {
                relativepos.X = (busStop.latitude - minLat) / (maxLat - minLat);
                relativepos.Y = (busStop.longitude - minLong) / (maxLong - minLat);

                Controls.BusStopControl busStopControl = new Controls.BusStopControl(busStop, relativepos);
                Canvas.SetLeft(busStopControl, mapWidth * relativepos.X );
                Canvas.SetTop(busStopControl, mapHeight * relativepos.Y );

                ///Add two events: Mouse Enter and Mouse Leave
                ///busStopControl.MouseEnter += BusStopOnMap_MouseEnter;
                ///busStopControl.MouseLeave += BusStopOnMap_MouseLeave;

                busStop.busStopControl = busStopControl;
            }

            ///Add and show links on the map
            IniLinksOnMap(mapHeight,mapWidth);

            //Add and show bus stops on map
            foreach(CBusStop busStop in busStops)
            {
                canvas.Children.Add(busStop.busStopControl);
            }
        }

        /// Initialise the link and add them to the map
        private void IniLinksOnMap(double mapHeight, double mapWidth)
        {
            Controls.LinkControl linkOnMap;
            foreach (CLink link in links){
                linkOnMap = new Controls.LinkControl();
                linkOnMap.Line.X1 = link.busStopA.busStopControl.relativePos.X * (mapWidth) + link.busStopA.busStopControl.ellipseOnMap.Width / 2;
                linkOnMap.Line.Y1 = link.busStopA.busStopControl.relativePos.Y * (mapHeight) + link.busStopA.busStopControl.ellipseOnMap.Height / 2;
                linkOnMap.Line.X2 = link.busStopB.busStopControl.relativePos.X * (mapWidth)  + link.busStopA.busStopControl.ellipseOnMap.Width / 2;
                linkOnMap.Line.Y2 = link.busStopB.busStopControl.relativePos.Y * (mapHeight) + link.busStopA.busStopControl.ellipseOnMap.Height / 2;
                canvas.Children.Add(linkOnMap);
            }
        }

    }
}
