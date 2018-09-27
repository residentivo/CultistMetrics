using CultistMetrics.GenericModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

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

            ParseFile(@"D:/Projetos/CultistMetrics/Saves/636712351935289474_save.txt");

            //D:/Projetos/CultistMetrics/Saves/636701346135041605_save.txt

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

            string jsonex = File.ReadAllText(filename).Replace(" NULL", " \"NULL\"");

            dynamic jsonFile = JObject.Parse(jsonex);

            var elementStacks = jsonFile["elementStacks"];

            foreach (JProperty property in elementStacks.Properties())
            {
                ElementStack element = new ElementStack
                {
                    ElementStackIdentification = property.Name,
                    SaveFile = fileItem
                };

                fileItem.ElementStacks.Add(element);

                //var elementStackItems = JObject.Parse(property.Values);
                foreach (JProperty subprop in property.Values())
                {
                    ElementStackItem item = new ElementStackItem
                    {
                        Value = subprop.Value.ToString(),
                        Key = subprop.Name,
                        ElementStack = element
                    };

                    element.ElementStackItems.Add(item);
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

                fileItem.Decks.Add(deck);
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
                            deck.DeckItems.Add(decksubItem);
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
                    deck.DeckItems.Add(deckItem);
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

            fileItem.MetaInfo = meta;

            var characterDetails = jsonFile["characterDetails"];
            CharacterDetail characterDetail = new CharacterDetail()
            {
                SaveFile = fileItem,
                Levers = new List<Lever>()
            };

            fileItem.CharacterDetail = characterDetail;

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
                    characterDetail.Levers.Add(lever);
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

                fileItem.Situations.Add(situation);

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

                            situation.OngoingSlotElements.Add(ongoingSlotElement);

                            foreach (JProperty subpropitem in subprop.Values())
                            {
                                OngoingSlotElementItem ongoingSlotElementItem = new OngoingSlotElementItem()
                                {
                                    OngoingSlotElement = ongoingSlotElement,
                                    Key = subpropitem.Name,
                                    Value = subpropitem.Value.ToString(),
                                };

                                ongoingSlotElement.OngoingSlotElementItems.Add(ongoingSlotElementItem);
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

                                situation.SituationOutputNotes.Add(situationOutputNote);
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
                            situation.SituationStoredElements.Add(situationStoredElement);

                            foreach (JProperty subpropitem in subprop.Values())
                            {
                                SituationStoredElementItem situationStoredElementItem = new SituationStoredElementItem()
                                {
                                    SituationStoredElement = situationStoredElement,
                                    Key = subpropitem.Name,
                                    Value = subpropitem.Value.ToString(),
                                };

                                situationStoredElement.SituationStoredElementItems.Add(situationStoredElementItem);
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

                    situation.SituationItems.Add(situationItem);
                }
            }

            //Verify some properties:
            VerifyFile(fileItem);
        }

        private static void VerifyFile(SaveFile fileItem)
        {
            //Hook Verbs
            Console.SetCursorPosition(0, 1);
            Console.CursorVisible = false;

            int topItem = 1;

            Situation VisionExtraAlert = null;

            foreach (var item in fileItem.Situations
                .OrderBy(s => FindSituationItem(s, "state"))
                .ThenBy(s => TryParseTimeRemanining(FindSituationItem(s, "timeRemaining")))
                )
            {
                //Find name                
                var verbId = FindSituationItem(item, "verbId");
                var timeRemaining = FindSituationItem(item, "timeRemaining");
                var state = FindSituationItem(item, "state");

                decimal timeRem = 0;
                if (decimal.TryParse(timeRemaining, out timeRem) && state.ToLower() == "ongoing")
                {
                    if (timeRem > 5 && timeRem <= 10)
                        SetColor(ConsoleColor.Yellow);
                    if (timeRem <= 5)
                        SetColor(ConsoleColor.Red);
                }
                if (state.ToLower() == "unstarted")
                {
                    SetColor(ConsoleColor.Green);
                }

                if (ValidateVision(verbId, item.SituationStoredElements))
                {
                    VisionExtraAlert = item;
                }

                if (timeRemaining.Length > 5)
                    timeRemaining = timeRemaining.Remove(5);

                Writexy(timeRemaining, topItem);
                Writexy(state, topItem, 7);
                Writexy(verbId, topItem, 18);

                //Console.WriteLine($"[{verbId.Value}     ]{title.Value}");
                topItem++;
                ResetColor();
            }

            //Show extra alerts
            Writexy("", topItem++);
            if (VisionExtraAlert != null)
            {
                byte VisionCount = 0;
                foreach (var item in VisionExtraAlert.SituationStoredElements)
                {
                    foreach (var subitem in item.SituationStoredElementItems)
                    {
                        if (subitem.Key.ToLower() == "elementId".ToLower() && subitem.Value.ToLower() == "fascination".ToLower()) VisionCount++;
                    }
                }
                switch (VisionCount)
                {
                    case 0:
                        SetColor(ConsoleColor.Green);
                        break;
                    case 1:
                        SetColor(ConsoleColor.Yellow);
                        break;
                    case 2:
                        SetColor(ConsoleColor.Red);
                        break;
                    case 3:
                        SetColor(ConsoleColor.Magenta);
                        break;
                    default:
                        SetColor(ConsoleColor.Blue);
                        break;
                }
                Writexy($"FASCINATION WARNING: {VisionCount}", topItem);

            }
        }
        private static decimal TryParseTimeRemanining(string timeRemaining)
        {
            decimal timeRem = 0;
            decimal.TryParse(timeRemaining, out timeRem);
            return timeRem;
        }
        private static bool ValidateVision(string verbid, List<SituationStoredElement> SituationStoredElements)
        {
            if (string.IsNullOrEmpty(verbid) || verbid.ToLower() != "visions".ToLower())
                return false;

            //situationStoredElements
            return true;
        }

        private static void Writexy(string texto, int top = 0, int left = 0)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(texto);
        }

        static ConsoleColor _back = ConsoleColor.Black;
        static ConsoleColor _fore = ConsoleColor.Gray;

        private static void SetColor(ConsoleColor fore, ConsoleColor back = ConsoleColor.Black)
        {
            _back = Console.BackgroundColor;
            _fore = Console.ForegroundColor;

            Console.BackgroundColor = back;
            Console.ForegroundColor = fore;
        }

        private static void ResetColor(bool resetDefault = false)
        {
            if (resetDefault)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
            Console.BackgroundColor = _back;
            Console.ForegroundColor = _fore;
        }
        private static string FindSituationItem(Situation item, string key)
        {
            SituationItem situationItem = item.SituationItems.FirstOrDefault(si => si.Key.ToLower() == key.ToLower());

            if (situationItem != null)
                return situationItem.Value;

            return string.Empty;
        }

        private static void Logger(string message)
        {
            //remove last line            
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] { message}");
        }
        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.Contains("save.txt"))
                return;

            //var backFile = Path.Combine(SaveFolder, DateTime.Now.Ticks.ToString() + "_" + e.Name);

            //File.Copy(e.FullPath, backFile);

            //Logger($"File Changed {e.Name} > {backFile} ");

        }
    }
}
