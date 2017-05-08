using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Photography.Data;
using Photography.Models;

namespace ImportDataFromXML
{
    class Program
    {
        private const string AccessoriesPath = @"../../../datasets/accessories.xml";
        private const string WorkshopsPath = @"../../../datasets/workshops.xml";
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            UnitOfWork unit = new UnitOfWork();
            //ImportAccessories(unit);
            ImportWorkshops(unit);
        }

        private static void ImportWorkshops(UnitOfWork unit)
        {
            XDocument document = XDocument.Load(WorkshopsPath);
            var workshopsXml = document.Descendants("workshop");
            foreach (var element in workshopsXml)
            {
                string trainer;
                try
                {

                    trainer = element.Descendants("trainer").First().Value;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Error. Invalid data provided");
                    continue;
                }
                var workshopNameAttr = element.Attribute("name");
                var workshopLocationAttr = element.Attribute("location");
                var workshopStartDateAttr = element.Attribute("start-date");
                var workshopEndDateAttr = element.Attribute("end-date");
                var workshopPriceAttr = element.Attribute("price");
                if (workshopPriceAttr == null || workshopNameAttr == null || trainer == null ||
                    workshopLocationAttr == null)
                {
                    Console.WriteLine("Error. Invalid data provided");
                    continue;
                }
                //Console.WriteLine(workshopNameAttr.Value);
                string workshopName = workshopNameAttr.Value;
                string workshopLocation = workshopLocationAttr.Value;
                decimal workshopPrice = decimal.Parse(workshopPriceAttr.Value);
                DateTime? workshopStartDate = null;
                if (workshopStartDateAttr != null)
                {
                    workshopStartDate = DateTime.Parse(workshopStartDateAttr.Value);
                }
                DateTime? workshopEndDate = null;
                if (workshopEndDateAttr != null)
                {
                    workshopEndDate = DateTime.Parse(workshopEndDateAttr?.Value);
                }
                var trainerFirstName = trainer.Split(' ')[0];
                var participants = element.Descendants("participant");
                ICollection<Photographer> participantPhotographers = new List<Photographer>();
                foreach (var xElement in participants)
                {
                    string firstName = xElement.Attribute("first-name")?.Value;
                    participantPhotographers.Add(unit.Photographers.FirstOrDefault(p => p.FirstName == firstName));
                }
                Workshop workshop = new Workshop()
                {
                    Name = workshopName,
                    Location = workshopLocation,
                    PricePerParticipant = workshopPrice,
                    Trainer = unit.Photographers.FirstOrDefault(p => p.FirstName == trainerFirstName),
                    StartDate = workshopStartDate,
                    EndDate = workshopEndDate,
                    Participants = participantPhotographers
                };
                unit.Worksops.Add(workshop);
                unit.Save();
                Console.WriteLine($"Successfully imported {workshop.Name}");
            }
        }

        private static void ImportAccessories(UnitOfWork unit)
        {
            Random random = new Random();
            XDocument document = XDocument.Load(AccessoriesPath);
            var accessoriesXml = document.Descendants("accessory");
            foreach (var element in accessoriesXml)
            {
                Accessory accessory = new Accessory()
                {
                    Name = element.Attribute("name").Value
                };
                accessory.Owner = unit.Photographers.GetById(random.Next(1, 89));
                unit.Accessories.Add(accessory);
                unit.Save();
                Console.WriteLine($"Successfully imported {accessory.Name}");
            }
        }
    }
}
