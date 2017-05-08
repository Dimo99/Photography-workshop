using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using AutoMapper;
using Newtonsoft.Json;
using Photography.Data;
using Photography.Dtos;
using Photography.Models;

namespace ImportingDataFromJson
{
    class Program
    {
        private const string LensesPath = @"../../../datasets/lenses.json";
        private const string CamerasPath = @"../../../datasets/cameras.json";
        private const string PhotographersPath = @"../../../datasets/photographers.json";
        static void Main()
        {
            UnitOfWork unit = new UnitOfWork();
            ConfigureAutoMapping(unit);
            //ImportLens(unit);
            //ImportCameras(unit);
            //ImportPhotographers(unit);
        }
        private static void ConfigureAutoMapping(UnitOfWork unit)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<LensDto, Lens>();
                config.CreateMap<CameraDto, DSLRCamera>();
                config.CreateMap<CameraDto, MirrorlessCamera>();
            });
        }

        private static void ImportPhotographers(UnitOfWork unit)
        {
            Random random = new Random();
            string json = File.ReadAllText(PhotographersPath);
            IEnumerable<PhotographerDto> photographerDtos =
                JsonConvert.DeserializeObject<IEnumerable<PhotographerDto>>(json);
            foreach (var photographerDto in photographerDtos)
            {
                if (photographerDto.FirstName == null || photographerDto.LastName == null)
                {
                    Console.WriteLine("Error. Invalid data provided");
                    continue;
                }
                Photographer photographer = new Photographer
                {
                    PrimaryCamera = unit.Cameras.GetById(random.Next(1, 329)),
                    SecondaryCamera = unit.Cameras.GetById(random.Next(1, 329)),
                    FirstName = photographerDto.FirstName,
                    LastName = photographerDto.LastName,
                    Phone = photographerDto.Phone
                };
                foreach (var lense in photographerDto.Lenses)
                {
                    Lens lens = unit.Lenses.FirstOrDefault(l => lense == l.Id);

                    if (lens==null||lens.CompatibleWith != photographer.PrimaryCamera.Make|| lens.CompatibleWith != photographer.SecondaryCamera.Make)
                    {
                        continue;
                    }
                    photographer.Lenses.Add(lens);
                }
                unit.Photographers.Add(photographer);
                try
                {
                    unit.Save();
                    Console.WriteLine($"Succesfully imported {photographer.FirstName} {photographer.LastName} | Lenses: {photographer.Lenses.Count}");
                }
                catch (DbEntityValidationException ex)
                {
                    unit.Photographers.Delete(photographer);
                    Console.WriteLine("Error. Invalid data provided");
                }
            }
        }

        private static void ImportCameras(UnitOfWork unit)
        {
            string json = File.ReadAllText(CamerasPath);
            IEnumerable<CameraDto> cameraDtos = JsonConvert.DeserializeObject<IEnumerable<CameraDto>>(json);
            foreach (var cameraDto in cameraDtos)
            {
                if (cameraDto.Type == null || cameraDto.Make == null || cameraDto.Model == null || cameraDto.MinIso == null)
                {
                    Console.WriteLine("Error. Invalid data provided");
                    continue;
                }
                Camera camera;
                if (cameraDto.Type == "DSLR")
                {
                     camera= Mapper.Map<DSLRCamera>(cameraDto);
                }
                else
                {
                    camera = Mapper.Map<MirrorlessCamera>(cameraDto);
                }
                unit.Cameras.Add(camera);
                unit.Save();
                Console.WriteLine($"Successfully imported {cameraDto.Type} {camera.Make} {camera.Model}");
            }
        }
        private static void ImportLens(UnitOfWork unit)
        {
            string json = File.ReadAllText(LensesPath);
            IEnumerable<LensDto> lensDtos = JsonConvert.DeserializeObject<IEnumerable<LensDto>>(json);
            foreach (var lensDto in lensDtos)
            {
                Lens lens = Mapper.Map<Lens>(lensDto);
                unit.Lenses.Add(lens);
                unit.Save();
                Console.WriteLine($"Successfully imported {lens.Make} {lens.FocalLength}mm f{lens.MaxAperture}");
            }
        }
    }
}
