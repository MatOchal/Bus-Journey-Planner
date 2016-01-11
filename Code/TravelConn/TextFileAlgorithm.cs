using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TravelConn
{
    /// CBus, CBusStop, CLink, CTimetableRecord, CDayCategory and CSpecialDay 
    /// are classes that match the tables in the database.

    public class CBus
    {
        //Core fields
        public int busID;
        public string name;
        public string description;
    }

    public class CBusStop
    {
        //Core fields
        public int busStopID;
        public string name;
        public double longitude;
        public double latitude;
        public string address;

        //Algorithm use
        public List<CLink> departureLinks;
        public List<CLink> arrivalLinks;
        public CTimetableRecord bestLinkRecord;

        //Canvas Map  
        public Controls.BusStopControl busStopControl;

        public CBusStop() { } // empty constructor

        public CBusStop(CBusStop model)
        {
            busStopID = model.busStopID;
            name = model.name;
            longitude = model.longitude;
            latitude = model.latitude;
            address = model.address;

            departureLinks = model.departureLinks;
            arrivalLinks = model.arrivalLinks;
            bestLinkRecord = model.bestLinkRecord;
        }
    }

    public class CLink
    {
        //Core fields
        public int LinkID;
        public CBus bus;
        public CBusStop busStopA;
        public CBusStop busStopB;

        //Algorithm use
        public List<CTimetableRecord> linkTimetable;

        //Canvas Map
        public Line linkOnMap;
    }

    public class CTimetableRecord
    {
        //Core fields
        public CDayCategory dayCategory;
        public CLink link;
        public int course;
        public int departTime;
        public int arrivalTime;
    }

    public class CDayCategory
    {
        //Core fields
        public string dayType;
        public int dayCatID;
    }

    public class CSpecialDay
    {
        //Core fields
        public DateTime dayDate = new DateTime();
        public CDayCategory dayCat = new CDayCategory();
    }

    /// <summary>
    /// CBusNetwork is the core class that includes all the procedure needed by the algorithm independent of the UI
    /// </summary>
    public class CBusNetwork
    {
        /// Lists bellow act as a database in object form
        public List<CBus> buses;
        public List<CBusStop> busStops;
        public List<CLink> links;
        public List<CDayCategory> dayCategories;
        public List<CSpecialDay> specialDays;

        public CDayCategory currentDayCat;
        public int changeRisk = 1;

        //Private
        string datasource; // could be "file" or "database"
        string path;// could be the path to folder with the text files or the database connection string

        /// Constructor
        public CBusNetwork(string datasource,string path) {
            buses = new List<CBus>();
            busStops = new List<CBusStop>();
            links = new List<CLink>();
            dayCategories = new List<CDayCategory>();
            specialDays = new List<CSpecialDay>();

            this.datasource = datasource;
            this.path = path;
        }

        /// Method publically visible to load core data.
        /// It decides whether to load from a database or a file,
        /// the decision is based on the datasource set during instantiatiation of the CBusNetwork class
        public void LoadCoreData()
        {
            if (datasource == "file")
                GetCoreFileData();
            else if (datasource == "database")
                GetCoreDBsData();

            this.currentDayCat = GetDayCatByDate(DateTime.Today);
        }

        public void LoadTimetable()
        {
            if (datasource == "file")
                GetFileTimetable(currentDayCat.dayCatID);
            else if (datasource == "database")
                GetDBsTimetable(currentDayCat.dayCatID);
        }

        private void GetCoreDBsData()
        {
            SqlConnection DBConnection = new SqlConnection(this.path);
            DBConnection.Open();

            string[] elements = new string[6];

            SqlCommand DBCommand = new SqlCommand("Select * From BusStops;", DBConnection);
            SqlDataReader DBReader = DBCommand.ExecuteReader();

            while (DBReader.Read())
            {
                for (int i = 0; i < 5; i++)
                {     // will change according to the number of columns
                    elements[i] = DBReader.GetSqlValue(i).ToString();
                }
                CBusStop newBusStop = new CBusStop();
                newBusStop.busStopID = Convert.ToInt32(elements[0]);
                newBusStop.name = elements[1];
                newBusStop.latitude = Convert.ToDouble(elements[2]);
                newBusStop.longitude = Convert.ToDouble(elements[3]);
                newBusStop.address = elements[4];

                newBusStop.departureLinks = new List<CLink>();
                newBusStop.arrivalLinks = new List<CLink>();

                busStops.Add(newBusStop);
            }
            DBReader.Close();

            DBCommand = new SqlCommand("Select * From DayCategories;", DBConnection);
            DBReader = DBCommand.ExecuteReader();

            while (DBReader.Read())
            {
                for (int i = 0; i < 2; i++)
                {
                    elements[i] = DBReader.GetSqlValue(i).ToString();
                }
                CDayCategory newDayCat = new CDayCategory();
                newDayCat.dayType = elements[0];
                newDayCat.dayCatID = Convert.ToInt32(elements[1]);
                dayCategories.Add(newDayCat);
            }
            DBReader.Close();

            DBCommand = new SqlCommand("Select * From Buses;", DBConnection);
            DBReader = DBCommand.ExecuteReader();

            while (DBReader.Read())
            {
                for (int i = 0; i < 3; i++)
                {     // will change according to the number of columns
                    elements[i] = DBReader.GetSqlValue(i).ToString();
                }

                CBus newBus = new CBus();
                newBus.busID = Convert.ToInt32(elements[0]);
                newBus.name = elements[1];
                newBus.description = elements[2];
                buses.Add(newBus);
            }
            DBReader.Close();

            DBCommand = new SqlCommand("Select * From BusLinks;", DBConnection);
            DBReader = DBCommand.ExecuteReader();

            while (DBReader.Read())
            {
                for (int i = 0; i < 4; i++)
                {     // will change according to the number of columns
                    elements[i] = DBReader.GetSqlValue(i).ToString();
                }
                CLink newLink = new CLink();
                newLink.LinkID = Convert.ToInt32(elements[0]);
                newLink.bus = GetBusByID(Convert.ToInt32(elements[1]));
                newLink.busStopA = GetBusStopByID(Convert.ToInt32(elements[2]));
                newLink.busStopB = GetBusStopByID(Convert.ToInt32(elements[3]));

                newLink.linkTimetable = new List<CTimetableRecord>();
                //add to list of Links to bus stop the bus is departing from
                newLink.busStopA.departureLinks.Add(newLink);
                newLink.busStopB.arrivalLinks.Add(newLink);

                links.Add(newLink);
            }
            DBReader.Close();
        }

        public void GetDBsTimetable(int dayCatID)
        {
            SqlConnection DBConnection = new SqlConnection(this.path);
            DBConnection.Open();

            SqlCommand DBCommand = new SqlCommand("Select * From Timetable;", DBConnection);
            SqlDataReader DBReader = DBCommand.ExecuteReader();

            string[] elements = new string[6];

            while (DBReader.Read())
            {
                for (int i = 0; i < 5; i++)
                {
                    elements[i] = DBReader.GetSqlValue(i).ToString();
                }

                if (Convert.ToInt32(elements[0])==dayCatID)
                {
                    CTimetableRecord linkTimetable = new CTimetableRecord();
                    linkTimetable.dayCategory = GetDayCatbyID(Convert.ToInt32(elements[0]));
                    linkTimetable.link = GetLinkByID(Convert.ToInt32(elements[1]));
                    linkTimetable.course = Convert.ToInt32(elements[2]);
                    linkTimetable.departTime = Convert.ToInt32(elements[3]);
                    linkTimetable.arrivalTime = Convert.ToInt32(elements[4]);
               
                    //add the timetable record to each bus stop timetable
                    linkTimetable.link.linkTimetable.Add(linkTimetable);
                }
            }
            DBReader.Close();
        }

        private void GetCoreFileData()
        {
            string[] lines = File.ReadAllLines(this.path + "DayCategories.txt");
            foreach (String line in lines)
            {
                if (line != lines[0])
                {
                    string[] elements = line.Split('\t');
                    CDayCategory newDayCat = new CDayCategory();
                    newDayCat.dayType = elements[0];
                    newDayCat.dayCatID = Convert.ToInt32(elements[1]);
                    dayCategories.Add(newDayCat);
                }
            }

            lines = File.ReadAllLines(this.path + "BusStops.txt");
            foreach (String line in lines)
            {
                if (line != lines[0])
                {
                    string[] elements = line.Split('\t');

                    CBusStop newBusStop = new CBusStop();
                    newBusStop.busStopID = Convert.ToInt32(elements[0]);
                    newBusStop.name = elements[1];
                    newBusStop.latitude = Convert.ToDouble(elements[2]);
                    newBusStop.longitude = Convert.ToDouble(elements[3]);
                    newBusStop.address = elements[4];

                    newBusStop.departureLinks = new List<CLink>();
                    newBusStop.arrivalLinks = new List<CLink>();
                    busStops.Add(newBusStop);
                }
            }

            lines = File.ReadAllLines(this.path + "Buses.txt");
            foreach (String line in lines)
            {
                if (line != lines[0])
                {
                    string[] elements = line.Split('\t');

                    CBus newBus = new CBus();
                    newBus.busID = Convert.ToInt32(elements[0]);
                    newBus.name = elements[1];
                    newBus.description = elements[2];
                    buses.Add(newBus);
                }
            }

            lines = File.ReadAllLines(this.path + "Links.txt");
            foreach (String line in lines)
            {
                if (line != lines[0])
                {
                    string[] elements = line.Split('\t');

                    CLink newLink = new CLink();
                    newLink.LinkID = Convert.ToInt32(elements[0]);
                    newLink.bus = GetBusByID(Convert.ToInt32(elements[1]));
                    newLink.busStopA = GetBusStopByID(Convert.ToInt32(elements[2]));
                    newLink.busStopB = GetBusStopByID(Convert.ToInt32(elements[3]));

                    newLink.linkTimetable = new List<CTimetableRecord>();
                    //add to list of Links to bus stop the bus is departing from
                    newLink.busStopA.departureLinks.Add(newLink);
                    newLink.busStopB.arrivalLinks.Add(newLink);
                    links.Add(newLink);
                }
            }

            lines = File.ReadAllLines(this.path + "SpecialDays.txt");
            foreach (String line in lines)
            {
                if (line != lines[0])
                {
                    string[] elements = line.Split('\t');
                    CSpecialDay newSpecDay = new CSpecialDay();
                    newSpecDay.dayDate = new DateTime(Convert.ToInt32(elements[0].Split('/')[0]), Convert.ToInt32(elements[0].Split('/')[1]), Convert.ToInt32(elements[0].Split('/')[2]));
                    newSpecDay.dayCat = GetDayCatbyID(Convert.ToInt32(elements[1]));
                    specialDays.Add(newSpecDay);
                }
            }
        }
    
        //get a timetable from a file
        public void GetFileTimetable(int dayCatID)
        {
            string[] lines = File.ReadAllLines(this.path + "Timetable.txt");
            foreach (String line in lines)
            {
                if (line != lines[0])
                {
                    string[] elements = line.Split('\t');

                    if (Convert.ToInt32(elements[0]) == dayCatID)
                    {
                        CTimetableRecord linkTimetable = new CTimetableRecord();
                        linkTimetable.dayCategory = GetDayCatbyID(Convert.ToInt32(elements[0]));
                        linkTimetable.link = GetLinkByID(Convert.ToInt32(elements[1]));
                        linkTimetable.course = Convert.ToInt32(elements[2]);
                        linkTimetable.departTime = Convert.ToInt32(elements[3]);
                        linkTimetable.arrivalTime = Convert.ToInt32(elements[4]);

                        //add the timetable record to each bus stop timetable
                        linkTimetable.link.linkTimetable.Add(linkTimetable);
                    }
                }
            }
            Console.WriteLine("Loaded timetable");
        }
        
        public CDayCategory GetDayCatByDate(DateTime date)
        {
            int i = 0;
            while ((specialDays[i].dayDate != date.Date) && (i < specialDays.Count() - 1))
            {
                i++;
            }

            if (specialDays[i].dayDate == date.Date)
            {
                return specialDays[i].dayCat;
            }
            else
            {
                i = 0;
                while ((dayCategories[i].dayType != date.DayOfWeek.ToString()))
                {
                    i++;
                }
                return dayCategories[i];
            }
        }


        public CBusStop GetBusStopByID(int id)
        {
            int i = 0;
            while (busStops[i].busStopID != id)
            {
                i++;
            }
            return busStops[i];
        }

        public CBusStop GetBusStopByName(string name)
        {
            int i = 0;
            while (busStops[i].name != name)
            {
                i++;
            }
            return busStops[i];
        }

        public CBus GetBusByID(int id)
        {
            int i = 0;
            while (buses[i].busID != id)
            {
                i++;
            }
            return buses[i];
        }

        public CLink GetLinkByID(int id)
        {
            int i = 0;
            while (links[i].LinkID != id)
            {
                i++;
            }
            return links[i];
        }

        public CDayCategory GetDayCatbyID(int id)
        {
            int i = 0;
            while (dayCategories[i].dayCatID != id)
            {
                i++;
            }
            return dayCategories[i];
        }


        /// This is the first of the two main algorithms for calculating the shortest path
        /// Accepting 2 CBusStop objects and time as integer will output a CTimetableRecord for each link in the calculated journey
        public List<CTimetableRecord> CalcDepartAfterPath(CBusStop startBusStop, CBusStop endBusStop, int time)
        {
            /// Recreating all the best links so that no data is left behind when running the algorithm for the next time
            RecreatebestLinkRecords();

            /// A new list of all the bus stops which have not been visited by the algorithm yet 
            List<CBusStop> unvisitedBusStops = new List<CBusStop>(busStops);
         
            /// currentBusStop will keep track of the bus stop the algorithm uses at an instance.
            /// Starts at the startBusStop
            CBusStop currentBusStop = startBusStop;

            /// This loop finds bestLinkRecord for each of the first bus stop neighbours
            /// Since it doesn't take the busChangeRisk into consideration it's a special case and is outside the main loop;
            /// it doesn't take the busChangeRisk into consideration because the journey can begin by taking any bus departing from the bus stop;
            foreach (CLink link in currentBusStop.departureLinks)
            { 
                foreach (CTimetableRecord record in link.linkTimetable)
                {
                    /// If the "departure time" from the "current bus stop" is more or equal to the "time"
                    /// AND
                    /// the "arrival time" of that timetable record, beats the "arrival time" of the "current best link"
                    /// THEN overide the "best link" with the timetable record that was just found
                    if ((record.departTime >= time) && (record.arrivalTime < link.busStopB.bestLinkRecord.arrivalTime))
                    {
                        link.busStopB.bestLinkRecord = record;
                    }
                }
            }

            /// After working out the best 
            ///Removes the current bus stop from the unvisited bus stops list since
            unvisitedBusStops.Remove(currentBusStop);

            ///FindQuickestArrival given a list of bus stops will chooses the one with the lowest/best "arrival time" of the "best link record"
            currentBusStop = FindQuickestArrival(unvisitedBusStops);
            unvisitedBusStops.Remove(currentBusStop);

            /// This is the main loop. It works by finding the "best link record" like in the loop above.
            /// Going through each bus stop in turn, until the "current bus stop" is the "end bus stop"
            /// OR
            /// 
            while ((currentBusStop != endBusStop) && (unvisitedBusStops.Count > 0))
            {
                foreach (CLink link in currentBusStop.departureLinks){
                    //find the ones with departure time straight after the given time in the 
                    foreach (CTimetableRecord record in link.linkTimetable){
                        if ((((record.course == currentBusStop.bestLinkRecord.course) & (record.link.bus == currentBusStop.bestLinkRecord.link.bus)) || (record.departTime >= currentBusStop.bestLinkRecord.arrivalTime + changeRisk)) && (record.arrivalTime < record.link.busStopB.bestLinkRecord.arrivalTime))
                        {
                            if (unvisitedBusStops.Contains(record.link.busStopB))
                                record.link.busStopB.bestLinkRecord = record;
                        }
                    }
                }
                currentBusStop = FindQuickestArrival(unvisitedBusStops); //choose the busstop with the quickest arrival time out of the unvisited bus stops
                unvisitedBusStops.Remove(currentBusStop);
            }
                      
            //Get the route by looping back from the end bus stop to the start bus stop
            List<CTimetableRecord> quickestJourney = new List<CTimetableRecord>();
            if (endBusStop.bestLinkRecord.arrivalTime != 9999)
            {
                currentBusStop = endBusStop;
                quickestJourney.Add(currentBusStop.bestLinkRecord);
                int count = 0;
                while ((currentBusStop.bestLinkRecord.link.busStopA != startBusStop))// && (count < 1000))
                {
                    count++;
                    Console.WriteLine(count);
                    CBusStop busStopA = currentBusStop.bestLinkRecord.link.busStopA;
                    CBusStop busStopB = currentBusStop.bestLinkRecord.link.busStopB;
                    Console.WriteLine("From: " + busStopA.name + " at: " + currentBusStop.bestLinkRecord.departTime + " To: " + busStopB.name + " at: " + currentBusStop.bestLinkRecord.arrivalTime + " using: " + currentBusStop.bestLinkRecord.link.bus.name);
                    currentBusStop = currentBusStop.bestLinkRecord.link.busStopA;
                    quickestJourney.Add(currentBusStop.bestLinkRecord);
                }
                quickestJourney.Reverse();
            }
            else quickestJourney = null;
            return quickestJourney; 
        }

        public void RecreatebestLinkRecords()
        {
            foreach (CBusStop busStop in busStops)
            {
                busStop.bestLinkRecord = new CTimetableRecord();
                busStop.bestLinkRecord.arrivalTime = 9999;
                busStop.bestLinkRecord.departTime = 9999;
                busStop.bestLinkRecord.link = new CLink();
                busStop.bestLinkRecord.course = 0;
                busStop.bestLinkRecord.dayCategory = new CDayCategory();
            }
        }

        public List<CTimetableRecord> CalcArriveBeforePath(CBusStop startBusStop, CBusStop endBusStop, int time)
        {
            RecreatebestLinkRecords();

            List<CBusStop> unvisitedBusStops = new List<CBusStop>(busStops);           

            // start from the destination (end) bus stop
            CBusStop currentBusStop = endBusStop;

            foreach (CBusStop busStop in busStops)
            {
                busStop.bestLinkRecord.arrivalTime = -9999;
                busStop.bestLinkRecord.departTime = -9999;
                busStop.bestLinkRecord.link = new CLink();
                busStop.bestLinkRecord.course = 0;
                busStop.bestLinkRecord.dayCategory = new CDayCategory();
            }

            //finds the ones that have the best arrival time 
            //special case the start bus stop
            foreach (CLink arrivalLink in currentBusStop.arrivalLinks)
            {
                //find the ones with departure time straight after the given time in the 
                foreach (CTimetableRecord record in arrivalLink.linkTimetable)
                {
                    if ((record.arrivalTime <= time) && (record.arrivalTime > arrivalLink.busStopA.bestLinkRecord.arrivalTime))
                    {
                        arrivalLink.busStopA.bestLinkRecord = record;
                    }
                }
            }
            unvisitedBusStops.Remove(currentBusStop);

            while ((unvisitedBusStops.Count > 0) && (currentBusStop != startBusStop))
            {
                currentBusStop = FindLatestArrival(unvisitedBusStops); //choose the busstop with the quickest arrival time out of the unvisited bus stops
                unvisitedBusStops.Remove(currentBusStop);

                foreach (CLink arrivalLink in currentBusStop.arrivalLinks)
                {
                    //find the ones with arrival time is straight before the given time 
                    foreach (CTimetableRecord record in arrivalLink.linkTimetable)
                    {
                        if ((((record.course == currentBusStop.bestLinkRecord.course) && (record.link.bus == currentBusStop.bestLinkRecord.link.bus)) || (record.arrivalTime <= currentBusStop.bestLinkRecord.departTime - changeRisk)) && (record.arrivalTime > arrivalLink.busStopA.bestLinkRecord.arrivalTime))
                            record.link.busStopA.bestLinkRecord = record;
                    }
                }
            }

            //Get the route by looping back from the end bus stop to the start bus stop
            List<CTimetableRecord> quickestJourney = new List<CTimetableRecord>();
            if (currentBusStop.bestLinkRecord.arrivalTime != -9999)
            {
                currentBusStop = startBusStop;
                quickestJourney.Add(currentBusStop.bestLinkRecord);
                int count = 0;

                while ((currentBusStop.bestLinkRecord.link.busStopB != endBusStop))// && (count < 1000))
                {
                    count++;
                    Console.WriteLine(currentBusStop.bestLinkRecord.link.busStopA.name);
                    currentBusStop = currentBusStop.bestLinkRecord.link.busStopB;
                    quickestJourney.Add(currentBusStop.bestLinkRecord);
                }
            }
            else quickestJourney = null;
            return quickestJourney;
        }


        public CBusStop FindQuickestArrival(List<CBusStop> list)
        {
            CBusStop tempBusStop = new CBusStop();
            tempBusStop.bestLinkRecord = new CTimetableRecord();
            tempBusStop.bestLinkRecord.arrivalTime = 9999;
            foreach (CBusStop busStop in list)
            {
                if (busStop.bestLinkRecord.arrivalTime <= tempBusStop.bestLinkRecord.arrivalTime)
                    tempBusStop = busStop;
            }
            return tempBusStop;
        }

        public CBusStop FindLatestArrival(List<CBusStop> list)
        {
            CBusStop tempBusStop = new CBusStop();
            tempBusStop.bestLinkRecord = new CTimetableRecord();
            tempBusStop.bestLinkRecord.arrivalTime = -9999;
            foreach (CBusStop busStop in list)
            {
                if (busStop.bestLinkRecord.arrivalTime >= tempBusStop.bestLinkRecord.arrivalTime)
                    tempBusStop = busStop;
            }
            return tempBusStop;
        }
    }
}