using AMONICAirlinesDesktopApp_Session2.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AMONICAirlinesDesktopApp_Session2.Services
{
    public class CsvScheduleImporter
        : IScheduleImporter, INotifyPropertyChanged
    {
        private int successfulChangesCount;
        private int duplicateRecordsCount;
        private int recordsWithMissingFieldsCount;

        public int SuccessfulChangesCount
        {
            get => successfulChangesCount;
            set
            {
                successfulChangesCount = value;
                PropertyChanged?
                    .Invoke(
                        this,
                        new PropertyChangedEventArgs(
                            nameof(SuccessfulChangesCount)));
            }
        }
        public int DuplicateRecordsCount
        {
            get => duplicateRecordsCount;
            set
            {
                duplicateRecordsCount = value;
                PropertyChanged?
                  .Invoke(
                      this,
                      new PropertyChangedEventArgs(
                          nameof(DuplicateRecordsCount)));
            }
        }
        public int RecordsWithMissingFieldsCount
        {
            get => recordsWithMissingFieldsCount;
            set
            {
                recordsWithMissingFieldsCount = value;
                PropertyChanged?
                  .Invoke(
                      this,
                      new PropertyChangedEventArgs(
                          nameof(RecordsWithMissingFieldsCount)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Import(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            List<string[]> performedLines = new List<string[]>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] fields = line.Split(',');
                if (fields.Length != 9)
                {
                    RecordsWithMissingFieldsCount++;
                    continue;
                }

                if (performedLines.Any(pl => pl[1] == fields[1]
                    || pl[3] == fields[3]))
                {
                    DuplicateRecordsCount++;
                    continue;
                }

                string action = fields[0];
                _ = DateTime.TryParse(fields[1], out DateTime flightDate);
                _ = TimeSpan.TryParse(fields[2], out TimeSpan flightTime);
                string flightNumber = fields[3];
                string fromAirportIATACode = fields[4];
                string toAirportIATACode = fields[5];
                _ = int.TryParse(fields[6], out int aircraftId);
                _ = decimal.TryParse(fields[7], out decimal basePrice);
                bool isConfirmed = fields[8]
                    .Trim()
                    .Length == 2;

                switch (fields[0])
                {
                    case "EDIT":
                        try
                        {
                            using (SessionTwoEntities context =
                                new SessionTwoEntities())
                            {
                                Schedule schedule = context
                                    .Schedules
                                    .FirstOrDefault(s => s.Date == flightDate
                                    && s.FlightNumber == flightNumber);

                                if (schedule == null)
                                {
                                    continue;
                                }

                                schedule.Date = flightDate;
                                schedule.Time = flightTime;
                                schedule.Route.DepartureAirportID = context
                                    .Airports
                                    .First(a => a.IATACode
                                    == fromAirportIATACode)
                                    .ID;
                                schedule.Route.ArrivalAirportID = context
                                    .Airports
                                    .First(a => a.IATACode
                                    == toAirportIATACode)
                                    .ID;
                                schedule.AircraftID = aircraftId;
                                schedule.Confirmed = isConfirmed;
                                schedule.EconomyPrice = basePrice;
                                _ = context.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.StackTrace);
                        }
                        break;
                    case "ADD":
                        try
                        {
                            using (SessionTwoEntities context =
                                new SessionTwoEntities())
                            {
                                Route route = context.Routes
                                    .FirstOrDefault(r =>
                                    r.ArrivalAirportID == context
                                    .Airports
                                    .First(a => a.IATACode
                                    == fromAirportIATACode)
                                    .ID && r.ArrivalAirportID == context
                                    .Airports
                                    .First(a => a.IATACode
                                    == toAirportIATACode)
                                    .ID);
                                if (route == null)
                                {
                                    continue;
                                }
                                Schedule schedule = new Schedule
                                {
                                    Date = flightDate,
                                    Time = flightTime,
                                    AircraftID = aircraftId,
                                    Route = route,
                                    Confirmed = isConfirmed,
                                    EconomyPrice = basePrice,
                                    FlightNumber = flightNumber.ToString(),
                                };
                                schedule.Date = flightDate;
                                schedule.Time = flightTime;
                                schedule.Route.DepartureAirportID = context
                                    .Airports
                                    .First(a => a.IATACode
                                    == fromAirportIATACode)
                                    .ID;
                                schedule.Route.ArrivalAirportID = context
                                    .Airports
                                    .First(a => a.IATACode
                                    == toAirportIATACode)
                                    .ID;
                                schedule.AircraftID = aircraftId;
                                schedule.Confirmed = isConfirmed;
                                _ = context.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.StackTrace);
                        }
                        break;
                    default:
                        break;
                }
                SuccessfulChangesCount++;
            }
            return true;
        }
    }
}
