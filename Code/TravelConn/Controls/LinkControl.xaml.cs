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
    /// Interaction logic for LinkControl.xaml
    /// </summary>
    public partial class LinkControl : UserControl
    {
        public LinkControl()
        {
            InitializeComponent();
        }

        public Point point1
        {
            get
            {
                Point point = new Point();
                point.X = this.Line.X1;
                point.Y = this.Line.Y1;
                return point;
            }
            set
            {
                this.Line.X1 = point1.X;
                this.Line.Y1 = point1.Y;
            }
        }
        public Point point2
        {
            get
            {
                Point point = new Point();
                point.X = this.Line.X2;
                point.Y = this.Line.Y2;
                return point;
            }
            set
            {
                this.Line.X2 = point2.X;
                this.Line.Y2 = point2.Y;
            }
        }
    }
}
