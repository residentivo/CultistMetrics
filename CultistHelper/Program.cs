using CultistMetrics.GenericModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace CultistHelper
{
    class Program
    {
        //private static SaveFile fileItem;

        private static UnitOfWork<CultistContext> GetUnitOfWork()
        {
            return new UnitOfWork<CultistContext>(new CultistContext());
        }

        private static IRepository<T> Repository<T>(IUnitOfWork unitOfWork) where T : class { return unitOfWork.GetRepository<T>(); }

        public static IConfigurationRoot Configuration { get; private set; }

        private static string filedirectory;
        private static string SaveFolder;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            filedirectory = Configuration["AppConfiguration:CultistPath"];
            SaveFolder = Configuration["AppConfiguration:CultistSave"];

            Logger("TESTE de LOG");

            

            Console.ReadKey();

        }

        private static void BasciOperation()
        {

            //Parse File for first time
            ParseFile(Path.Combine(filedirectory, "save.txt"));

            FileSystemWatcher save_watcher = new FileSystemWatcher(filedirectory)
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.txt"
            };
            save_watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            save_watcher.EnableRaisingEvents = true;
            Logger("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') { }
        }
        private static void ParseFile(string filename)
        {
            SaveFile fileItem;
            fileItem = new SaveFile()
            {
                FileName = filename,
                FileTime = long.TryParse(filename.Split("_")[0], out long timeInTick) ? new DateTime(timeInTick) : DateTime.Now,
                Processed = (int)SaveFileProcessedEnum.NotProcessed
            };

            var jsonex = File.ReadAllText(filename).Replace(" NULL", " \"NULL\"");

            dynamic jsonFile = JObject.Parse(jsonex);

            var elementStacks = jsonFile["elementStacks"];

            foreach (JProperty property in elementStacks.Properties())
            {
                ElementStack element = new ElementStack
                {
                    ElementStackIdentification = property.Name,
                    SaveFile = fileItem
                };

                //var elementStackItems = JObject.Parse(property.Values);
                foreach (JProperty subprop in property.Values())
                {
                    ElementStackItem item = new ElementStackItem
                    {
                        Value = subprop.Value.ToString(),
                        Key = subprop.Name,
                        ElementStack = element
                    };

                }
            }

            var decks = jsonFile["decks"];

            foreach (JProperty property in decks.Properties())
            {
                Deck deck = new Deck()
                {
                    Name = property.Name,
                    SaveFile = fileItem
                };

                //How to identify eliminated cards?

                foreach (JProperty subprop in property.Values())
                {
                    if (subprop.Name == "eliminatedCards")
                    {
                        //Aqui eu tenho uma sub parte                            
                        foreach (JToken subsubprop in subprop.Values())
                        {
                            DeckItem decksubItem = new DeckItem()
                            {
                                Eliminated = true,
                                Value = subsubprop.Value<string>(),
                                Key = "0",
                                Deck = deck
                            };

                        }
                        continue;
                    }

                    DeckItem deckItem = new DeckItem()
                    {
                        Eliminated = false,
                        Key = subprop.Name,
                        Value = subprop.Value.ToString(),
                        Deck = deck
                    };
                    //How to identify eliminated cards?
                }
            }

            var metainfo = jsonFile["metainfo"];

            MetaInfo meta = new MetaInfo();

            foreach (JProperty property in metainfo.Properties())
            {
                if (property.Name.ToUpper() == "BirdWormSlider".ToUpper())
                    meta.BirdWormSlider = int.Parse(property.Value.ToString());
                if (property.Name.ToUpper() == "VERSIONNUMBER".ToUpper())
                    meta.Version = property.Value.ToString();
                if (property.Name.ToUpper() == "WeAwaitSTE".ToUpper())
                    meta.WeAwaitSTE = property.Value.ToString();
            }

            var characterDetails = jsonFile["characterDetails"];
            CharacterDetail characterDetail = new CharacterDetail()
            {
                SaveFile = fileItem,
                Levers = new List<Lever>()
            };

            foreach (JProperty property in characterDetails.Properties())
            {
                if (property.Name.ToUpper() == "activeLegacy".ToUpper())
                {
                    characterDetail.ActiveLegacy = property.Value.HasValues ? property.Value.ToString() : string.Empty;
                    continue;
                }
                if (property.Name.ToUpper() == "name".ToUpper())
                {
                    characterDetail.Name = property.Value.ToString();
                    continue;
                }
                if (property.Name.ToUpper() == "profession".ToUpper())
                {
                    characterDetail.Profession = property.Value.ToString();
                    continue;
                }
                LeverPartEnum leverPart = LeverPartEnum.Error;

                if (property.Name.ToUpper() == "pastLevers".ToUpper())
                    leverPart = LeverPartEnum.PastLevers;

                if (property.Name.ToUpper() == "futureLevers".ToUpper())
                    leverPart = LeverPartEnum.FutureLevers;

                if (property.Name.ToUpper() == "executions".ToUpper())
                    leverPart = LeverPartEnum.Executions;

                foreach (JProperty subprop in property.Values())
                {
                    Lever lever = new Lever()
                    {
                        CharacterDetail = characterDetail,
                        Part = (int)leverPart,
                        Key = subprop.Name,
                        Value = subprop.Value.ToString()
                    };


                }
            }

            var situations = jsonFile["situations"];

            foreach (JProperty property in situations.Properties())
            {
                Situation situation = new Situation()
                {
                    SaveFile = fileItem,
                    SituationIdentification = property.Name
                };

                foreach (JProperty sitItem in property.Values())
                {
                    //ongoingSlotElements
                    if (sitItem.Name.ToUpper() == "ongoingSlotElements".ToUpper())
                    {
                        foreach (JProperty subprop in sitItem.Values())
                        {
                            OngoingSlotElement ongoingSlotElement = new OngoingSlotElement()
                            {
                                Situation = situation,
                                OngoingSlotElementIdentification = subprop.Name
                            };

                            foreach (JProperty subpropitem in subprop.Values())
                            {
                                OngoingSlotElementItem ongoingSlotElementItem = new OngoingSlotElementItem()
                                {
                                    OngoingSlotElement = ongoingSlotElement,
                                    Key = subpropitem.Name,
                                    Value = subpropitem.Value.ToString(),
                                };


                            }
                        }
                        continue;
                    }

                    //situationOutputNotes
                    if (sitItem.Name.ToUpper() == "situationOutputNotes".ToUpper())
                    {
                        foreach (JProperty subprop in sitItem.Values())
                        {
                            string indexValue = subprop.Name;
                            foreach (JProperty subpropitem in subprop.Values())
                            {
                                SituationOutputNote situationOutputNote = new SituationOutputNote()
                                {
                                    Situation = situation,
                                    Index = indexValue,
                                    Key = subpropitem.Name,
                                    Value = subpropitem.Value.ToString(),
                                };


                            }
                        }
                        continue;
                    }

                    //situationStoredElements
                    if (sitItem.Name.ToUpper() == "situationStoredElements".ToUpper())
                    {
                        foreach (JProperty subprop in sitItem.Values())
                        {
                            SituationStoredElement situationStoredElement = new SituationStoredElement()
                            {
                                Situation = situation,
                                SituationStoredElementIdentification = subprop.Name
                            };

                            foreach (JProperty subpropitem in subprop.Values())
                            {
                                SituationStoredElementItem situationStoredElementItem = new SituationStoredElementItem()
                                {
                                    SituationStoredElement = situationStoredElement,
                                    Key = subpropitem.Name,
                                    Value = subpropitem.Value.ToString(),
                                };


                            }
                        }
                        continue;
                    }

                    SituationItem situationItem = new SituationItem()
                    {
                        Key = sitItem.Name,
                        Value = sitItem.Value.ToString(),
                        Situation = situation
                    };


                }
            }



        }
        private static void Logger(string message)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] { message}");
        }
        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.Contains("save.txt"))
                return;

            var backFile = Path.Combine(SaveFolder, DateTime.Now.Ticks.ToString() + "_" + e.Name);

            File.Copy(e.FullPath, backFile);

            Logger($"File Changed {e.Name} > {backFile} ");

        }
    }
}
