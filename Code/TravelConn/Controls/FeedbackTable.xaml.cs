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
    /// Interaction logic for FeedbackTable.xaml
    /// </summary>
    public partial class FeedbackTable : UserControl
    {
        public FeedbackTable()
        {
            InitializeComponent();
        }

        public FeedbackTable(DateTime date, List<CTimetableRecord> journey){
            InitializeComponent();
            this.journey = journey;
            setTextBlocks(date, journey);
        }

        private string getMinuteDifference(string time1, string time2){
            int hours1 = Convert.ToInt16(time1.Split(':')[0]);
            int hours2 = Convert.ToInt16(time2.Split(':')[0]);
            int mins1 = Convert.ToInt16(time1.Split(':')[1]);
            int mins2 = Convert.ToInt16(time2.Split(':')[1]);

            int difmins = (mins2 - mins1) + (hours2 - hours1)*60;
            return difmins.ToString();
        }

        private void setTextBlocks(DateTime date,List<CTimetableRecord> journey)
        {
            dateTxbl.Text = date.Date.Day+"/"+date.Date.Month+"/"+date.Date.Year;
            fromTxbl.Text = journey[0].link.busStopA.name;
            toTxbl.Text = journey[journey.Count() - 1].link.busStopB.name;

            string departTime = intTimeToStr(journey[0].departTime);
            string arriveTime = intTimeToStr(journey[journey.Count() - 1].arrivalTime);

            departTimeTxbl.Text = departTime;
            arriveTimeTxbl.Text = arriveTime;

            totalTimeTxbl.Text = getMinuteDifference(departTime,arriveTime) + " min";

            CBus tempBus = new CBus();
            int tempCourse = 0;

            int busChanges = 0;

            tempBus = journey[0].link.bus;
            tempCourse = journey[0].course;

            foreach (CTimetableRecord record in journey)
            {
                if ((tempCourse != record.course)||(tempBus != record.link.bus)){
                    busChanges++;
                    record.link.busStopA.busStopControl.ChangeType("buschange");
                }
                else
                {
                    record.link.busStopA.busStopControl.ChangeType("injourney");
                }

                tempCourse = record.course;
                tempBus = record.link.bus;
            }

            journey[0].link.busStopA.busStopControl.ChangeType("start");
            journey[journey.Count() - 1].link.busStopB.busStopControl.ChangeType("end");

            changesTxbl.Text = busChanges.ToString();
        }

        private string intTimeToStr(int intTime)
        {
            string stringTime = "";
            string temp = intTime.ToString();
            
            if (temp.Count() == 3)
                stringTime = "0" + temp[0] + ":" + temp[1] + temp[2];
            else
                stringTime = temp[0]+ "" + temp[1] + ":" + temp[2] + temp[3];
            return stringTime;
        }

        private string currentstate;

        public List<CTimetableRecord> journey;

        public void ChangetoState(string state)
        {
            switch (state)
            {
                case "resting":

                    foreach (CTimetableRecord record in journey)
                    {
                        record.link.busStopA.busStopControl.ChangeState("resting");
                    }

                    journey[journey.Count() - 1].link.busStopB.busStopControl.ChangeState("resting");

                    FeedbackGrid.Style = (Style)FindResource("resting");
                    break;

                case "selected":
                    CBus tempBus = new CBus();
                    int tempCourse = 0;

                    tempBus = journey[0].link.bus;
                    tempCourse = journey[0].course;                

                    foreach (CTimetableRecord record in journey)
                    {
                        if ((tempCourse != record.course) || (tempBus != record.link.bus))
                        {
                            record.link.busStopA.busStopControl.ChangeType("buschange");
                        }
                        else
                        {
                            record.link.busStopA.busStopControl.ChangeType("injourney");
                        }
                        tempCourse = record.course;
                        tempBus = record.link.bus;
                    }


                    FeedbackGrid.Style = (Style)FindResource("selected");
                    break;

                case "highlighted":
                    tempBus = journey[0].link.bus;
                    tempCourse = journey[0].course;                

                    foreach (CTimetableRecord record in journey)
                    {
                        if ((tempCourse != record.course) || (tempBus != record.link.bus))
                        {
                            record.link.busStopA.busStopControl.ChangeType("buschange");
                        }
                        else
                        {
                            record.link.busStopA.busStopControl.ChangeType("injourney");
                        }
                        tempCourse = record.course;
                        tempBus = record.link.bus;
                    }

                    journey[0].link.busStopA.busStopControl.ChangeType("start");
                    journey[journey.Count() - 1].link.busStopB.busStopControl.ChangeType("end");

                    FeedbackGrid.Style = (Style)FindResource("selected");
                    break;
             }
            this.currentstate = state;

            
        }

        public Section GetJourneySection(){
            Section form = new Section();

            string departTime = intTimeToStr(journey[0].departTime);
            string arrivalTime = intTimeToStr(journey[journey.Count() - 1].arrivalTime);


            // header table
            Table headertbl = new Table();
            
            headertbl.Columns.Add(new TableColumn());
            headertbl.Columns.Add(new TableColumn());

            headertbl.RowGroups.Add(new TableRowGroup());

            headertbl.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = headertbl.RowGroups[0].Rows[0];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Journey from: "))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journey[0].link.busStopA.name))));

            headertbl.RowGroups[0].Rows.Add(new TableRow());
            currentRow = headertbl.RowGroups[0].Rows[1];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depart at: "))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(departTime))));

            headertbl.RowGroups[0].Rows.Add(new TableRow());
            currentRow = headertbl.RowGroups[0].Rows[2];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Bus Name: "))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journey[0].link.bus.name))));

            form.Blocks.Add(headertbl);

            // for tracking use
            CTimetableRecord prevbestLink = new CTimetableRecord();
            prevbestLink = journey[0];
            int busChanges = 0;

            Table busChangeTbl = new Table();
            busChangeTbl.Columns.Add(new TableColumn());
            busChangeTbl.Columns.Add(new TableColumn());

            foreach (CTimetableRecord record in this.journey)
            {
                if ((prevbestLink.course != record.course) || (prevbestLink.link.bus != record.link.bus))
                {
                    busChangeTbl.RowGroups.Add(new TableRowGroup());
                    busChangeTbl.RowGroups[busChanges].Rows.Add(new TableRow());
                    currentRow = busChangeTbl.RowGroups[busChanges].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Get off at: "))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(prevbestLink.link.busStopB.name))));

                    busChangeTbl.RowGroups[busChanges].Rows.Add(new TableRow());
                    currentRow = busChangeTbl.RowGroups[busChanges].Rows[1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("      at: "))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(intTimeToStr(prevbestLink.arrivalTime)))));

                    busChangeTbl.RowGroups[busChanges].Rows.Add(new TableRow());
                    currentRow = busChangeTbl.RowGroups[busChanges].Rows[2];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Take bus: "))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(record.link.bus.name))));

                    busChangeTbl.RowGroups[busChanges].Rows.Add(new TableRow());
                    currentRow = busChangeTbl.RowGroups[busChanges].Rows[3];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("      at: "))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(intTimeToStr(record.departTime)))));

                    busChanges++;
                }
                prevbestLink = record;
            }
            form.Blocks.Add(busChangeTbl);

            //footer table
            Table footertbl = new Table();

            footertbl.Columns.Add(new TableColumn());
            footertbl.Columns.Add(new TableColumn());

            footertbl.RowGroups.Add(new TableRowGroup());

            footertbl.RowGroups[0].Rows.Add(new TableRow());
            currentRow = footertbl.RowGroups[0].Rows[0];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Arrive at :"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journey[journey.Count() - 1].link.busStopB.name))));

            footertbl.RowGroups[0].Rows.Add(new TableRow());
            currentRow = footertbl.RowGroups[0].Rows[1];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("      at: "))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(arrivalTime))));

            footertbl.RowGroups[0].Rows.Add(new TableRow());
            currentRow = footertbl.RowGroups[0].Rows[2];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total Duration: "))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(getMinuteDifference(departTime, arrivalTime) + " min"))));

            footertbl.RowGroups[0].Rows.Add(new TableRow());
            currentRow = footertbl.RowGroups[0].Rows[3];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total Changes: "))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(busChanges.ToString()))));

            form.Blocks.Add(footertbl);

            return form;
        }

        private void FeedbackGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentstate != "selected")
            {
                ChangetoState("highlighted");
            }
        }
       
        private void FeedbackGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (currentstate == "highlighted")
            {
                ChangetoState("resting");
            }
        }

        private void FeedbackGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangetoState("selected");
        }

        private void moreBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangetoState("selected");
            MoreInfoWindow infoWindow = new MoreInfoWindow();
            infoWindow.textField.Blocks.Add(GetJourneySection());
            infoWindow.Show();
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();

            FlowDocument printDocument = new FlowDocument();
            printDocument.Name = "JourneyPrintForm";

            var separator = new Rectangle();
            separator.Stroke = new SolidColorBrush(Colors.Blue);
            separator.StrokeThickness = 3;
            separator.Height = 3;
            separator.Width = double.NaN;

            printDocument.Blocks.Add(GetJourneySection());

            IDocumentPaginatorSource idpSource = printDocument;

            printDlg.PageRangeSelection = PageRangeSelection.AllPages;
            printDlg.UserPageRangeEnabled = true;

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = printDlg.ShowDialog();
            if (print == true)
            {
                printDlg.PrintDocument(idpSource.DocumentPaginator, "Bus Journey Printing");
            }
        }
    }
}
