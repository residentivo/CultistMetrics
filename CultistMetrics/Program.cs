﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using CultistMetrics.GenericModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace CultistMetrics
{
    class Program
    {
        //ShortCut

        private static UnitOfWork<CultistContext> GetUnitOfWork()
        {
            return new UnitOfWork<CultistContext>(new CultistContext(Configuration));
        }

        //private static void RollbackTransaction()
        //{
        //    if (transInstance != null)
        //        transInstance.Rollback();
        //}

        private static IRepository<T> Repository<T>(IUnitOfWork unitOfWork) where T : class { return unitOfWork.GetRepository<T>(); }

        public static List<Task> AllTasks = new List<Task>();
        //public static ConcurrentBag<Task> AllTasks = new ConcurrentBag<Task>();
        //public static ConcurrentBag<Task> AllConcluidedTasks = new ConcurrentBag<Task>();

        public static string SaveFolder = "Saves";
        public static IConfiguration Configuration { get; set; }

        private static void ProcessFileTest()
        {

            //Saves\20180610_114639_1164save.txt
            //var jsonex = File.ReadAllText(@"Saves\20180610_114639_1164save.txt").Replace("NULL", "\"NULL\"");

            //dynamic jsonFile = JObject.Parse(jsonex);

            //UnitOfWork.SaveFileRepository.Add();

            ////dynamic elementStacks = jsonFile.elementStacks;
            //var elementStacks = jsonFile["elementStacks"];

            //foreach (JProperty property in elementStacks.Properties())
            //{
            //    Console.WriteLine(property.Name + " -> ");

            //    foreach (JProperty item in property.Value)
            //    {
            //        Console.WriteLine(item.Name + " - " + item.Value);
            //    }
            //}

            //Console.WriteLine(elementStacks);
        }
        private static void VerifyFile(SaveFile fileItem)
        {
            //Clearwindow
            ResetColor(true);
            Console.SetCursorPosition(0, 1);
            for (int i = 0; i < 15; i++) Console.WriteLine("                                                                     ");
            
            //Hook Verbs

            int topItem = 2;

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
                //Writexy(state, topItem, 7);
                //Writexy(verbId, topItem, 18);
                Writexy(verbId, topItem, 7);

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

            ResetColor(true);
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
            Console.SetCursorPosition(0, 0);
            Console.Write("                                                                                                     ");
            Console.SetCursorPosition(0, 0);
            Console.Write($"[{DateTime.Now.ToString("HH:mm:ss")}] { message }");
        }

        private static async void QueueFile(string FileNames, bool UseVerifyFile = false)
        {
            var unitOfWork = GetUnitOfWork();

            Logger($"File {FileNames} for process!");

            string filename = Path.GetFileName(FileNames);

            IPagedList<SaveFile> listFile = Repository<SaveFile>(unitOfWork).GetPagedListAsync(sf => sf.FileName == filename, null, null, 0, 20, true).Result;

            if (listFile.TotalCount > 0)
            {
                Logger("File alread listed for process");
                return;
            }

            await Repository<SaveFile>(unitOfWork).InsertAsync(new SaveFile()
            {
                FileName = filename,
                FileTime = long.TryParse(filename.Split("_")[0], out long timeInTick) ? new DateTime(timeInTick) : DateTime.Now,
                Processed = (int)SaveFileProcessedEnum.NotProcessed
            });

            Logger($"File {FileNames} created !");

            await unitOfWork.SaveChangesAsync();

            QueueItem(FileNames, UseVerifyFile);
            //AllTasks.Add(Task.Factory.StartNew(() =>  QueueItem(FileNames)));
        }



        private static async void QueueItem(string FileNames, bool UseVerifyFile = false)
        {
            var _unitOfWork = GetUnitOfWork();

            IDbContextTransaction transaction = null;

            string filename = Path.GetFileName(FileNames);

            Logger($"File {filename} for parse!");

            SaveFile fileItem = Repository<SaveFile>(_unitOfWork).GetFirstOrDefaultAsync(sf => sf.FileName == filename, null, null, false).Result;

            if (fileItem == null)
            {
                Logger($" File {FileNames} not exists on database.");
                return;
            }
            try
            {
                transaction = _unitOfWork.DbContext.Database.BeginTransaction();

                var jsonex = File.ReadAllText(FileNames).Replace(" NULL", " \"NULL\"");

                dynamic jsonFile = JObject.Parse(jsonex);

                var elementStacks = jsonFile["elementStacks"];

                //Capture instance
                var repElStack = Repository<ElementStack>(_unitOfWork);
                var repElStackItem = Repository<ElementStackItem>(_unitOfWork);

                //BeginTransaction();

                foreach (JProperty property in elementStacks.Properties())
                {
                    ElementStack element = new ElementStack
                    {
                        ElementStackIdentification = property.Name,
                        SaveFile = fileItem
                    };

                    fileItem.ElementStacks.Add(element);

                    await repElStack.InsertAsync(element);

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

                        await repElStackItem.InsertAsync(item);
                    }
                }

                var decks = jsonFile["decks"];

                var repDek = Repository<Deck>(_unitOfWork);
                var repDekItem = Repository<DeckItem>(_unitOfWork);

                foreach (JProperty property in decks.Properties())
                {
                    Deck deck = new Deck()
                    {
                        Name = property.Name,
                        SaveFile = fileItem
                    };

                    fileItem.Decks.Add(deck);
                    //How to identify eliminated cards?
                    await repDek.InsertAsync(deck);

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
                                await repDekItem.InsertAsync(decksubItem);
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
                        //How to identify eliminated cards?
                        await repDekItem.InsertAsync(deckItem);
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
                await Repository<MetaInfo>(_unitOfWork).InsertAsync(meta);

                fileItem.MetaInfo = meta;

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
                        characterDetail.Levers.Add(lever);
                        await Repository<Lever>(_unitOfWork).InsertAsync(lever);
                    }
                }

                fileItem.CharacterDetail = characterDetail;

                await Repository<CharacterDetail>(_unitOfWork).InsertAsync(characterDetail);

                var situations = jsonFile["situations"];

                foreach (JProperty property in situations.Properties())
                {
                    Situation situation = new Situation()
                    {
                        SaveFile = fileItem,
                        SituationIdentification = property.Name
                    };
                    fileItem.Situations.Add(situation);
                    await Repository<Situation>(_unitOfWork).InsertAsync(situation);

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
                                await Repository<OngoingSlotElement>(_unitOfWork).InsertAsync(ongoingSlotElement);

                                foreach (JProperty subpropitem in subprop.Values())
                                {
                                    OngoingSlotElementItem ongoingSlotElementItem = new OngoingSlotElementItem()
                                    {
                                        OngoingSlotElement = ongoingSlotElement,
                                        Key = subpropitem.Name,
                                        Value = subpropitem.Value.ToString(),
                                    };
                                    ongoingSlotElement.OngoingSlotElementItems.Add(ongoingSlotElementItem);
                                    await Repository<OngoingSlotElementItem>(_unitOfWork).InsertAsync(ongoingSlotElementItem);
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
                                    await Repository<SituationOutputNote>(_unitOfWork).InsertAsync(situationOutputNote);
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
                                await Repository<SituationStoredElement>(_unitOfWork).InsertAsync(situationStoredElement);

                                foreach (JProperty subpropitem in subprop.Values())
                                {
                                    SituationStoredElementItem situationStoredElementItem = new SituationStoredElementItem()
                                    {
                                        SituationStoredElement = situationStoredElement,
                                        Key = subpropitem.Name,
                                        Value = subpropitem.Value.ToString(),
                                    };
                                    situationStoredElement.SituationStoredElementItems.Add(situationStoredElementItem);
                                    await Repository<SituationStoredElementItem>(_unitOfWork).InsertAsync(situationStoredElementItem);
                                }
                            }
                            continue;
                        }

                        //startingSlotElements
                        if (sitItem.Name.ToUpper() == "startingSlotElements".ToUpper())
                        {
                            foreach (JProperty subprop in sitItem.Values())
                            {
                                StartingSlotElement startingSlotElement = new StartingSlotElement()
                                {
                                    Situation = situation,
                                    StartingSlotElementIdentification = subprop.Name
                                };

                                situation.StartingSlotElements.Add(startingSlotElement);
                                await Repository<StartingSlotElement>(_unitOfWork).InsertAsync(startingSlotElement);

                                foreach (JProperty subpropitem in subprop.Values())
                                {
                                    StartingSlotElementItem startingSlotElementItem = new StartingSlotElementItem()
                                    {
                                        StartingSlotElement = startingSlotElement,
                                        Key = subpropitem.Name,
                                        Value = subpropitem.Value.ToString(),
                                    };
                                    startingSlotElement.StartingSlotElementItems.Add(startingSlotElementItem);
                                    await Repository<StartingSlotElementItem>(_unitOfWork).InsertAsync(startingSlotElementItem);
                                }
                            }
                            continue;
                        }

                        //situationOutputStacks
                        if (sitItem.Name.ToUpper() == "situationOutputStacks".ToUpper())
                        {
                            foreach (JProperty subprop in sitItem.Values())
                            {
                                SituationOutputStack situationOutputStack = new SituationOutputStack()
                                {
                                    Situation = situation,
                                    SituationOutputStacksIdentification = subprop.Name
                                };
                                situation.SituationOutputStacks.Add(situationOutputStack);
                                await Repository<SituationOutputStack>(_unitOfWork).InsertAsync(situationOutputStack);

                                foreach (JProperty subpropitem in subprop.Values())
                                {
                                    SituationOutputStackItem situationOutputStackItem = new SituationOutputStackItem()
                                    {
                                        SituationOutputStack = situationOutputStack,
                                        Key = subpropitem.Name,
                                        Value = subpropitem.Value.ToString(),
                                    };
                                    situationOutputStack.SituationOutputStackItems.Add(situationOutputStackItem);
                                    await Repository<SituationOutputStackItem>(_unitOfWork).InsertAsync(situationOutputStackItem);
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

                        await Repository<SituationItem>(_unitOfWork).InsertAsync(situationItem);
                    }
                }

                fileItem.Processed = (int)SaveFileProcessedEnum.Processed;

                Repository<SaveFile>(_unitOfWork).Update(fileItem);

                transaction.Commit();

                if (UseVerifyFile)
                    VerifyFile(fileItem);
                //CommitTransaction();
            }
            catch (Exception ex)
            {
                Logger(ex.Message);

                File.WriteAllText(Path.Combine(SaveFolder, $@"log\{filename}.log"), ex.ToString());

                transaction.Rollback();
                //RollbackTransaction();

                fileItem.Processed = (int)SaveFileProcessedEnum.Error;

                Repository<SaveFile>(_unitOfWork).Update(fileItem);
            }

            await _unitOfWork.SaveChangesAsync();

            Logger($"File {filename} parsed");

        }


        static void Main(string[] args)
        {
            //ParseMetaDadosAsync();
            Console.CursorVisible = false;
            //ini config
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            var filedirectory = Configuration["AppConfiguration:CultistPath"];
            var savedirectory = Configuration["AppConfiguration:CultistSave"];

            if (!string.IsNullOrEmpty(savedirectory)) SaveFolder = savedirectory;

            //Queue all files on start, confere if it is loaded or not
            //foreach (var item in Directory.EnumerateFiles(savedirectory, "*save.txt")) QueueFile(item);
            //var alltask = new List<Task>();

            //Parallel.ForEach<string>(Directory.EnumerateFiles(savedirectory, "*save.txt"), s =>  QueueFileAsync(s));

            //foreach (var item in Directory.EnumerateFiles(savedirectory, "*save.txt")) AllTasks.Add(QueueFile(item));

            //Task.WhenAll(AllTasks);

            //while (AllTasks.Count > 0)
            //{
            //    var task = Task.WhenAny(AllTasks);               
            //    AllTasks.Remove(task);
            //    //Console.WriteLine($"Yum! {tasks.Count} left!");
            //}

            //_unitOfWork.SaveChanges();

            //Parallel.ForEach<string>(Directory.EnumerateFiles(savedirectory, "*save.txt"), s => QueueItemAsync(s));
            //AllTasks.Clear();
            Logger("Validando arquivos passados!");

            foreach (var item in Directory.EnumerateFiles(SaveFolder, "*save.txt")) QueueFile(item);

            //foreach (var item in Directory.EnumerateFiles(savedirectory, "*save.txt")) AllTasks.Add(Task.Factory.StartNew(() => QueueFile(item)).ContinueWith((t) => Logger($"File {item} tasked")));

            //while (AllTasks.Count != AllConcluidedTasks.Count)
            //while (AllTasks.Count > 0)
            //{
            //    var task = Task.WhenAny(AllTasks)  ;                
            //    //AllConcluidedTasks.Add(task.Result);
            //    AllTasks = new ConcurrentBag<Task>(AllTasks.Where(t => !t.IsCompleted || t != task.Result));
            //}
            //Task.WaitAll(AllTasks.ToArray());

            //Parallel.ForEach(Directory.EnumerateFiles(savedirectory, "*save.txt"), item => QueueFile(item));



            AllTasks.Clear();

            //Logger("Validando arquivos Parseados!");
            //foreach (var item in Directory.EnumerateFiles(savedirectory, "*save.txt")) AllTasks.Add(Task.Factory.StartNew(() => QueueItem(item)).ContinueWith((t) => Logger($"File {item} tasked")));

            ////while (AllTasks.Count != AllConcluidedTasks.Count)
            //Task.WaitAll(AllTasks.ToArray());

            //_unitOfWork.SaveChanges();

            //AllTasks.Clear();


            //foreach (var item in Directory.EnumerateFiles(savedirectory, "*save.txt")) QueueFile(item);

            //QueueFile(allFiles.First());
            //QueueFile(@"D:\Projetos\CultistMetrics\CultistMetrics\bin\Debug\netcoreapp2.0\Saves\636696983275320378_save.txt");
            //QueueFile(@"Saves\636696983275320378_save.txt");


            //while (Console.Read() != 'q') ;


            //foreach (var item in Directory.EnumerateFiles(savedirectory, "*save.txt")) QueueFile(item);


            //QueueFile(allFiles.First());
            //Console.ReadKey();

            //using (var context = new Model.CultistContext())
            //{
            //    var item = context.SaveFiles.Add(new Model.SaveFile()
            //    {
            //        FileName = "TESTE",
            //        Processed = false
            //    });               
            //}


            //var create = context.Database.CreateExecutionStrategy();

            //Start Look File
            //FileSystemWatcher save_watcher = new System.IO.FileSystemWatcher(@"C:\Users\ivoce\AppData\LocalLow\Weather Factory\Cultist Simulator\")
            FileSystemWatcher save_watcher = new FileSystemWatcher(filedirectory)
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.txt"
            };

            // Add event handlers.
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            save_watcher.Changed += new FileSystemEventHandler(Watcher_Changed);
            //save_watcher.Created += new FileSystemEventHandler(Save_watcher_Created);
            //save_watcher.Deleted += new FileSystemEventHandler(Save_watcher_Deleted);

            // Begin watching.
            save_watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            //Console.WriteLine("Press \'q\' to quit the sample.");
            Logger("Press \'q\' to quit the sample.");
            //while (Console.Read() != 'q')
            //{
            //    //var task = Task.WhenAny(AllTasks);
            //    //Rebuild list with not completed tasks
            //    //AllTasks = new ConcurrentBag<Task>(AllTasks.Where(t => !t.IsCompleted || t != task.Result));
            //}
            Console.ReadKey();
            Logger("Wait for any pending task.");


            //var waitFor = save_watcher.WaitForChanged(WatcherChangeTypes.All);
            //Console.ReadKey();
        }


        private static void Save_watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[DEL][{DateTime.Now.ToShortTimeString()}] { e.Name } ");

        }

        private static void Save_watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[CRE][{DateTime.Now.ToShortTimeString()}] { e.Name } ");
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.Contains("save.txt"))
                return;

            //Console.WriteLine($"[CHA][{DateTime.Now.ToShortTimeString()}] { e.Name } ");


            //var saveFile = Path.Combine(SaveFolder, e.Name);
            var backFile = Path.Combine(SaveFolder, DateTime.Now.Ticks.ToString() + "_" + e.Name);
            //Make Backup
            //if (File.Exists(saveFile))
            //    File.Move(saveFile, backFile);

            //Active File
            //File.Copy(e.FullPath, saveFile);
            File.Copy(e.FullPath, backFile);

            Logger($"File Changed {e.Name} > {backFile} ");

            QueueFile(backFile,true);
            //QueueItem(backFile);
            //AllTasks.Add(QueueFileAsync(backFile));

        }
    }

}
