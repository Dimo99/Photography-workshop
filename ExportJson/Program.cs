using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Photography.Data;
using Photography.Models;

namespace ExportJson
{
    class Program
    {
        static void Main()
        {
            PhotographyContext context = new PhotographyContext();
            OrderedPhotographers(context);
            LandScapePhotographers(context);
        }

        private static void LandScapePhotographers(PhotographyContext context)
        {
            var photographers =
                context.Photographers.Where(p => p.PrimaryCamera is DSLRCamera && !p.Lenses.Any(l => l.FocalLength > 30)).Select(ph=>new
                {
                    FirstName = ph.FirstName,
                    LastName = ph.LastName,
                    CameraMake = ph.PrimaryCamera.Make,
                    LensesCount = ph.Lenses.Count
                });
            string json = JsonConvert.SerializeObject(photographers, Formatting.Indented);
            File.WriteAllText("../../../results/landscape-photographers.json",json);
        }

        private static void OrderedPhotographers(PhotographyContext context)
        {
            var photographers = context.Photographers.Where(p=>true).OrderBy(p => p.FirstName).ThenByDescending(p => p.LastName).Select(ph=>new 
            {
                FirstName = ph.FirstName,
                LastName = ph.LastName,
                Phone = ph.Phone
            });
            string json = JsonConvert.SerializeObject(photographers, Formatting.Indented);
            File.WriteAllText("../../../results/photographers-ordered.json", json);
        }
    }
}
