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
    /// Interaction logic for BusStopControl.xaml
    /// </summary>
    public partial class BusStopControl : UserControl
    {
        public BusStopControl()
        {
            InitializeComponent();
        }

        public BusStopControl(CBusStop busStop, Point relativePos)
        {
            InitializeComponent();
            this.relativePos = relativePos;
            this.busStop = busStop;
        }

        private string type;
        public CBusStop busStop;
        public Point relativePos; // always between 0 and 1;

        public string CurrentType()
        {
            return type;
        }

        public void ChangeType(string type)
        {
            switch (type)
            {
                case "default":
                    break;
                case "start":
                    break;
                case "end":
                    break;
                case "injourney":
                    break;
                case "buschange":
                    break;
            }

            ellipseOnMap.Style = (Style)FindResource(type);
            this.type = type;
        }

        public void ChangeState(string state)
        {
            switch (state)
            {
                case "selected":
                    ellipseOnMap.Style = (Style)FindResource(type);
                    break;

                case "resting":
                    ellipseOnMap.Style = (Style)FindResource("default");
                    break;
            }
        }

    }
}
