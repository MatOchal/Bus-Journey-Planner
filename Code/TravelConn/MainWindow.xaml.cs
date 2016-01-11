using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using TravelConn.Controls;

namespace TravelConn
{
    public partial class MainWindow : Window
    {
        /// global private variables
        
        /// splashWindow - used to inform the user that the program is doing something
        /// when there is a lot of data loading and creating objects can use up a lot of time
        SplashWindow splashWindow = new SplashWindow();

        /// busNetwork - this object includes all the core objects used for the algorithm
        CBusNetwork busNetwork;

        /// Initialising the Main Window
        public MainWindow()
        {
            splashWindow.Show();
            
            /// Initialises all the main window XAML components
            InitializeComponent();
            
            /// Creates the bus network object
            busNetwork = new CBusNetwork("file", "TestTimetable/");
            busNetwork.LoadCoreData();
            busNetwork.LoadTimetable();
            /// An example of a string that has to be passed when connecting to a database
            /// "Server=.\\SQLEXPRESS;Database=TravelConnDB;Integrated Security=true";

            /// Initialises all the UI components that are dependent on the data just loaded
            map.Initialise(busNetwork.busStops,busNetwork.links);
            IniDropDowns();

            splashWindow.Hide();
        }

        private void IniDropDowns()
        {
            /// Go through each busStop in the busNetwork and add their names to the drop down menus
            foreach (CBusStop busStop in busNetwork.busStops)
            {
                dropDownA.Items.Add(busStop.name);
                dropDownB.Items.Add(busStop.name);
            }

            /// Add integers 0 to 23, inclusive, to the hour drop down menu
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                    hoursComboBox.Items.Add("0" + i.ToString());
                else
                    hoursComboBox.Items.Add(i.ToString());
            }

            ///Add minutes to the minute drop down menu, every 5 min
            for (int i = 0; i < 60; i = i + 5)
            {
                if (i < 10)
                    minCombobox.Items.Add("0" + i.ToString());
                else
                    minCombobox.Items.Add(i.ToString());
            }
            minCombobox.Items.Add("01");
            ///Set the time in the drop downs to be the current time
            SetTimeDropBoxes();
        }

        ///Based on the Windows current clock time, sets a default time in the drop downs 
        private void SetTimeDropBoxes()
        {
            ///Gets the current time
            DateTime now = DateTime.Now;
            int nowHour = now.Hour;
            int nowMin = now.Minute;

            ///The time is set so that it's always rounding up to the nearst 5 min
            ///There is a special case when the time is 23:55 because rounding up would run it into the next day
            if (nowMin >= 55){
                if (nowHour == 23)
                    nowMin = 55;
                else{
                    nowHour += 1;
                    nowMin = 0;
                }
            }
            else
                nowMin = ((nowMin / 5) + 1) * 5;
            
            if (nowHour < 10)
                hoursComboBox.SelectedItem = "0" + nowHour.ToString();
            else
                hoursComboBox.SelectedItem = nowHour.ToString();

            if (nowMin < 10)
                minCombobox.SelectedItem = "0" + nowMin.ToString();
            else
                minCombobox.SelectedItem = nowMin.ToString();
        }

        /// When mouse leaves the bus stop control
        private void BusStopOnMap_MouseLeave(object sender, MouseEventArgs e)
        {
            busStopNameTextBlock.Text = "Bus stop name";
        }

        ///When mouse enters the bus stop control
        private void BusStopOnMap_MouseEnter(object sender, MouseEventArgs e)
        {
            var busStopOnMap = sender as Controls.BusStopControl;
            busStopNameTextBlock.Text = busStopOnMap.busStop.name;
            busStopNameTextBlock.Opacity = 1;
        }

        //Interface method to calculate the quickest journey given that details provided passes all validation
        private void CalculateJourney(DateTime date, CBusStop startbusStop, CBusStop endbusStop, string hours, string mins)
        {
            ///Convert time to a string form
            int time = Convert.ToInt32(hours + mins);
            
            ///Instantiate the journey object
            List<CTimetableRecord> journey = new List<CTimetableRecord>();

            ///Chooses appropriate method to calculate the quickest time
            if (departafterRadBut.IsChecked == true)
                journey = busNetwork.CalcDepartAfterPath(startbusStop, endbusStop, time);
            else if (arrivebeforeRadBut.IsChecked == true)
                journey = busNetwork.CalcArriveBeforePath(startbusStop, endbusStop, time);

            ///If returns null display error else add a feedback table control to the feedback stack list
            if (journey == null){
                const string message = "No Route Found!";
                const string caption = "Calculating";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Controls.FeedbackTable resultForm = new Controls.FeedbackTable(date,journey);
                foreach (Controls.FeedbackTable form in FeedbackStackList.Items)
                {
                    form.ChangetoState("resting");
                }
                FeedbackStackList.Items.Add(resultForm);
                resultForm.ChangetoState("highlighted");
            }
        }

        ///When the calculate button is clicked
        private void calcButton_Click(object sender, RoutedEventArgs e)
        {
            //Validates whether the bus stops have been selected from the drop down menus
            if ((dropDownA.SelectedItem != null) && (dropDownB.SelectedItem != null))
            {
                ///Display the splashWindow while calculating
                splashWindow.Topmost = true;
                splashWindow.Focus();
                splashWindow.Show();
                splashWindow.Topmost = true;
                splashWindow.Focus();

                ///Gets selected bus stops and time from the drop down menus 
                string hours = hoursComboBox.SelectedItem.ToString();
                string mins = minCombobox.SelectedItem.ToString();

                string busStopA = dropDownA.SelectedItem.ToString();
                string busStopB = dropDownB.SelectedItem.ToString();

                ///Gets the selected bus stop object by looking at the name
                CBusStop startStop = busNetwork.GetBusStopByName(busStopA);
                CBusStop endStop = busNetwork.GetBusStopByName(busStopB);

                ///Checks the date and whether the day category matches the seleced date from the calendar
                DateTime date = CheckDateDayCat();

                CalculateJourney(date, startStop, endStop, hours, mins);

                //Once calculated hide the splash screen
                splashWindow.Hide();
            }
            else {
                ///Displays an error if destination or departure bus stops have not been selected from the bus stop
                const string message = "Select departure and destination bus stop!";
                const string caption = "Calculating";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// Checks what date was selected and looks if the day categories match; 
        /// reloads timetable if the day category loaded is different
        /// Returns the date
        private DateTime CheckDateDayCat()
        {
            ///Gets the selected date from the calendar
            DateTime? selectedDate = calendarControl.SelectedDate; 
            //the question mark means it is a nullable variable type
            //null is returned if a new date was not selected

            if (selectedDate != null){
                ///Gets the day category of the selected date
                CDayCategory selectedDateDayCat = busNetwork.GetDayCatByDate((DateTime)selectedDate);

                /// if the day cat. is not the same as the current day cat. then load the correct timetable
                if (busNetwork.currentDayCat.dayCatID != selectedDateDayCat.dayCatID){

                    foreach (CLink link in busNetwork.links){
                        link.linkTimetable = new List<CTimetableRecord>();
                    }
                    busNetwork.currentDayCat = selectedDateDayCat;
                    busNetwork.LoadTimetable();
                }
                return (DateTime)selectedDate;
            }
            else return DateTime.Today;
        }

        ///Clears all the feedback forms from the stack list
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            FeedbackStackList.Items.Clear();
        }
       
        private void dropDownA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (CBusStop busStop in busNetwork.busStops)
            {
                if (busStop.busStopControl.CurrentType() != "end")
                {
                    busStop.busStopControl.ChangeType("default");
                }
            }
            CBusStop busStopA = busNetwork.GetBusStopByName(dropDownA.SelectedItem.ToString());
            busStopA.busStopControl.ChangeType("start");
        }

        private void dropDownB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (CBusStop busStop in busNetwork.busStops)
            {
                if (busStop.busStopControl.CurrentType() != "start")
                {
                    busStop.busStopControl.ChangeType("default");
                }
            }
            CBusStop busStopB = busNetwork.GetBusStopByName(dropDownB.SelectedItem.ToString());
            busStopB.busStopControl.ChangeType("end");
        }

        private void busRiskupBtn_Click(object sender, RoutedEventArgs e)
        {
            if (busNetwork.changeRisk < 15){
                busNetwork.changeRisk += 1;
            }
            updateChangeRisk();
        }

        private void busRiskdownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (busNetwork.changeRisk > 0)
            {
                busNetwork.changeRisk -= 1;
            }
            updateChangeRisk();
        }

        private void updateChangeRisk() {
            busChangeRiskTxbl.Text = busNetwork.changeRisk.ToString();
        }

        private void FeedbackStackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //foreach (FeedbackTable table in FeedbackStackList.Items)
            //{
            //    table.ChangetoState("resting");
            //}

            //FeedbackTable selected = (FeedbackTable)FeedbackStackList.SelectedItem;
            ////selected.ChangetoState("selected");
        }

        private void endStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void startStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleTransform scaleTransform1 = new ScaleTransform(zoomSize.Value * 0.2,zoomSize.Value*0.2, 390, 242);
            map.RenderTransform = scaleTransform1;
        }

        private void backCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void backCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private Point startPoint;

        private void backCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    Point point = e.GetPosition(backCanvas);
            //    Canvas.SetTop(map, Canvas.GetTop(map) + point.Y - prevPoint.Y);
            //    Canvas.SetLeft(map, Canvas.GetLeft(map) + point.X - prevPoint.X);
            //    Console.WriteLine(backCanvas. + point.Y - prevPoint.Y);
            //    Console.WriteLine(Canvas.GetTop(map) + point.X - prevPoint.X);
            //}
            //else prevPoint = e.GetPosition(backCanvas);

            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void backCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ListViewItem
                Map myMap = sender as Controls.Map;

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", myMap);
                DragDrop.DoDragDrop(myMap, dragData, DragDropEffects.Move);
            }
        }
    }
}

