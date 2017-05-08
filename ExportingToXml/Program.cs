using System.Linq;
using System.Xml.Linq;
using Photography.Data;

namespace ExportingToXml
{
    class Program
    {
        static void Main()
        {
            PhotographyContext context = new PhotographyContext();
            PhotographersWithSameCameraMake(context);
            WorkshopByLocation(context);
        }

        private static void WorkshopByLocation(PhotographyContext context)
        {
            XElement root = new XElement("locations");
            foreach (var workshop in context.Workshops.Where(w=>true))
            {
                var location = workshop.Location;
                var workshops = context.Workshops.Where(w => w.Location == location && w.Participants.Count >= 5);
                if (!workshops.Any())
                {
                    continue;
                }
                XElement locationElement = new XElement("location");
                locationElement.SetAttributeValue("name",location);
                foreach (var w in workshops)
                {
                        XElement workshopElement = new XElement("workshop");
                        workshopElement.SetAttributeValue("name", w.Name);
                        workshopElement.SetAttributeValue("total-profit",
                            w.Participants.Count*w.PricePerParticipant-0.2m* (w.Participants.Count * w.PricePerParticipant));
                        XElement participants = new XElement("participants");
                        participants.SetAttributeValue("count", w.Participants.Count);
                        foreach (var photographer in w.Participants)
                        {
                            XElement participant = new XElement("participant");
                            participant.SetValue($"{photographer.FirstName} {photographer.LastName}");
                            participants.Add(participant);
                        }
                        workshopElement.Add(participants);
                        locationElement.Add(workshopElement);
                }

                root.Add(locationElement);
            }
            root.Save("../../../results/workshops-by-location.xml");
        }

        private static void PhotographersWithSameCameraMake(PhotographyContext context)
        {
            var photographers = context.Photographers
                .Where(p => p.SecondaryCamera.Make == p.PrimaryCamera.Make)
                .Select(p=>new
            {
                FullName = p.FirstName+" "+p.LastName,
                PrimaryCamera = p.PrimaryCamera.Make+" "+p.PrimaryCamera.Model,
                Lenses = p.Lenses
            });
            XElement root = new XElement("photographers");
            foreach (var photographer in photographers)
            {
                XElement photographerElement = new XElement("photographer");
                photographerElement.SetAttributeValue("name",photographer.FullName);
                photographerElement.SetAttributeValue("primary-camera",photographer.PrimaryCamera);
                if (photographer.Lenses.Count > 0)
                {
                    XElement lenses = new XElement("lenses");
                    foreach (var lense in photographer.Lenses)
                    {
                        XElement lens = new XElement("lens");
                        lens.SetValue($"{lense.Make} {lense.FocalLength}mm f{lense.MaxAperture}");
                        lenses.Add(lens);
                    }
                    photographerElement.Add(lenses);
                }
                root.Add(photographerElement);
            }
            root.Save("../../../results/same-cameras-photographers.xml");
        }
    }
}
