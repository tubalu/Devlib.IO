﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Samples
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing.Design;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Security.Cryptography;
    using System.ServiceModel;
    using System.ServiceModel.Routing;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Linq;
    using DevLib.AddIn;
    using DevLib.Compression;
    using DevLib.Configuration;
    using DevLib.Csv;
    using DevLib.DaemonProcess;
    using DevLib.Data;
    using DevLib.Data.Repository;
    using DevLib.DesignPatterns;
    using DevLib.Diagnostics;
    using DevLib.DirectoryServices;
    using DevLib.Dynamic;
    using DevLib.Expressions;
    using DevLib.ExtensionMethods;
    using DevLib.ExtensionProperties;
    using DevLib.Input;
    using DevLib.IO.Ports;
    using DevLib.Ioc;
    using DevLib.Logging;
    using DevLib.Logging.Viewer;
    using DevLib.Net;
    using DevLib.Net.Ftp;
    using DevLib.Net.Sockets;
    using DevLib.Parameters;
    using DevLib.Reflection;
    using DevLib.Remoting;
    using DevLib.Serialization;
    using DevLib.ServiceBus;
    using DevLib.ServiceModel;
    using DevLib.ServiceModel.Extensions;
    using DevLib.ServiceProcess;
    using DevLib.TerminalServices;
    using DevLib.Text;
    using DevLib.Timers;
    using DevLib.Web;
    using DevLib.Web.Hosting.WebHost20;
    using DevLib.Web.Hosting.WebHost40;
    using DevLib.Web.Services;
    using DevLib.WinForms;
    using DevLib.Xml;

    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            InternalLogger.Log("Begin InternalLogger");
            LogManager.Open().Log("Begin LogManager");

            Benchmark.Run(delegate
            {
                PrintStartInfo();

                var result = Benchmark.Run(i =>
                {
                    //TestCodeSnippets();
                });

                Benchmark.Run(i =>
                {
                    //TestCsv();
                });

                Benchmark.Run(i =>
                {
                    //TestData();
                });

                Benchmark.Run(i =>
                {
                    //TestDynamic();
                });

                Benchmark.Run(i =>
                {
                    //TestXml();
                });

                Benchmark.Run(i =>
                {
                    //TestReflection();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibAddIn();
                });

                Benchmark.Run(i =>
                {
                    //TestCompression();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibDaemonProcess();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibDesignPatterns();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibDiagnostics();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibExtensionMethods();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibExtensionProperties();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibIoc();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibLogging();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibNet();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibUtilities();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibServiceBus();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibServiceModel();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibServiceProcess(args);
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibTerminalServices();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibTimer();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibWeb();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibWebHosting();
                });

                Benchmark.Run(delegate
                {
                    //TestDevLibConfiguration();
                });

                Benchmark.Run(delegate
                {
                    TestDevLibWinForms();
                    //new ThreadStart(() => { TestDevLibWinForms(); }).BeginInvoke((asyncResult) => { Console.WriteLine("WinForm exit..."); }, null);
                });

                Benchmark.Run(delegate
                {
                    //Application.Run(new LoggingViewerMainForm());
                });

                PrintExitInfo();
            }, "DevLib.Samples", 1);

            InternalLogger.Log("End");
        }

        private static void TestXml()
        {
            var fluentXml = FluentXml.Load("TestData.xml");

            var a = fluentXml.NodePathValue<bool>("A.B.C[D]");
        }

        private static void TestDevLibWeb()
        {
            HttpWebClient client = new HttpWebClient("http://www.codeplex.com");
            var msg = client.SendRequest();
        }

        //[Serializable]
        public class BrokeredMessageTest
        {
            public int MyProperty { get; set; }

            public string MyProperty2 { get; set; }
        }

        private static void TestDevLibServiceBus()
        {

            var m1 = new BrokeredMessage(new BrokeredMessageTest());

            var t1 = ServiceBusManager.GetOrCreateTopic("t1");
            var pub1 = ServiceBusManager.GetOrCreatePublisher("pub1");
            var sub1 = ServiceBusManager.GetOrCreateSubscription("sub1");

            t1.AddPublisher("pub1").AddSubscription("sub1");

            sub1.OnMessage(msg =>
            {
                Console.WriteLine(msg);
                Console.WriteLine(msg.GetBody());
                //Console.WriteLine(msg.GetBody<string>());
                Console.WriteLine(msg.GetBody<BrokeredMessageTest>());
                if (!msg.IsReturned) msg.Return();
            });

            //pub1.Send(new BrokeredMessage("Hello1"));
            pub1.Send(new BrokeredMessage(new BrokeredMessageTest { MyProperty = 2, MyProperty2 = "aa" }));

            Console.ReadLine();
        }

        private static void TestDevLibWebHosting()
        {
            DevLib.Web.Hosting.WebHost20.WebServer ws = new Web.Hosting.WebHost20.WebServer(1, "test", 80, true);

            ws.Start();
        }

        private static void TestData()
        {
            //XmlFileRepository<FooBar> data = new XmlFileRepository<FooBar>(@"d:\work\temp\repo.xml");

            //data.Add(new FooBar());
            //data.Add(new FooBar() { bar = 2 });

            Benchmark.Run(i => GuidGenerator.NewGuidSequentialAtEnd(), "NewGuidSequentialAtEnd", 10);

            var a = GuidGenerator.NewGuidSequentialString();
            var b = GuidGenerator.NewGuidSequentialAtEnd();

            var c = GuidGenerator.GetTimestampFromGuidSequentialString(a);
            var d = GuidGenerator.GetTimestampFromGuidSequentialAtEnd(b);

            Console.ReadLine();
        }

        private static void TestCsv()
        {
            CsvDocument csv = new CsvDocument();
            csv.Load(@"d:\test.csv", true);

            foreach (var item in CsvDocument.Open(@"d:\test.csv"))
            {
                Console.WriteLine(item[0]);
            }

            Console.ReadLine();
            //DataTable dataTable = csv.Table;
            //csv.Table.Rows.RemoveAt(1);
            //csv.Table.Columns.Add("NewColumn");
            //csv.Table.Columns.Add("b");
            //csv.Table.Columns.Add("c");
            //csv[0,0] = "hello";
            //string a = csv[0,0];
            //List<string> headers = csv.ColumnNames;
            //int rowCount = csv.RowCount;
            //int columnCount = csv.ColumnCount;
            //csv[0, 0] = "hello";
            //csv.Save(@"c:\new.csv", true, false, true, false, ',', '"', Environment.NewLine);

            //string cellAtRow0Column1 = csv[0, 1];
            //string cellAtRow0ColumnNameA = csv[0, "A"];
            //DataRow row2 = csv[0];
            //DataColumn columnA = csv["A"];


        }

        private static void TestDynamic()
        {
            var json = DynamicJson.Parse(@"{""name"":""json"", ""age"":23, ""nest"":{ ""foobar"":true } }");

            string p1 = json.name; // "json" - dynamic(string)
            int p2 = json.age; // 23 - dynamic(double)
            bool p3 = json.nest.foobar; // true - dynamic(bool)
            bool p4 = json["nest"]["foobar"]; // can access string indexer
            bool p5 = json[2][0]; // can access int indexer

            var p6 = json.Has("name");// true
            var p7 = json.Has("address"); // false
            var p8 = (Dictionary<DynamicJson, DynamicJson>)json;
            json.Arr = new string[] { "ABC", "DEF" }; // Add Array
            json.Obj1 = new { }; // Add Object
            json.address = new { postcode = "abc120", street = "def" }; // Add and Init

            json.Remove("age");
            json.Arr.Remove(0);

            json.Obj1 = 5000; // use 5000 to replace new { }

            string jsonString = json.ToString();


            // DynamicJson - (IsArray)
            var arrayJson = DynamicJson.Parse(@"[1,10,200,300]");
            arrayJson[9] = 600;
            foreach (int item in arrayJson)
            {
                Console.WriteLine(item); // 1, 10, 200, 300, 600
            }

            // DynamicJson - (IsObject)
            var objectJson = DynamicJson.Parse(@"{""foo"":""json"",""bar"":100}");
            foreach (KeyValuePair<string, dynamic> item in objectJson)
            {
                Console.WriteLine(item.Key + ":" + (string)item.Value); // foo:json, bar:100
            }


            var arrayJson2 = DynamicJson.Parse(@"[1,10,200,300]");

            var array1 = (string[])json; // string[] {"1", "10", "200", "300"}
            var list1 = (List<int>)arrayJson2; // List<int> {1, 10, 200, 300}

            var objectJson2 = DynamicJson.Parse(@"{""foo"":""json"",""bar"":100}");
            var foobar1 = (FooBar)objectJson2; // Deserialize to FooBar

            var objectJson3 = DynamicJson.LoadFrom(new FooBar { foo = "你好", bar = 10 });
            string jsonString1 = objectJson3.ToString(); // Serialize to json string

            // with linq
            var objectJsonList = DynamicJson.Parse(@"[{""bar"":50},{""bar"":100}]");
            var barSum = ((FooBar[])objectJsonList).Select(fb => fb.bar).Sum(); // 150
            var dynamicWithLinq = ((IEnumerable<dynamic>)objectJsonList).Select(d => (int)d.bar).ToList();


            var dynamicXml = DynamicXml.Parse("<FooBar name=\"foobar1\" ><foo>xml</foo><bar size=\"456\">123<aaa>xml2</aaa></bar></FooBar>");
            dynamicXml.foo[0] = 1;
            var b1 = dynamicXml.bar[0].aaa;

            string x1 = dynamicXml.foo; // element "xml" - dynamic(string)
            var x2 = dynamicXml.bar; // element 123 - dynamic(int)
            string x4 = dynamicXml["name"]; // attribute "foobar1"
            string x5 = dynamicXml[0, 0]; // first element "xml"
            int x6 = dynamicXml[0, 1]["size"]; // second element's attribute 456

            var x7 = dynamicXml.HasElement("foo");// element true
            var x8 = dynamicXml.HasElement("foooo"); // element false
            var x9 = dynamicXml.HasAttribute("name");// attribute true
            var x10 = dynamicXml.HasAttribute("size"); // attribute false
            var x11 = dynamicXml.bar.HasAttribute("size"); // attribute true

            dynamicXml.Date = DateTime.Now; // add new element
            dynamicXml["age"] = 10; // add new attribute
            dynamicXml[9, 2] = "hello"; // add new element, element name is value's Type
            var atts = dynamicXml.Attributes(); // get all attributes
            var elms = dynamicXml.Elements(); // get all elements
            //dynamicXml.RemoveElement("bar"); //remove element
            //dynamicXml.RemoveElement(0); //remove first element
            //dynamicXml.RemoveAttribute("name"); //remove attribute
            //dynamicXml.RemoveAttribute(0); //remove first attribute

            dynamicXml.Date = 2001; // update element with another type

            var dynamicXml1 = DynamicXml.Parse("<FooBar name=\"foobar1\" ><foo>xml</foo><bar size=\"456\">123</bar></FooBar>");
            foreach (KeyValuePair<string, dynamic> item in dynamicXml1)
            {
                Console.WriteLine(item.Key + ":" + (string)item.Value); //name:foobar1, foo:xml, bar:123
            }

            var dynamicXml2 = DynamicXml.Parse("<FooBar><foo>xml</foo><bar>123</bar></FooBar>");
            var foobar2 = (FooBar)dynamicXml2;

            var dynamicXml4 = DynamicXml.LoadFrom(new List<FooBar> { new FooBar { foo = "foo1", bar = 10 }, new FooBar { foo = "foo2", bar = 20 } });
            var barSum1 = ((FooBar[])dynamicXml4).Select(fb => fb.bar).Sum(); //30
            var fooList = ((IEnumerable<dynamic>)dynamicXml4).Select(p => (string)p.foo).ToList();


            Console.WriteLine("done");

        }

        private static void TestDevLibTimer()
        {
            var idleTimer = new IdleTimer(3000, true, false);
            idleTimer.IdleOccurred += new EventHandler(idleTimer_IdleOccurred);
        }

        static void idleTimer_IdleOccurred(object sender, EventArgs e)
        {
            Console.WriteLine("idle");
        }

        private static void TestDevLibLogging()
        {

            for (int j = 0; j < 20; j++)
            {
                Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        Thread.Sleep(500);
                        LogManager.Open("000.log", "testlogconfig.xml").Log(i, "一二三");

                        LogManager.Open("000.log", "testlogconfig.xml").Log(i, "The quick brown fox jumps over the lazy dog.");
                    }
                });
            }

            Console.ReadLine();

            //var logger = LogManager.Open("d:\\Work\\temp\\000.log");

            //Benchmark.Run(index =>
            //{
            //    for (int i = 0; i < 10000000; i++)
            //    {
            //        //Thread.Sleep(10);
            //        LogManager.Open("d:\\Work\\temp\\000.log").Log(i, "The quick brown fox jumps over the lazy dog.");
            //    }
            //});

            //Console.ReadLine();

            //Logger logger = LogManager.Open();
            //logger.Log("This is a log message.");

            //Random random = new Random();

            //LogManager.Open(@"e:\1\test.log", new LoggerSetup { RollingFileCountLimit = 5, RollingFileSizeMBLimit = 1 });
            //int i = 0;
            //while (true)
            //{
            //    i++;
            //    LogManager.Open(@"e:\1\test.log").Log(LogLevel.DBUG, i, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow);
            //    LogManager.Open(@"e:\1\test.log").Log(LogLevel.DBUG, i, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow);
            //    LogManager.Open(@"e:\1\test.log").Log(LogLevel.DBUG, i, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow);
            //    LogManager.Open(@"e:\1\test.log").Log(LogLevel.DBUG, i, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow);
            //    LogManager.Open(@"e:\1\test.log").Log(LogLevel.DBUG, i, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow, DateTime.Now, DateTime.UtcNow);

            //    //Thread.Sleep(random.Next(5, 100));
            //}

            //new LogConfig().WriteXml("a.xml",true);

            //LogManager.OpenConfig("a.xml");

            //var a = LogManager.DefaultLogFile;

            //for (int i = 0; i < 10; i++)
            //{
            //    new Thread(new ThreadStart(() => { LogManager.Open(@"C:\\AAA.log").Log(); })).Start();
            //}

            //LogManager.Open(@"C:\\AAA.log").Log(LogLevel.DBUG, Thread.CurrentThread.ManagedThreadId);
            //LogManager.Open(@"C:\\AAA.log").Log(LogLevel.INFO, Thread.CurrentThread.ManagedThreadId);
            //LogManager.Open(@"C:\\AAA.log").Log(LogLevel.EXCP, Thread.CurrentThread.ManagedThreadId);
            //LogManager.Open(@"C:\\AAA.log").Log(LogLevel.WARN, Thread.CurrentThread.ManagedThreadId);
            //LogManager.Open(@"C:\\AAA.log").Log(LogLevel.ERRO, Thread.CurrentThread.ManagedThreadId);
            //LogManager.Open(@"C:\\AAA.log").Log(LogLevel.FAIL, Thread.CurrentThread.ManagedThreadId);

            //Console.ReadLine();

            //var a = new Uri(Path.GetFullPath(@"C:\a b\b c\c d e\1 3 4.5")).AbsolutePath;

            //LogManager.Open(@"c:\a\b\c\d.log").Log(LogLevel.DBUG, "hello", new Exception());



            //LogManager.DefaultLoggerSetup.RollingByDate = true;

            //while (true)
            //{
            //    LogManager.Open(@"C:\\AAA.log").Log(LogLevel.DBUG, Process.GetCurrentProcess().Id);

            //    Thread.Sleep(random.Next(5, 100));
            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    LogManager.Open(@"C:\\AAA.log").Log(LogLevel.DBUG, i.ToString(), DateTime.Now, Environment.UserName);
            //}

            //Task.Factory.StartNew(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        LogManager.Open().Log(LogLevel.DBUG, i.ToString());
            //    }


            //    Thread.Sleep(10000);

            //    LogManager.Open().Log(LogLevel.DBUG, "b");
            //});
        }

        private static void TestDevLibIoc()
        {
            PrintMethodName("Test DevLib.Ioc");

            IocContainer container = new IocContainer();

            container.RegisterAssembly<IFoo>(false, Assembly.GetEntryAssembly());

            container.RegisterFile<IFoo>(new[] { typeof(Person) }, false, "DevLib.Samples.exe");

            //container.RegisterFile<IFoo>("DevLib.Samples.exe");

            //container.RegisterFile<IFoo>("AClassLibrary1.dll");

            //container.RegisterFile<IFoo>("AClassLibrary1.dll");

            //container.RegisterDirectory<IFoo>(new[] { typeof(Person) }, false, ".");

            //container.RegisterAssembly<IFoo>(new[] { typeof(Person) }, Assembly.GetExecutingAssembly());

            //List<object> list1 = container.GetAllInstances(typeof(IFoo)) as List<object>;

            //list1 = container.GetAllInstances(typeof(IFoo)) as List<object>;

            //var listi0 = (IFoo)list1[0];

            //var list = container.GetAllInstances<IFoo>();

            //container.RegisterAssembly<IFoo>(Assembly.GetExecutingAssembly(), typeof(Person));

            //container.RegisterAssembly<IFoo>(Assembly.GetExecutingAssembly());

            //container.Register<IPerson>(new Person("aaaa", "bbbb", 0));

            var iocReg = container.Register<IPerson>(c => new Person("aaaa", "bbbb", 1));

            //var aPerson = container.GetInstance<IPerson>();
            //aPerson = container.GetInstance<IPerson>();

            container.Register<IPerson>(new Person("aaaa", "bbbb", 2), "A");

            container.Register<IPerson>(new Person("ccccc", "ddddd", 3));

            container.Register<IPerson>(c => new Person(c.Resolve<IPerson>().LastName, c.Resolve<IPerson>().FirstName, 4));
            container.Register<IPerson>(c => new Person(c.Resolve<IPerson>().FirstName, c.Resolve<IPerson>().FirstName, 5));
            //container.Register<IPerson>(c => new Person("eeeee", "fffff", 6));

            //container.Register<Person>(new Person("e", "f", 3));

            var list = container.GetAllInstances<IPerson>();

            Console.WriteLine(container.Resolve<IPerson>());

            //iocReg.Dispose();

            Console.WriteLine(container.Resolve<IPerson>());

            //container.Unregister<IPerson>();

            Console.WriteLine(container.Resolve<IPerson>());
            Console.WriteLine(container.Resolve<IPerson>());

            list = container.GetAllInstances<IPerson>();

            Console.WriteLine(container.Resolve<IPerson>());
            Console.WriteLine(container.Resolve<IPerson>());

            container.Clear();


            container.Dispose();

        }

        private static void TestDevLibDaemonProcess()
        {
            PrintMethodName("Test DevLib.DaemonProcess");

            //var a = DaemonProcess.NativeAPI.NativeMethodsHelper.GetCommandLine(Process.GetCurrentProcess().Id);


            //DaemonProcessManager.StartProtect(Guid.Parse("8C0CD469-3C7B-4F7A-80D1-2987456877AA"), 1, 1, ProcessMode.Service, "AMD External Events Utility");

            //DaemonProcessManager.StartSelfProtect(Guid.Parse("8C0CD469-3C7B-4F7A-80D1-2987456877AA"), ProcessMode.Process, 0, "a", "b c ", "d", "e", " f g ");

            //DaemonProcessHelper.GetCommandLineArguments("\"C:\\addin_DataReceived.exe\" a b c d e \"f g\"");
        }

        private static void TestDevLibTerminalServices()
        {
            PrintMethodName("Test DevLib.TerminalServices");

            //string domainname = string.Empty;

            //SelectQuery query = new SelectQuery("Win32_ComputerSystem");

            //using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            //{
            //    foreach (ManagementObject mo in searcher.Get())
            //    {
            //        if ((bool)mo["partofdomain"])
            //        {
            //            domainname = mo["domain"].ToString();
            //            break;
            //        }
            //    }
            //}

            Console.WriteLine(SystemInformation.UserDomainName);

            foreach (var item in TerminalServicesManager.GetServers(SystemInformation.UserDomainName))
            {
                item.Open();
                Console.WriteLine(item.ServerName);
                item.Dispose();
            }



            //DevLib.TerminalServices.RemoteServerHandle
            foreach (var item in TerminalServicesManager.GetLocalServer().GetSessions())
            {
                Console.WriteLine(@"/--------\");
                try
                {
                    Console.WriteLine(item.MessageBox("Hello", "123456789", false, 5, RemoteMessageBoxButtons.YesNoCancel, RemoteMessageBoxIcon.Exclamation));
                }
                catch
                {
                    //Console.WriteLine(e);
                }
                Console.WriteLine(item.SessionId);
                Console.WriteLine(item.UserName);
                Console.WriteLine(item.WindowStationName);
                Console.WriteLine(item.ConnectState);
                Console.WriteLine(@"\--------/");
            }
        }

        private static void TestCompression()
        {
            PrintMethodName("Test DevLib.Compression");

            //File.Delete("d:\\1.zip");


            //using (FileStream file = File.Create("d:\\1.zip"))
            //using (ZipArchive zip = new ZipArchive(file, ZipArchiveMode.Create, false))
            //{
            //    zip.CreateEntryFromDirectory(@"D:\test\New folder", "New folder" + Path.DirectorySeparatorChar);
            //    zip.CreateEntryFromDirectory(@"D:\test\New folder (2)", "New folder2" + Path.DirectorySeparatorChar);
            //    zip.CreateEntryFromFile("d:\\test\\1.txt", "1.txt");
            //    zip.CreateEntryFromFile("d:\\test\\2.txt", "2.txt");

            //    //using (Stream stream = File.Open("d:\\test\\1.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            //    //{
            //    //    ZipArchiveEntry zipArchiveEntry = zip.CreateEntry("1.txt", FileAttributes.Directory);
            //    //    DateTime lastWriteTime = File.GetLastWriteTime("d:\\test\\1.txt");
            //    //    if (lastWriteTime.Year < 1980 || lastWriteTime.Year > 2107)
            //    //    {
            //    //        lastWriteTime = new DateTime(1980, 1, 1, 0, 0, 0);
            //    //    }
            //    //    zipArchiveEntry.LastWriteTime = lastWriteTime;
            //    //    using (Stream stream2 = zipArchiveEntry.Open())
            //    //    {
            //    //        stream.CopyStreamTo(stream2);
            //    //    }
            //    //}

            //    //using (Stream stream = File.Open("d:\\test\\2.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            //    //{
            //    //    ZipArchiveEntry zipArchiveEntry = zip.CreateEntry("2.txt", FileAttributes.Directory);
            //    //    DateTime lastWriteTime = File.GetLastWriteTime("d:\\test\\2.txt");
            //    //    if (lastWriteTime.Year < 1980 || lastWriteTime.Year > 2107)
            //    //    {
            //    //        lastWriteTime = new DateTime(1980, 1, 1, 0, 0, 0);
            //    //    }
            //    //    zipArchiveEntry.LastWriteTime = lastWriteTime;
            //    //    using (Stream stream2 = zipArchiveEntry.Open())
            //    //    {
            //    //        stream.CopyStreamTo(stream2);
            //    //    }
            //    //}
            //}


            //ZipFile.CreateFromDirectory("d:\\1.zip", "d:\\test", true, true);

            //ZipFile.ExtractToDirectory("d:\\1.zip", "d:\\test2", true, true);

            ////Environment.Exit(-1);

            //ZipFile.ExtractToDirectory("d:\\test.zip", "d:\\test1", true);

            //using (ZipArchive destination = ZipFile.Open("test.zip", ZipArchiveMode.Create))
            //{
            //    destination.CreateEntryFromFile("DevLib.Compression.xml", "DevLib.Compression.xml");
            //}

            //ZipFile.CreateFromDirectory("E:\\A", "E:\\1.zip", false, true);

            //Console.ReadLine();

            //ZipFile.OpenRead("E:\\1.zip").ExtractToDirectory("E:\\B", true);
            //var zip = ZipFile.OpenRead("c:\\22.zip");
            //foreach (ZipArchiveEntry item in zip.Entries)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //zip.ExtractToDirectory("c:\\3\\4", true);

            //ZipFile.CreateFromDirectory("c:\\1", "c:\\1.zip", true, true);
            //ZipFile.CreateFromDirectory("c:\\1", "c:\\2.zip", true, false);
            //ZipFile.CreateFromDirectory("c:\\1", "c:\\3.zip", false, true);
            //ZipFile.CreateFromDirectory("c:\\1", "c:\\4.zip", false, false);
            //var zip1 = ZipFile.Open("c:\\4.zip", ZipArchiveMode.Update);
            //zip1.CreateEntryFromFile("c:\\22.zip", "22.zip");
            //zip1.Dispose();
        }

        private static void TestDevLibServiceProcess(string[] args)
        {
            PrintMethodName("Test DevLib.ServiceProcess");

            Argument argument = new Argument("-s");

            ServiceProcessTestService testService = new ServiceProcessTestService();

            WindowsServiceBase.Run(testService, null, new string[] { "s" });

            //WindowsServiceBase.Run(testService, args);
        }

        private static void PrintStartInfo()
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Start DevLib.Samples ...");
            Console.ForegroundColor = originalForeColor;
            Console.WriteLine();
        }

        private static void PrintExitInfo()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void TestDevLibDesignPatterns()
        {
            PrintMethodName("Test DevLib.DesignPatterns");


            var deferQueue = new DeferQueue<string>(items =>
            {
                Console.WriteLine(items.Length);
            }, 250, 3000, 0);

            for (int i = 0; i < 100; i++)
            {
                deferQueue.Enqueue("");
                Thread.Sleep(10);
            }

            Console.WriteLine("--");

            //for (int i = 0; i < 10; i++)
            //{
            //    deferQueue.Enqueue("");
            //    Thread.Sleep(1500);
            //}

            Console.ReadLine();

            TestClass a = Singleton<TestClass>.Instance;

            a.Age = 30;
            a.Name = "Same one";

            Parallel.For(0, 5, (i) =>
            {
                int age = i;
                Singleton<TestClass>.Instance.Age = age;
                Singleton<TestClass>.Instance.Age.ConsoleOutput();
                Singleton<TestClass>.Instance.Name.ConsoleOutput();
            });

            Singleton<TestClass>.Instance.Age.ConsoleOutput();

        }

        private static void TestDevLibAddIn()
        {
            PrintMethodName("Test DevLib.AddIn");

            var addin = new AddInDomain();
            addin.CreateInstance<DateTime>();

            //var info = new AddInActivatorProcessInfo();

            //var addin = new AddInDomain("DevLib.AddIn.Sample");
            //addin.Loaded += new EventHandler(addin_Started);
            //addin.Reloaded += new EventHandler(addin_Restarted);
            //addin.Unloaded += new EventHandler(addin_Stopped);
            //addin.DataReceived += new DataReceivedEventHandler(addin_DataReceived);

            //addin.CreateInstance<WcfServiceHost>(@"E:\Temp\WcfCalc.dll", @"E:\Temp\WcfCalc.dll.config");

            //using (AddInDomain domain = new AddInDomain("DevLib.AddIn.Sample1", false))
            //{
            //    var remoteObj = domain.CreateInstance<TestClass>();
            //    string a = remoteObj.ShowAndExit();
            //    Console.WriteLine(a);
            //}

            //AddInDomain domain = new AddInDomain("DevLib.AddIn.Sample1", true, new AddInDomainSetup { Platform = PlatformTargetEnum.AnyCPU });
            //AddInDomain domain3 = new AddInDomain("DevLib.AddIn.Sample3", true, new AddInDomainSetup { Platform = PlatformTargetEnum.AnyCPU });
            //var remoteObj = domain.CreateInstance<AsyncSocketServer>();
            //var remoteObj3 = domain3.CreateInstance<AsyncSocketServer>();
            //remoteObj.Start(2000);
            //remoteObj3.Start(2500);
            //remoteObj.DataReceived += remoteObj_DataReceived;
            //domain.DataReceived += domain_DataReceived;

            //remoteObj3.DataReceived += remoteObj_DataReceived;
            //domain3.DataReceived += domain_DataReceived;

            //Console.WriteLine("next");
            //domain.ProcessInfo.PrivateWorkingSetMemorySize.ConsoleOutput();
            //Console.ReadKey();
            //domain.AddInDomainSetupInfo.DllDirectory.ConsoleOutput();

            ////Task.Factory.StartNew(() =>
            ////{
            ////    Parallel.For(0, 2000, (loop) =>
            ////    {

            ////        AsyncSocketClient client1 = new AsyncSocketClient("127.0.0.1", 9999);
            ////        client1.Connect();
            ////        //client1.SendOnce("127.0.0.1", 9999, loop.ToString(), Encoding.ASCII);
            ////        client1.Send(loop.ToString(), Encoding.Default);
            ////    });
            ////});

            //Console.WriteLine("next");
            //Console.ReadKey();

            //AddInDomain domain1 = new AddInDomain("DevLib.AddIn.Sample2");
            //var remoteObj1 = domain1.CreateInstance<AsyncSocketClient>();
            //remoteObj1.Connect("127.0.0.1", 2500);

            //for (int i = 0; i < 20000; i++)
            //{
            //    new AsyncSocketClient("127.0.0.1", 2500).Connect().Send(i.ToString(), Encoding.Default);

            //    //remoteObj1.Send(DateTime.Now.ToString()+"  ", Encoding.Default);
            //}

            //Console.WriteLine("next1");
            //Console.ReadKey();

            //Task.Factory.StartNew(() =>
            //{
            //    Parallel.For(0, 20000, (loop) =>
            //    {
            //        new AsyncSocketClient("127.0.0.1", 999).Connect().Send(DateTime.Now.ToString(), Encoding.Default);
            //    });
            //});
            //remoteObj1.SendOnce("127.0.0.1", 9999, "!!!!!!!!!!!!!!!!hello555555555555", Encoding.Default);


            //Console.ReadKey();
            //domain1.AddInDomainSetupInfo.DllDirectory.ConsoleOutput();

            //domain.Dispose();
            //domain1.Dispose();

            //addin.CreateInstance<TestClass>().TestAdd(1,2).ConsoleOutput();

            //addin.AddInObject.ConsoleOutput();
            //addin.ProcessInfo.RetrieveProperties().ConsoleOutput();

            //addin.AddInObject.GetType().AssemblyQualifiedName.ConsoleOutput();

            //addin.Dispose();
            //addin.Dispose();
            //addin.Dispose();
            //addin.Reload();


            //var form = addin.CreateInstance<WinFormRibbon>();
            //form.ShowDialog();

            //wcf = addin.CreateInstance<WcfServiceHost>(new object[] { @"E:\Temp\WcfCalc.dll", @"E:\Temp\WcfCalc.dll.config" });
            //wcf.Initialize(@"E:\Temp\WcfCalc.dll", @"E:\Temp\WcfCalc.dll.config");
            //wcf.Open();

            //addin.Dispose();

            //WcfServiceHost wcf = n  WcfServiceHost();

            //ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Path.Combine(Environment.CurrentDirectory, "WcfCalc.dll.config") }, ConfigurationUserLevel.None);
            ////WcfIsolatedServiceHost wcf = new WcfIsolatedServiceHost();
            //wcf.Initialize(Path.Combine(Environment.CurrentDirectory, "WcfCalc.dll"));
            //wcf.Open();

        }

        static void domain_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        static void remoteObj_DataReceived(object sender, AsyncSocketSessionEventArgs e)
        {

        }

        static void addin_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        static void addin_Started(object sender, EventArgs e)
        {


            Console.WriteLine("!!!!!!!!addin_Started    ");
            //e.ProcessInfo.RetrieveProperties().ForEach((k, v) => { Console.WriteLine(string.Format("{0} = {1}", k, v)); });
            Debug.WriteLine("!!!!!!!!addin_Started    ");
        }

        static void addin_Stopped(object sender, EventArgs e)
        {
            Console.WriteLine("!!!!!!!!addin_Stopped    ");
            //e.ProcessInfo.RetrieveProperties().ForEach((k, v) => { Console.WriteLine(string.Format("{0} = {1}", k, v)); });
            Debug.WriteLine("!!!!!!!!addin_Stopped    ");
        }

        static void addin_Restarted(object sender, EventArgs e)
        {
            Console.WriteLine("!!!!!!!!addin_Restarted    ");
            //e.ProcessInfo.RetrieveProperties().ForEach((k, v) => { Console.WriteLine(string.Format("{0} = {1}", k, v)); });
            Debug.WriteLine("!!!!!!!!addin_Restarted    ");
            var temp = (sender as AddInDomain).AddInObject as WcfServiceHost;
            if (temp != null)
            {
                temp.Open();
            }
        }

        private static void ShowArray(Array theArray)
        {
            foreach (Object o in theArray)
            {
                Console.Write("[{0}]", o);
            }
            Console.WriteLine();
        }

        public class MyClass
        {
            public MyClass()
            {
                this.MyProperty = true;
                this.MyProperty1 = new object();
            }

            public bool MyProperty { get; set; }

            public object MyProperty1 { get; set; }
        }

        public class TestJsonClass
        {
            public string MyProperty1 { get; set; }
            public int MyProperty2 { get; set; }
            public Day MyProperty3 { get; set; }
        }

        private static void TestCodeSnippets()
        {
            RSACryptoServiceProvider rsa11 = new RSACryptoServiceProvider();
            var privateKey11 = rsa11.ExportCspBlob(true).ToHexString();
            var publicKey11 = rsa11.ExportCspBlob(false).ToHexString();




            new List<Person>() { new Person() }.Distinct(GenericEqualityComparer<Person>.By(i => i.FirstName)).ToList();

            //foreach (var item in TimeZoneInfo.GetSystemTimeZones())
            //{
            //    Console.WriteLine("/// <summary>");
            //    Console.WriteLine("/// StandardName: " + item.StandardName);
            //    Console.WriteLine("/// DisplayName: " + item.DisplayName);
            //    Console.WriteLine("/// </summary>");
            //    Console.WriteLine(@"public const string {0} = ""{1}"";".FormatInvariantCultureWith(item.Id.Replace(" ", "_").Replace(".", "_").Replace("+", "_Plus_").Replace("-", "_Minus_"), item.Id));
            //    Console.WriteLine();

            //}


            DateTime localTime = DateTime.UtcNow;

            var localTimeString = localTime.ToString("o");

            var t1 = localTime.ToTimeZone(-4);

            var t1String = t1.ToString("o");

            var testJson = new TestJsonClass { MyProperty1 = "a", MyProperty2 = 1, MyProperty3 = Day.Friday };
            var testjs = testJson.SerializeJsonString();
            var testjso = testjs.DeserializeJsonString<TestJsonClass>();


            var r1 = 1234.Base62Encode();
            var r2 = r1.Base62Decode();

            var request = "\x0081";

            var requestE = request.ToByteArray(TextExtensions.CP1252);


            string astring = "PO:\t{0}\nDate:\t{1}";
            var bs = string.Format(astring, "a", "b");

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            var privateKey = rsa.ExportParameters(true);
            var publicKey = rsa.ExportParameters(false);

            var privateKey1 = rsa.ExportCspBlob(true);
            var publicKey1 = rsa.ExportCspBlob(false);

            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
            var privateKey2 = rsa2.ExportParameters(true);
            var publicKey2 = rsa2.ExportParameters(false);

            var data = new string('a', 100000).ToByteArray();

            var signdata = data.SignDataRSA(privateKey1);

            var veri = data.VerifyDataRSA(signdata, publicKey1);


            string patha = Path.GetFullPath(".");

            RemotingObject<Person>.Register();
            Person aaPerson = RemotingObject<Person>.GetObject();
            Person bPerson = RemotingObject<Person>.GetObject();
            Person cPerson = bPerson.Get();
            Person dPerson = aaPerson.Get();

            var equa = RemotingObject.RemotingEquals(aaPerson, bPerson);
            var equb = RemotingObject<Person>.RemotingEquals(dPerson, cPerson);


            Type ta = Type.GetType("System.MarshalByRefObject, mscorlib");
            MethodInfo ma = ta.GetMethod("GetIdentity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(MarshalByRefObject) }, null);
            //var oa = ma.Invoke(null, new object[] { cPerson });

            //Type ta = d.GetType();
            //var pa = ta.GetProperty("IdentityObject", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //var oa = pa.GetValue(d);
            //pa = pa.PropertyType.GetProperty("ObjURI", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //oa = pa.GetValue(oa);


            GCNotification.GCDone += new Action<int>(GCNotification_GCDone);

            Thread.Sleep(1000);

            GC.Collect(2, GCCollectionMode.Forced);

            Console.ReadLine();
            string xmltotest = @"d:\a.txt".ReadTextFile();

            string xmltotest1 = xmltotest.ToIndentXml();

            Console.WriteLine(xmltotest.IsValidXml());

            Benchmark.Initialize();

            Benchmark.Run(i =>
            {
                xmltotest.ToIndentXml();
            }, iteration: 100000);


            Console.ReadLine();



            UTF8Encoding utf8 = new UTF8Encoding();
            UTF8Encoding utf8EmitBOM = new UTF8Encoding(true);

            Console.WriteLine("utf8 preamble:");
            ShowArray(utf8.GetPreamble());

            Console.WriteLine("utf8EmitBOM:");
            ShowArray(utf8EmitBOM.GetPreamble());

            Console.ReadLine();

            //var tempp0 = @"\\gytp\d$\Download";
            var tempp = LogConfigManager.GetFileFullPath(@"\\gytp\d$\Download");
            var tempp1 = LogConfigManager.GetFileFullPath(@"$TMp$\a\Logs\b.log");
            var tempp2 = LogConfigManager.GetFileFullPath(@"%TmP\a\Logs\b.log");
            var tempp3 = LogConfigManager.GetFileFullPath(@"%tMP%\a\Logs\b.log");

            string evnv = Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Process);
            string evnv1 = Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.User);
            string evnv2 = Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine);

            var en = @"%tmp%\a\b\c".Split(Path.DirectorySeparatorChar);

            for (int i = 0; i < en.Length; i++)
            {
                if (en[i].StartsWith("%"))
                {
                    en[i] = Environment.GetEnvironmentVariable(en[i].Trim('%'));
                }
            }

            string dts = DateTime.Now.ToString((string)null);

            string path = "LDAP://contoso.local";

            LdapEntry ldapEntry = new LdapEntry(path, "aaa", "bbb!");

            //var ldapuser = ldapEntry.Authenticate("aaa","ccc");

            var ldapusers = ldapEntry.GetUser("aaa");


            var result = ldapEntry.Authenticate("ccc", "ddd");

            ////Uri baseAddressUri = new Uri("http://localhost:888/opc");

            ////string rp = Path.Combine(baseAddressUri.AbsolutePath, "def").Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            ////var path = new Uri(baseAddressUri, rp);

            ////Uri serviceContractUri = new Uri(path);

            //WebServiceClientProxyFactory wsf = new WebServiceClientProxyFactory("http://wsf.cdyne.com/WeatherWS/Weather.asmx");
            ////var wsc = wsf.GetProxy();
            ////var mds = wsc.Methods;
            ////var rv = wsc.CallMethod("GetCityForecastByZIP", "33133");

            ////WebServiceClientProxy.CompileAssembly("d:\\ws.xml", "d:\\ws1.dll");

            ////WebServiceClientProxy proxy = new WebServiceClientProxy("http://localhost:888/opc");

            ////var w1 = WcfClientProxy<IWcfTest>.GetClientBaseInstance("http://localhost:888/test");
            ////var w2 = WcfClientProxy<IWcfTest>.GetPerSessionUnthrowableInstance("http://localhost:888/test");



            //DynamicClientProxyFactory cf = new DynamicClientProxyFactory("http://wsf.cdyne.com/WeatherWS/Weather.asmx", "d:\\1\\1.dll");

            //var cf1 = DynamicClientProxyFactory.Load("d:\\1\\1.dll");


            //var epr = cf.Endpoints;

            ////var dp = cf.Foo();

            //using (var dp = cf1.GetClientBaseProxy("http://wsf.cdyne.com/WeatherWS/Weather.asmx"))
            //{
            //    //DynamicClientObject obj = new DynamicClientObject(cf.Types.First(i => i.Name == "AgentInfoDTO"));
            //    //obj.CallConstructor();
            //    //obj.SetProperty("Alias", "a1");
            //    //obj.SetProperty("UID", "UID1");
            //    //obj.SetProperty("HostName", "h1");
            //    //obj.SetProperty("SessionIP", "127.0.0.1");
            //    //obj.SetProperty("Port", 888);


            //    var retval = dp.CallMethod("GetCityForecastByZIP", "33133");
            //}

            //RemotingObject<FooBar>.Register("a");
            //RemotingObject<Person>.Register("a");


            RemotingObject<FooBar>.Register();
            FooBar fb1 = RemotingObject<FooBar>.GetObject();
            fb1.foo = "fb1";
            fb1.bar = 111;

            RemotingObject<FooBar>.Register("fb2");
            FooBar fb2 = RemotingObject<FooBar>.GetObject("fb2");
            fb2.foo = "fb2";
            fb2.bar = 222;

            FooBar fb3 = RemotingObject<FooBar>.GetObject();
            string foo3 = fb3.foo; // fb1
            int bar3 = fb3.bar; // 111

            FooBar fb4 = RemotingObject<FooBar>.GetObject("fb2");
            string foo4 = fb4.foo; //fb2
            int bar4 = fb4.bar; // 222

            RemotingObject<FooBar>.Register();
            FooBar fb5 = RemotingObject<FooBar>.GetObject();
            string foo5 = fb5.foo; // fb1
            int bar5 = fb5.bar; //111

            FooBar fb = ArgumentParser.ParseTo<FooBar>(" /bar:123 --ok -foo=aaa");

            Expression<Func<Company, object>> exp1 = p => p.Bosses[3].Home.Street;

            exp1.ExtractPropertyName();

            string expression1 = PropertyEvaluator.ExtractPropertyName<Company>(p => p.Bosses[3].Home.Street); // will return Bosses[3].Home.Street

            string expression2 = PropertyEvaluator.ExtractPropertyName<Company, int>(p => p.Bosses[3].Home.PostCode); //will return Bosses[3].Home.PostCode

            var aaa = "abcdeABCDe".ReplaceAny('W', false, 'a', 'E');

            Benchmark.Run(i =>
            {
                var csv = new CsvDocument();
                csv.Load(@"d:\work\temp\2.csv", false);
            });

            Benchmark.Run(i =>
            {
                var csv = new CsvDocument();
                csv.Load(@"d:\work\temp\2.csv", false);
            });

            Benchmark.Run(i =>
            {
                var csv = new CsvDocument();
                csv.Load(@"d:\work\temp\2.csv", false);
            });

            //Keyboard.Press(Key.Ctrl);
            //Keyboard.Press(Key.Alt);
            //Keyboard.Type(Key.Delete);
            //Keyboard.Release(Key.Alt);
            //Keyboard.Release(Key.Ctrl);

            //MemorySnapshot s1 = MemorySnapshot.FromProcess(Process.GetCurrentProcess().Id);
            //s1.ToFile(@"s1.xml");

            ////Thread.Sleep(3000);

            //MemorySnapshot s2 = MemorySnapshot.FromProcess(Process.GetCurrentProcess().Id);
            //s1.ToFile(@"s2.xml");

            //s2.CompareTo(s1).ToFile("sDiff.xml");

            //Console.WriteLine("done");
            Console.ReadLine();

            //Keyboard.Type(Key.LWin);
            //Keyboard.Type("notepad");
            //Keyboard.Type(Key.Enter);
            //Keyboard.Press(Key.LeftShift);
            //Keyboard.Type("h");
            //Keyboard.Release(Key.LeftShift);
            //Keyboard.Type("ello DevLib.Diagnostics.Input");
            //Keyboard.Type(Key.Enter);
            //Keyboard.Type("Hello DevLib.Diagnostics.Input", 10);
            //Keyboard.Type(Key.Enter);
            //Keyboard.Type("Bye bye.");

            Console.ReadLine();

            Person pa = new Person();
            pa.Error += (s, ev) =>
            {
                var eee = ev;
            };
            pa.DoTryf();

            Console.ReadLine();

            SpellingOptions sa = new SpellingOptions();
            SpellingOptions sb = new SpellingOptions();
            SpellingOptions sc = new SpellingOptions();

            sa.SpellCheckCAPS = false;
            sa.SpellCheckWhileTyping = true;
            sa.SuggestCorrections = false;

            sb.SpellCheckCAPS = false;
            sb.SpellCheckWhileTyping = true;
            sb.SuggestCorrections = false;

            sc.SpellCheckCAPS = false;
            sc.SpellCheckWhileTyping = false;
            sc.SuggestCorrections = true;

            sa.GetHashCode().ConsoleOutput();
            sb.GetHashCode().ConsoleOutput();
            sc.GetHashCode().ConsoleOutput();

            Console.ReadLine();

            InternalLogger.Log(1);
            InternalLogger.Log(2);
            InternalLogger.Log(3);
            InternalLogger.Log(4);

            Console.ReadLine();

            PrintMethodName("Test CodeSnippets");
            RetryAction.Execute(i =>
            {
                throw new Exception();
            }, null, 9, 250);

            Console.ReadLine();

            for (int i = 0; i < 100000; i++)
            {
                LogManager.Open("AAA.log", "LoggingConfig.xml").Log(LogLevel.ERRO, i, DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }

            Console.ReadLine();

            object aPerson = new Person();
            aPerson.InvokeMethod("ShowName");
            aPerson.InvokeMethodGeneric("ShowName", null, false, typeof(string));


            DateTime a = new DateTime();
            a.IsWeekend().ConsoleOutput();

            var b1 = new Dictionary<int, string>(1000000);
            ReaderWriterLockSlim rwl1 = new ReaderWriterLockSlim();
            Benchmark.Initialize();
            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int i = loop;
                        rwl1.EnterWriteLock();
                        try
                        {
                            b1[i] = DateTime.Now.ToString();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            rwl1.ExitWriteLock();
                        }
                    });
                }
            }, "ReaderWriterLockSlimDictW", 2);

            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        string output;
                        int i = loop;
                        rwl1.EnterReadLock();
                        try
                        {
                            b1.TryGetValue(2, out output);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            rwl1.ExitReadLock();
                        }
                    });
                }
            }, "ReaderWriterLockSlimDictR", 2);

            var b = new Dictionary<int, string>(1000000);
            ReaderWriterLock rwl = new ReaderWriterLock();
            Benchmark.Initialize();
            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int i = loop;
                        rwl.AcquireWriterLock(Timeout.Infinite);
                        try
                        {
                            b[i] = DateTime.Now.ToString();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            rwl.ReleaseWriterLock();
                        }
                    });
                }
            }, "ReaderWriterLockDictW", 2);

            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        string output;
                        int i = loop;
                        rwl.AcquireReaderLock(Timeout.Infinite);
                        try
                        {
                            b.TryGetValue(2, out output);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            rwl.ReleaseReaderLock();
                        }
                    });
                }
            }, "ReaderWriterLockDictR", 2);

            var d = new Dictionary<int, string>(1000000);
            Benchmark.Initialize();
            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int i = loop;
                        //c[i] = DateTime.Now.ToString();
                        lock (((ICollection)d).SyncRoot)
                        {
                            try
                            {
                                d[i] = DateTime.Now.ToString();
                            }
                            catch
                            {
                            }
                        }
                    });
                }
            }, "LockDictW", 2);

            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int i = loop;
                        string output;
                        lock (((ICollection)d).SyncRoot)
                        {
                            try
                            {
                                d.TryGetValue(2, out output);
                            }
                            catch
                            {
                            }
                        }
                    });
                }
            }, "LockDictR", 2);





            var c = new Dictionary<int, string>(1000000);
            Benchmark.Initialize();
            Benchmark.Run(delegate
            {
                Parallel.For(0, 1000000, (loop) =>
                {
                    int i = loop;
                    c.Add(i, DateTime.Now.ToString());
                });
            }, "DictW", 1);

            Benchmark.Run(delegate
            {
                for (int loop = 0; loop < 1000000; loop++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int i = loop;
                        string output;

                        try
                        {
                            c.TryGetValue(2, out output);
                        }
                        catch
                        {
                        }
                    });
                }
            }, "DictR", 1);

            var e = new ConcurrentDictionary<int, string>(10000, 1000000);
            Benchmark.Initialize();
            Benchmark.Run(delegate
            {
                Parallel.For(0, 1000000, (loop) =>
                {
                    int i = loop;
                    e.TryAdd(i, DateTime.Now.ToString());
                });
            }, "ConcurrentDictionaryW", 1);

            Benchmark.Run(delegate
            {
                Parallel.For(0, 1000000, (loop) =>
                {
                    int i = loop;
                    string output;
                    e.TryGetValue(2, out output);
                });
            }, "ConcurrentDictionaryR", 1);


            Console.WriteLine("done!");
            Console.ReadKey();



            //AddInProcess addin = new AddInProcess(Platform.AnyCpu);
            //Activator.CreateInstance<>

            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Path.Combine(Environment.CurrentDirectory, "test.config") }, ConfigurationUserLevel.None);
            //Configuration config1 = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap() { ExeConfigFilename = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()) }, ConfigurationUserLevel.None);
            //config.Sections.Add(.AppSettings.Settings.Add("key1", "value1");
            //config.AppSettings.Settings.Add("key2", "value2");
            //config.AppSettings.Settings.Add("key3", "value3");
            //config.AppSettings.Settings["key3"].Value = "value3";
            //config.Save(ConfigurationSaveMode.Minimal, true);
            //config.SaveAs("test1.config", ConfigurationSaveMode.Minimal, true);

            //Properties.Settings.Default



            //Dns.GetHostAddresses("localhost").ForEach(p => p.ConsoleWriteLine());
            //NetworkInterface.GetAllNetworkInterfaces().ForEach(p => p.Id.ConsoleWriteLine());
            //"hello".ConsoleOutput();
            //"Hello".ConsoleOutput(" MD5 of {0} is ", false).ToMD5().ConsoleOutput();
            //TestEventClass obj = null;
            //"Hello".ConsoleOutput(obj);
            //"hello".ConsoleOutput("  ");

            //int[] a = null;
            //a.IsEmpty().ConsoleOutput();


            //AssemblyAccessor.AssemblyVersion().ConsoleOutput();
            //AssemblyAccessor.AssemblyDescription().ConsoleOutput();

            //Dictionary<int, string> dict = new Dictionary<int, string>();

            //for (int i = 0; i < 10; i++)
            //{
            //    dict.Add(i, i.ToString() + "Hello");
            //}

            //byte[] bys = new byte[] { 1, 2, 3, 4, 5 };
            //bys.ToHexString();
            //string.Empty.ToMD5().ConsoleOutput();
            //string.Empty.ToHexByteArray().ForEach((p) => { p.ConsoleOutput(); });
            //"monday".IsItemInEnum<DayOfWeek>().ConsoleOutput();
            //"asd".ToEnum<DayOfWeek>().ConsoleOutput();
            //decimal? de = null;
            //de.HasValue.ConsoleOutput("has value {0}");
            //long? lo = (long?)de;
            //lo.ConsoleOutput();

            //TestEventClass testclass = new TestEventClass() { MyName = "a" };
            //var a = XDocument.Parse(testclass.ToXml(Encoding.UTF8));
            //XmlDocument b = new XmlDocument();
            //XmlNode node = b.CreateElement("Hello");
            //"AppendChildNodeTo".AppendChildNodeTo(node);
            //node.CreateChildNode("CreateChildNode");
            //b.AppendChild(node);

            //"hello".Base64Encode().ConsoleOutput().Base64Decode().ConsoleOutput();
            //"Hello".ToByteArray().Compress(CompressionType.Deflate).Decompress(CompressionType.Deflate).ToEncodingString().ConsoleOutput();
            //"DevLib.ExtensionMethods.dll".ReadBinaryFile().ConsoleOutput().Compress().CreateBinaryFile("demo.bin").OpenContainingFolder();
            //Trace.Listeners.Add(new ConsoleTraceListener());
            //Trace.Listeners.Add(new TextWriterTraceListener("trace.log"));
            //Trace.AutoFlush = false;
            //Trace.WriteLine("Entering Main");
            //Trace.TraceError("hello error");
            //Trace.TraceError("hello error");
            //Console.WriteLine("Hello World.");
            //Trace.WriteLine("Exiting Main");
            //Trace.Unindent();

            //TestClass aclass = new TestClass() { MyName = "aaa" };
            //aclass.ToByteArray().Compress().WriteBinaryFile("test.bin").ReadBinaryFile().Decompress().ToObject<TestEventClass>().MyName.ConsoleOutput();
            //aclass.ToXml().ToByteArray(Encoding.UTF8).Compress().Decompress().ToEncodingString(Encoding.UTF8).FromXml<TestEventClass>().MyName.ConsoleOutput();

            //Environment.GetLogicalDrives().ForEach(p => p.ConsoleOutput());
            //Environment.MachineName.ConsoleOutput();
            //Environment.OSVersion.Platform.ConsoleOutput();
            //Environment.WorkingSet.ConsoleOutput();
            //Path.GetDirectoryName(@"""""""").ConsoleOutput();
            //@"""""""".GetFullPath().ConsoleOutput();

            //@"hello".Base64Encode().ConsoleOutput();

            //WMIUtilities.QueryWQL(WMIUtilities.MACADDRESS).ForEach(p => p.ConsoleOutput());


            //TraceSource ts = new TraceSource("TraceTest");
            //SourceSwitch sourceSwitch = new SourceSwitch("SourceSwitch", "Verbose");
            //ts.Switch = sourceSwitch;
            //int idxConsole = ts.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
            //ts.Listeners.Add(new TextWriterTraceListener("test.log"));
            //ts.Listeners[idxConsole].Name = "console";

            //ts.Listeners["console"].TraceOutputOptions |= TraceOptions.Callstack;
            //ts.TraceEvent(TraceEventType.Warning, 1);
            //ts.Listeners["console"].TraceOutputOptions = TraceOptions.DateTime;
            //// Issue file not found message as a warning.
            //ts.TraceEvent(TraceEventType.Warning, 2, "File Test not found");

            //// Issue file not found message as a verbose event using a formatted string.
            //ts.TraceEvent(TraceEventType.Verbose, 3, "File {0} not found.", "test");
            //// Issue file not found message as information.
            //ts.TraceInformation("File {0} not found.", "test");


            //ts.Listeners["console"].TraceOutputOptions |= TraceOptions.LogicalOperationStack;
            //// Issue file not found message as an error event.
            //ts.TraceEvent(TraceEventType.Error, 4, "File {0} not found.", "test");


            //// Test the filter on the ConsoleTraceListener.
            //ts.Listeners["console"].Filter = new SourceFilter("No match");
            //ts.TraceData(TraceEventType.Error, 5,
            //    "SourceFilter should reject this message for the console trace listener.");
            //ts.Listeners["console"].Filter = new SourceFilter("TraceTest");
            //ts.TraceData(TraceEventType.Error, 6,
            //    "SourceFilter should let this message through on the console trace listener.");
            //ts.Listeners["console"].Filter = null;


            //// Use the TraceData method. 
            //ts.TraceData(TraceEventType.Warning, 7, "hello");
            //ts.TraceData(TraceEventType.Warning, 8, new object[] { "Message 1", "Message 2" });


            // Activity tests.

            //ts.TraceEvent(TraceEventType.Start, 9, "Will not appear until the switch is changed.");
            //ts.Switch.Level = SourceLevels.ActivityTracing | SourceLevels.Critical;
            //ts.TraceEvent(TraceEventType.Suspend, 10, "Switch includes ActivityTracing, this should appear");
            //ts.TraceEvent(TraceEventType.Critical, 11, "Switch includes Critical, this should appear");

            //ts.Flush();
            //ts.Close();

            //CodeTimer.Initialize();
        }

        static void GCNotification_GCDone(int obj)
        {
            Console.WriteLine("GC done.");
        }

        private static void TestDevLibDiagnostics()
        {
            PrintMethodName("Test Dev.Lib.Diagnostics");

            ConcurrentDictionary<int, string> safeDict = new ConcurrentDictionary<int, string>();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            List<int> list = new List<int>();
            ConcurrentBag<string> safeBag = new ConcurrentBag<string>();

            Benchmark.Initialize();

            int times = 1000 * 10;

            TestClass testClass = new TestClass { Name = "Bill" };
            object[] parameters = new object[] { 1, 2 };
            MethodInfo methodInfo = typeof(TestClass).GetMethod("TestAdd");

            Benchmark.Run(delegate { }, string.Empty, 1, delegate { });

            Benchmark.Run(delegate { testClass.TestAdd(1, 2); }, iteration: times);

            //methodInfo.Invoke(testClass, parameters).ConsoleOutput();
            //CodeTimer.Time(new Action(() =>
            //{
            //    methodInfo.Invoke(testClass, parameters);
            //}), times, "Reflection invoke1");

            //testClass.InvokeMethod("TestAdd", parameters).ConsoleOutput();
            //CodeTimer.Time(new Action(() =>
            //{
            //    testClass.InvokeMethod("TestAdd", parameters);
            //}), times, "Reflection invoke3");

            //ReflectionUtilities.DynamicMethodExecute(methodInfo, testClass, parameters).ConsoleOutput();
            //CodeTimer.Time(times, "DynamicMethodExecute", () =>
            //{
            //    ReflectionUtilities.DynamicMethodExecute(methodInfo, testClass, parameters);
            //});

            //ReflectionUtilities.FastInvokeExecute(methodInfo, testClass, parameters).ConsoleOutput();
            //CodeTimer.Time(times, "FastInvokeExecute", () =>
            //{
            //    ReflectionUtilities.FastInvokeExecute(methodInfo, testClass, parameters);
            //});


            //CodeTimer.Time(1, "No action", () => { });

            //new Action(() => { }).CodeTime(times);

            //CodeTimer.Time(times, "ConcurrentDictionary1", () =>
            //{
            //    safeDict.AddOrUpdate(1, "hello", (key, oldValue) => oldValue);
            //}, null);

            //CodeTimer.Time(times, "Dictionary1", () =>
            //{
            //    dict.Update(1, "hello");
            //});

            //CodeTimer.Time(times, "ConcurrentDictionary2", () =>
            //{
            //    safeDict.AddOrUpdate(2, "hello", (key, oldValue) => oldValue);
            //});

            //CodeTimer.Time(times, "Dictionary2", () =>
            //{
            //    dict.Update(2, "hello");
            //});

            //CodeTimer.Time(times, "ConcurrentBag1", () =>
            //{
            //    safeBag.Add("hello");
            //});
        }

        private static void TestDevLibExtensionProperties()
        {
            FooBar a = new FooBar();

            ExtensionPropertiesClass<FooBar> xa = new ExtensionPropertiesClass<FooBar>(a);

            xa["A"] = "hello";

            var xaA = xa["A"];
        }

        private static void TestDevLibExtensionMethods()
        {
            PrintMethodName("Test Dev.Lib.ExtensionMethods");

            var t1 = "int32".GetType(false, false, true);

            var name1 = "1.txt".RenameFile();

            var aa = "abcd".Remove(null, true);

            #region SerializationExtensions

            Person person = new Person("foo", "好的", 1);
            person.WriteXml("1.xml", true);
            person.SerializeXmlString().DeserializeXmlString(new Type[] { typeof(Person) });
            Console.ReadLine();
            //person.SerializeJson().ConsoleOutput().DeserializeJson<Person>();
            //var aperson = person.SerializeJson(Encoding.UTF8).ConsoleOutput().DeserializeJson<Person>(Encoding.UTF8);
            var aperson = person.SerializeXmlString().DeserializeXmlString<Person>().LastName.ConsoleOutput();
            var bperson = person.SerializeJsonString();

            Console.ReadKey();

            #endregion

            #region Array
            TestClass[] sourceArray
                = new TestClass[]
            {
                new TestClass(){ Name="a"},
                new TestClass(){ Name="b"},
                new TestClass(){ Name="c"},
            };

            TestClass[] appendArray = new TestClass[]
            {
                new TestClass(){ Name="d"},
                new TestClass(){ Name="e"},
                new TestClass(){ Name="f"},
            };

            int[] sourceValueTypeArray
                = new int[]
            {
                1,
                2,
                3,
            };

            int[] appendValueTypeArray = new int[]
            {
                4,
                5,
                6,
            };

            //sourceArray = null;
            appendArray.AddRangeTo(ref sourceArray, true);

            sourceArray[1].Name = "change1";
            appendArray[1].Name = "change2";
            sourceArray.ForEach((p) => { p.Name.ConsoleOutput(); });

            sourceValueTypeArray.AddRangeTo(ref appendValueTypeArray);
            appendValueTypeArray.ForEach((p) => { p.ConsoleOutput(); });
            "".ConsoleOutput();
            sourceValueTypeArray[1] = 3;
            appendValueTypeArray.FindArrayIndex(sourceValueTypeArray).ConsoleOutput();

            "End: ArrayExtensions".ConsoleOutput();
            #endregion

            #region Byte
            byte[] bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //var obj = bytes.ToObject<int>();

            "compress".ConsoleOutput();
            var input = sourceArray.SerializeBinary();
            input.Length.ConsoleOutput();

            var compress = input.Compress();
            compress.Length.ConsoleOutput();

            var output = compress.Decompress();
            output.Length.ConsoleOutput();
            #endregion

            #region Collection

            #endregion

            #region EventHandler
            //TestEventClass testEventClassObject = new TestEventClass() { MyName = "testEventClassObject" };
            //testEventClassObject.OnTestMe += new EventHandler<EventArgs>(testEventClassObject_OnTestMe);
            //testEventClassObject.TestMe();
            #endregion

            #region IO
            var a = "   C:\\asdasd\\ \" \"   ";
            var c = Path.GetInvalidPathChars();
            string b = a.RemoveAny(false, c);
            "hello".WriteTextFile(@".\out\hello.txt").GetFullPath().OpenContainingFolder();

            //DateTime.Now.CreateBinaryFile(@".\out\list.bin").ConsoleWriteLine().ReadTextFile().ConsoleWriteLine();
            //@".\out\list.bin".ReadBinaryFile<DateTime>().ConsoleWriteLine();
            #endregion

            #region Object
            //sourceArray.ToJson().FromJson<TestEventClass[]>().ForEach((p) => { p.MyName.ConsoleWriteLine(); });
            //sourceArray.ToXml(Encoding.UTF8).ConsoleWriteLine();

            sourceArray.ForEach((p) => { p.CopyPropertiesFrom(appendArray[1]); });
            sourceArray.ForEach((p) => { p.Name.ConsoleOutput(); });
            "End: Object".ConsoleOutput();
            #endregion

            #region Security
            string inputString = "Hello I am secret.".ConsoleOutput();

            #endregion

            #region String

            #endregion

        }

        static AsyncSocketTcpServer staticserver = null;


        private static void TestDevLibNet()
        {
            PrintMethodName("Test Dev.Lib.Net");

            // rangeA.Begin is "192.168.0.0", and rangeA.End is "192.168.0.255".
            var rangeA = IPAddressRange.Parse("192.168.0.0/255.255.255.0");
            var a1=rangeA.Contains(IPAddress.Parse("192.168.0.34")); // is True.
            var a2 = rangeA.Contains(IPAddress.Parse("192.168.10.1")); // is False.
            var a3 = rangeA.ToCidrString(); // is 192.168.0.0/24

            // rangeB.Begin is "192.168.0.10", and rangeB.End is "192.168.10.20".
            var rangeB = IPAddressRange.Parse("192.168.0.10 - 192.168.10.20");
            var a4 = rangeB.Contains(IPAddress.Parse("192.168.3.45")); // is True.
            var a5 = rangeB.Contains(IPAddress.Parse("192.168.0.9")); // is False.

            var rangeC = IPAddressRange.Parse("fe80::/10"); // Support CIDR expression and IPv6.
            var a6 = rangeC.Contains(IPAddress.Parse("fe80::d503:4ee:3882:c586%3")); // is True.
            var a7 = rangeC.Contains(IPAddress.Parse("::1")); // is False.

            // "Contains()" method also support IPAddressRange argument.
            var rangeD1 = IPAddressRange.Parse("192.168.0.0/16");
            var rangeD2 = IPAddressRange.Parse("192.168.10.0/24");
            var a8 = rangeD1.Contains(rangeD2); // is True.

            // IEnumerable<IPAddress> support, it's lazy evaluation.
            //foreach (var ip in IPAddressRange.Parse("192.168.0.1/23"))
            //{
            //    Console.WriteLine(ip);
            //}

            // Constructors from IPAddress objects.
            var ipBegin = IPAddress.Parse("192.168.0.1");
            var ipEnd = IPAddress.Parse("192.168.0.128");
            var ipSubnet = IPAddress.Parse("255.255.255.0");

            var rangeE = new IPAddressRange(); // This means "0.0.0.0/0".
            var rangeF = new IPAddressRange(ipBegin, ipEnd);
            var rangeG = new IPAddressRange(ipBegin, maskLength: 24);
            var rangeH = new IPAddressRange(ipBegin, IPAddressRange.GetSubnetMaskLength(ipSubnet));

            // Calculates Cidr subnets
            var rangeI = IPAddressRange.Parse("192.168.0.0-192.168.0.255");
            var rangeJ = IPAddressRange.Parse("192.168.0.0/24");

            var a9 = rangeI.ToCidrString();  // is 192.168.0.0/24

            #region AsyncSocket

            try
            {
                AsyncSocketTcpServer server = new AsyncSocketTcpServer(999);
                server.DataReceived += (s, e) => Console.WriteLine(e.DataTransferred.ToEncodingString());
                server.Start(true);
                Console.WriteLine("tcp server is on");
            }
            catch
            {
                Console.WriteLine("tcp server is off");
            }

            Console.ReadLine();

            AsyncSocketTcpClient client0 = new AsyncSocketTcpClient("127.0.0.1", 999);
            client0.Start();
            client0.Send("hello from client0".ToByteArray());

            Console.ReadLine();
            //try
            //{
            //    AsyncSocketUdpServer udpserver = new AsyncSocketUdpServer(999);
            //    udpserver.DataReceived += server_DataReceived;
            //    udpserver.Start().IfTrue(() => { Console.WriteLine("udp server started."); });
            //}
            //catch (Exception e)
            //{
            //    e.ConsoleOutput();
            //}

            //Console.ReadKey();

            //AsyncSocketUdpClient.SendTo("127.0.0.1", 999, "Hello udp01".ToByteArray());
            //AsyncSocketUdpClient.SendTo("127.0.0.1", 999, "Hello udp02".ToByteArray());
            //AsyncSocketUdpClient.SendTo("127.0.0.1", 999, "Hello udp03".ToByteArray());
            //AsyncSocketUdpClient.SendTo("127.0.0.1", 999, "Hello udp04".ToByteArray());

            //AsyncSocketUdpClient udpclient = new AsyncSocketUdpClient("127.0.0.1", 999);
            //udpclient.Start();
            //udpclient.Send("Hello udp1".ToByteArray());
            //udpclient.Send("Hello udp2".ToByteArray());
            //udpclient.Send("Hello udp3".ToByteArray());
            //udpclient.Send("Hello udp4".ToByteArray());
            //udpclient.Stop();
            //udpclient.Send("Hello udp5".ToByteArray());
            //Console.WriteLine("udp client sent.");
            //Console.ReadKey();

            //AsyncSocketUdpServer s1 = new AsyncSocketUdpServer(999);
            //AsyncSocketUdpServer s2 = new AsyncSocketUdpServer(999);
            //AsyncSocketUdpServer s3 = new AsyncSocketUdpServer(999);

            //s1.DataReceived += (s, e) => { Console.WriteLine("s1 " + e.DataTransferred.ToEncodingString()); };
            //s2.DataReceived += (s, e) => { Console.WriteLine("s2 " + e.DataTransferred.ToEncodingString()); };
            //s3.DataReceived += (s, e) => { Console.WriteLine("s3 " + e.DataTransferred.ToEncodingString()); };

            //s1.Start(true);
            //s2.Start(true);
            //s3.Start(true);



            //AsyncSocketUdpClient c1 = new AsyncSocketUdpClient("127.0.0.1", 999);
            //c1.Start();
            //for (int i = 0; i < 9; i++)
            //{
            //    c1.Send(i.ToString().ToByteArray());
            //    Thread.Sleep(100);
            //}
            //s1.Stop();
            //c1.Stop();
            //c1.Start();
            //for (int i = 9; i < 19; i++)
            //{
            //    c1.Send(i.ToString().ToByteArray());
            //    Thread.Sleep(100);
            //}
            //s2.Stop();
            //for (int i = 99; i < 109; i++)
            //{
            //    c1.Send(i.ToString().ToByteArray());
            //    Thread.Sleep(100);
            //}
            //Console.ReadLine();


            AddInDomain tcpdomain = null;

            try
            {
                //throw new Exception();

                tcpdomain = new AddInDomain("AsyncSocketTcpServer");
                staticserver = tcpdomain.CreateInstance<AsyncSocketTcpServer>();
                staticserver.LocalPort = 999;
            }
            catch (Exception e)
            {
                e.ConsoleOutput();

                try
                {
                    staticserver = new AsyncSocketTcpServer(999);
                }
                catch (Exception ee)
                {
                    ee.ConsoleOutput();
                }
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    staticserver.Connected += server_Connected;
                    staticserver.Disconnected += server_Disconnected;
                    staticserver.DataReceived += server_DataReceived;
                    staticserver.DataSent += server_DataSent;
                    staticserver.Start().IfTrue(() => "Started!".ConsoleOutput()).IfFalse(() => "Start failed!".ConsoleOutput());
                    new AsyncSocketTcpServer(999).Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            });

            Console.WriteLine("press to stop server");
            Console.ReadKey();
            try
            {
                staticserver.Stop().IfTrue(() => { Console.WriteLine("server stoped!"); Console.ReadKey(); });
            }
            catch (Exception e)
            {
                e.ConsoleOutput();
            }


            try
            {
                staticserver.Start().IfTrue(() => { Console.WriteLine("server start!"); Console.ReadKey(); });
            }
            catch (Exception e)
            {
                e.ConsoleOutput();
            }

            Console.WriteLine("tcp client.");
            Console.ReadKey();
            AsyncSocketTcpClient tcpclient = new AsyncSocketTcpClient("127.0.0.1", 999);
            tcpclient.DataReceived += tcpclient_DataReceived;
            tcpclient.Start();
            tcpclient.Send("hello1".ToByteArray(), "test token1");
            Console.WriteLine("tcp client over1.");
            Console.ReadKey();
            tcpclient.Send("hello2".ToByteArray(), "test token2");
            Thread.Sleep(50);
            tcpclient.Send("hello3".ToByteArray(), "test token3");
            Console.WriteLine("tcp client over2.");
            Console.ReadKey();

            AsyncSocketTcpClient client1 = new AsyncSocketTcpClient("127.0.0.1", 999);
            client1.Start();
            client1.Send("hello".ToByteArray());

            Console.WriteLine("Over1.");
            Console.ReadKey();
            //client1.Disconnect();

            Console.WriteLine("press to stop server");
            Console.ReadKey();
            try
            {
                staticserver.Stop().IfTrue(() => { Console.WriteLine("server stoped!"); Console.ReadKey(); });
            }
            catch (Exception e)
            {
                e.ConsoleOutput();
            }


            try
            {
                staticserver.Start().IfTrue(() => { Console.WriteLine("server start!"); Console.ReadKey(); });
            }
            catch (Exception e)
            {
                e.ConsoleOutput();
            }

            //AsyncSocketClient client = new AsyncSocketClient("127.0.0.1", 999);
            //client.Connect();
            //client.Send("a");
            //client.Send("b");

            //AsyncSocketClient client1 = new AsyncSocketClient("127.0.0.1", 999);
            //client1.Connect();
            //client1.Send("c");
            //client1.Send("d");

            //client.Send("a");

            string temp = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345 || ";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 1; i++)
            {
                builder.Append(temp);
            }


            //client1.Send(builder.ToString());
            Console.WriteLine("Over2.");
            Console.ReadKey();

            AsyncSocketTcpClient client2 = new AsyncSocketTcpClient("127.0.0.1", 999);
            client2.Start();
            client2.Send(builder.ToString().ToByteArray());

            Console.WriteLine("Over3.");
            Console.ReadKey();
            //client1.Send(new byte[1] { 1 });
            client2.Stop();



            Console.WriteLine("Over4.");
            Console.ReadKey();

            List<AsyncSocketTcpClient> clientList = new List<AsyncSocketTcpClient>();

            for (int loop = 0; loop < 1000; loop++)
            {
                clientList.Add(new AsyncSocketTcpClient("127.0.0.1", 999));
            }

            foreach (var item in clientList)
            {
                item.DataSent += item_DataSent;
                item.DataReceived += tcpclient_DataReceived;
                item.Start();
                //Thread.Sleep(5);
            }

            for (int i = 0; i < clientList.Count; i++)
            {
                int loop = i;
                Task.Factory.StartNew(() =>
                {
                    clientList[loop].Send(loop.ToString().ToByteArray(), loop);
                    //Thread.Sleep(5);
                });
            }

            //for (int i = 0; i < 1000; i++)
            //{
            //    Thread.Sleep(5);
            //    client.Send(i.ToString());
            //    client1.Send(i.ToString() + Environment.NewLine);
            //}


            Console.WriteLine("Press to Stop Server.");
            Console.ReadKey();
            staticserver.Stop();

            Console.WriteLine("Over.");
            Console.ReadKey();
            //AsyncSocketServer svr = new AsyncSocketServer();
            //svr.DataReceived += svr_DataReceived;
            //svr.Start(9999);

            //Task.Factory.StartNew(() =>
            //{
            //    Parallel.For(0, 2000, (loop) =>
            //    {

            //        AsyncSocketClient client1 = new AsyncSocketClient("127.0.0.1", 9999);
            //        client1.Connect();
            //        client1.SendOnce("127.0.0.1", 9999, loop.ToString(), Encoding.ASCII);
            //        client1.Send(loop.ToString(), Encoding.ASCII);
            //    });
            //});

            //Console.ReadKey();

            //AsyncSocketClient client = new AsyncSocketClient();
            //client.DataSent += new EventHandler<AsyncSocketUserTokenEventArgs>(client_DataSent);
            //client.Connect("127.0.0.1", 9999);
            //client.Send("hello1  你好 end", Encoding.UTF8);
            //Thread.Sleep(100);
            //client.Send("hello2  你好 end", Encoding.UTF8);
            //Thread.Sleep(100);
            //client.Send("hello3  你好 end", Encoding.UTF8);
            //Thread.Sleep(100);
            //client.Send("hello2  你好 end", Encoding.UTF32);
            //client.Send("hello3  你好 end", Encoding.BigEndianUnicode);
            //client.Send("hello4  你好 end", Encoding.ASCII);
            //client.Send("hello5  你好 end", Encoding.UTF8);

            //Console.ReadKey();

            //svr.Dispose();
            //client.Dispose();

            if (tcpdomain != null)
            {
                tcpdomain.Dispose();
            }


            #endregion

            //NetUtilities.GetLocalIPArray().ForEach((p) => { p.ConsoleOutput(); });
            //NetUtilities.GetRandomPortNumber().ConsoleOutput();

        }

        static void item_DataSent(object sender, AsyncSocketSessionEventArgs e)
        {
            "client sent: {0} token{1}".FormatWith(e.DataTransferred.ToEncodingString(), e.UserToken).ConsoleOutput();
        }

        static void tcpclient_DataReceived(object sender, AsyncSocketSessionEventArgs e)
        {
            "client received: {0} token{1}".FormatWith(e.DataTransferred.ToEncodingString(), e.UserToken).ConsoleOutput();
        }

        static void server_DataSent(object sender, AsyncSocketSessionEventArgs e)
        {
            //e.DataTransferred.ToEncodingString().ConsoleOutput(" sent!");
        }

        static byte[] senddata = "hello".ToByteArray(Encoding.ASCII);
        static void server_DataReceived(object sender, AsyncSocketSessionEventArgs e)
        {
            staticserver.Send(e.SessionId, senddata);
            //(sender as AsyncSocketTcpServer).Send(e.SessionId, e.DataTransferred);

            //e.DataTransferred.ToEncodingString().ConsoleOutput(" received!");

            //(sender as AsyncSocketTcpServer).Send(e.SessionId, "from server".ToByteArray(Encoding.UTF8));
            //Thread.Sleep(10);
            //(sender as AsyncSocketTcpServer).Send(e.SessionId, e.SessionIPEndPoint.ToString().ToByteArray(Encoding.UTF8));
            //Thread.Sleep(10);
            //try
            //{
            //    (sender as AsyncSocketTcpServer).Send(e.SessionId, e.DataTransferred);
            //}
            //catch (Exception ee)
            //{
            //    ee.ConsoleOutput();
            //}
            //(sender as AsyncSocketServer).GetRemoteIPEndPoint(e.sessionId).Port.ConsoleOutput();
        }

        static void server_Disconnected(object sender, AsyncSocketSessionEventArgs e)
        {
            e.SessionIPEndPoint.ConsoleOutput(" disconnected!");
            (sender as AsyncSocketTcpServer).ConnectedSocketsCount.ConsoleOutput("Disconnected! There are {0} clients connected to the server");
            (sender as AsyncSocketTcpServer).PeakConnectedSocketsCount.ConsoleOutput("Peak: {0} clients connected to the server");
        }

        static void server_Connected(object sender, AsyncSocketSessionEventArgs e)
        {
            e.SessionIPEndPoint.ConsoleOutput(" connected.");
            (sender as AsyncSocketTcpServer).ConnectedSocketsCount.ConsoleOutput("Connected. There are {0} clients connected to the server");
        }

        private static void TestDevLibWinForms()
        {
            PrintMethodName("Test Dev.Lib.WinForms");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DemoForm());
            Application.Run(new NormalForm());
        }

        private static void TestDevLibUtilities()
        {
            PrintMethodName("Test Dev.Lib.Utilities");

            NetUtilities.GetRandomPortNumber().ConsoleOutput();
            StringUtilities.GetRandomAlphabetString(32).ConsoleOutput();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static void TestDevLibServiceModel()
        {
            PrintMethodName("Test Dev.Lib.ServiceModel");

            WcfClientUtilities.SaveGeneratedAssemblyFile = true;

            var testsrv0 = new WcfServiceHost(typeof(WcfTest), new[] { typeof(IWcfTest), typeof(IWcfAnotherTest) }, WcfBinding.BasicHttp, 6001, "WcfTest");

            testsrv0.Open();

            testsrv0.IgnoreMessageInspect = false;

            testsrv0.Restart();
            //var client0 = new DynamicClientProxyFactory("http://wsf.cdyne.com/WeatherWS/Weather.asmx", "client0.dll", true).GetPerSessionThrowableProxy();

            var client0 = WcfClientProxy<IWcfTest>.GetPerCallThrowableInstance(WcfBinding.BasicHttp, "127.0.0.1", 6001, "WcfTest");

            client0.AsIWcfClientBase().SendingRequest += (s, e) =>
            {
                try
                {
                    Console.WriteLine(((ClientBase<IWcfTest>)s).ClientCredentials.UserName.UserName);
                }
                catch
                {

                }
                Console.WriteLine(e.ClientCredentials.UserName.UserName);
            };

            client0.SetClientCredentials(c =>
            {
                c.UserName.UserName = "abc";
            });

            try
            {
                client0.MyOperation1("", 1);
                //client0.Call("GetCityForecastByZIP", "33133");
            }
            catch (Exception e)
            {

            }
            //client0.CallMethod("GetCityForecastByZIP", "");

            Console.ReadLine();

            client0.SetClientCredentials(c =>
            {
                c.UserName.UserName = "def";
            });

            try
            {
                client0.MyOperation1("", 1);
                //client0.Call(client0.Methods[1], "33133");
            }
            catch (Exception e)
            {

            }

            //client0.CallMethod("GetCityForecastByZIP", "");

            Console.ReadLine();
            //var client0 = WcfClientProxy<IWcfTest>.GetPerSessionUnthrowableInstance("http://127.0.0.1:6000/WcfTest");

            //var testsrv1 = new WcfServiceHost(new WcfTest("user defined"), new BasicHttpBinding(), "http://127.0.0.1:6000/WcfTest", false);
            //testsrv1.SetWebHttpBehaviorAction = i =>
            //{
            //    i.HelpEnabled = true;
            //    i.DefaultBodyStyle = WebMessageBodyStyle.WrappedRequest;
            //};
            //testsrv1.Open();
            //Console.ReadLine();
            ////string soapMsg = @"D:\soap.txt".ReadTextFile();

            //SoapClient soapClient = new SoapClient("http://wsf.cdyne.com/WeatherWS/Weather.asmx");

            ////string soapresult = soapClient.SendSoapRequestFile(@"D:\soap.txt").Content;
            //var factory = new WebServiceClientProxyFactory("http://wsf.cdyne.com/WeatherWS/Weather.asmx");

            //new WcfServiceHost(typeof(RoutingService), "DevLib.Samples.exe.config", null, true);

            //new WcfServiceHost(typeof(WcfTest), "DevLib.Samples.exe.config", "http://127.0.0.1:6000/WcfTest", true);

            var testsrv = new WcfServiceHost(typeof(WcfTest), new BasicHttpBinding(), "http://127.0.0.1:6000/WcfTest", false);
            //testsrv.ReceivingRequest += new EventHandler<WcfMessageInspectorEventArgs>(calcsvr_Receiving);
            //testsrv.SendingReply += new EventHandler<WcfMessageInspectorEventArgs>(calcsvr_Replying);
            //testsrv.ErrorOccurred += new EventHandler<WcfErrorEventArgs>(testsrv_ErrorOccurred);
            testsrv.SetDataContractResolverAction = i => i.DataContractResolver = new GenericDataContractResolver(new string[] { @"D:\Work\Temp\ClassLibrary2\ClassLibrary2\bin\Debug\ClassLibrary2.dll" });
            testsrv.Open();
            Console.ReadLine();

            var client = WcfClientProxy<IWcfTest>.GetClientBaseInstance("http://127.0.0.1:6000/WcfTest");

            WcfMessageInspectorEndpointBehavior bh = (WcfMessageInspectorEndpointBehavior)client.AsClientBase<IWcfTest>().AddEndpointBehavior(new WcfMessageInspectorEndpointBehavior());
            bh.ReceivingReply += (s, e) => Console.WriteLine(e);
            bh.SendingRequest += (s, e) => Console.WriteLine(e);
            //client.Foo("a");

            Console.ReadLine();

            client.AsClientBase<IWcfTest>().Close();

            try
            {
                client.Foo("a");
            }
            catch (Exception)
            {

            }

            client.AsIWcfClientBase().Close();

            client.Foo("a");

            client.Foo("a");

            Console.ReadLine();

            //new WcfServiceHost(typeof(WcfTest), typeof(BasicHttpBinding), "http://127.0.0.1:6000/WcfTest", true);

            //var client = WcfClientChannelFactory<IWcfTest>.CreateChannel(typeof(BasicHttpBinding), "http://127.0.0.1:6000/WcfTest", false);

            //WcfClientUtilities.SaveGeneratedAssemblyFile = true;

            var client1 = WcfClientProxy<IWcfTest>.GetPerSessionThrowableInstance(typeof(BasicHttpBinding), "http://127.0.0.1:6000/WcfTest");

            //client1.SetDataContractResolver(i => i.AddGenericDataContractResolver());

            //client1.GetWcfClientBase().SendingRequest += new EventHandler<WcfMessageInspectorEventArgs>(Program_SendingRequest);
            //client1.GetWcfClientBase().ReceivingReply += new EventHandler<WcfMessageInspectorEventArgs>(Program_ReceivingReply);
            //client2.SetClientCredentialsAction = (c) => { c.UserName.UserName = "a"; c.UserName.Password = "b"; };


            //client1.AddAnimal(new Dog());

            //client1.AddAnimal(new Tigar());

            //client1.AddAnimal(new Bird());

            //Console.ReadLine();

            //client1.Foo("");

            //clinet2.ClientCredentials
            string a = string.Empty;
            object b = new object();

            try
            {
                a = client1.MyOperation1("a", 1);
                //b = client1.Foo("aaa");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(a);
            Console.WriteLine(b);

            Console.ReadLine();

            ////WcfServiceHost host = WcfServiceHost.Create(@"C:\YuGuan\Document\DevLib\DevLib.Samples\bin\Debug\Service1.dll", @"C:\YuGuan\Document\DevLib\DevLib.Samples\bin\Debug\Service1.dll.config");
            ////host.CurrentAppDomain.FriendlyName.ConsoleOutput("AppDomain");

            //var a = WcfServiceType.LoadFile(@"E:\Temp\WcfCalc.dll")[0].GetInterfaces()[0];

            //var wcfclient = WcfClientBase<IWcfTest>.GetReusableFaultUnwrappingInstance();

            ////var client1 = WcfClientBase<IWcfService>.GetInstance("");
            ////var x = new BasicHttpBinding().RetrieveProperties().SerializeBinary();

            //WcfServiceHost host = new WcfServiceHost();
            //host.Initialize(@"E:\Temp\WcfCalc.dll", typeof(BasicHttpBinding), @"http://localhost:888/abcd");

            //host.Opened += (s, e) => (e as WcfServiceHostEventArgs).WcfServiceName.ConsoleOutput("|Opened");
            //host.Closed += (s, e) => (e as WcfServiceHostEventArgs).WcfServiceName.ConsoleOutput("|Closed");
            //host.Reloaded += (s, e) => s.ConsoleOutput();

            //host.Open();
            //Console.WriteLine("first open");



            //Console.ReadKey();

            //host.Close();
            //Console.WriteLine("first close");
            //host.Open();
            //Console.WriteLine("2 open");
            //host.Close();
            //Console.WriteLine("2 close");
            //host.Open();
            //Console.WriteLine("3 open");
            //host.Abort();
            //Console.WriteLine("Abort");
            //host.Open();
            //Console.WriteLine("4 open");
            ////host.Restart();
            //host.GetAppDomain().FriendlyName.ConsoleOutput("|AppDomain");
            ////host.GetStateList().Values.ToList().ForEach(p => p.ConsoleOutput());
            ////var a = host.GetStateList();
            //Console.ReadKey();

            //host.Unload();
            ////host.GetStateList().Values.ToList().ForEach(p => p.ConsoleOutput());
            //host.Unload();
            //host.Unload();
            //host.Reload();
            //host.Reload();
            ////host.GetStateList().Values.ToList().ForEach(p => p.ConsoleOutput());
            //host.Open();
            ////host.GetStateList().Values.ToList().ForEach(p => p.ConsoleOutput());
            //host.GetAppDomain().FriendlyName.ConsoleOutput("|after reload AppDomain");

            //Console.ReadKey();
            //host.Dispose();
        }

        static void testsrv_ErrorOccurred(object sender, WcfErrorEventArgs e)
        {
            Console.WriteLine(e);
        }

        static void Program_ReceivingReply(object sender, WcfMessageInspectorEventArgs e)
        {
            Console.WriteLine("Receiving Reply");
            Console.WriteLine(e.Message);
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Program_SendingRequest(object sender, WcfMessageInspectorEventArgs e)
        {
            Console.WriteLine("Sending Request");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.ClientCredentials.UserName.UserName);
            Console.WriteLine(e.ClientCredentials.UserName.Password);
            Console.WriteLine();
            Console.WriteLine();
        }

        static void calcsvr_Replying(object sender, WcfMessageInspectorEventArgs e)
        {
            Console.WriteLine("Sending Reply");
            Console.WriteLine(e);
            Console.WriteLine();
            Console.WriteLine();

        }

        static void calcsvr_Receiving(object sender, WcfMessageInspectorEventArgs e)
        {
            Console.WriteLine("Receiving Request");
            Console.WriteLine(e);
            Console.WriteLine();
            Console.WriteLine();

        }

        private static void TestDevLibConfiguration()
        {
            PrintMethodName("Test DevLib.Settings");


            IniEntry ini = IniManager.Open();

            ini["section1", "hello"] = 123;
            ini["section1", "hello1"] = 123;
            ini["section1", "hello2"] = 123;

            ini["section2", "hello3"] = 123;
            ini["section2", "hello4"] = 123;

            ini["section3", "hello5"] = 123;
            ini["section3", "hello6"] = 123;
            ini.Save();



            var form = new ConfigEditorForm();
            form.AddPlugin((filename) => { return ConfigManager.Open(filename).GetValue<TestConfig>("keyA"); }, (filename, obj) => { ConfigManager.Open(filename).SetValue("keyA", obj); ConfigManager.Open(filename).Save(); });
            form.AddPlugin((filename) => { return ConfigManager.Open(filename).GetValue<TestConfig>("keyA"); }, (filename, obj) => { ConfigManager.Open(filename).SetValue("keyA", obj); ConfigManager.Open(filename).Save(); }, "haha");

            //try
            //{
            //    form.OpenConfigFile(@"e:\d.xml");
            //}
            //catch
            //{
            //}
            Application.Run(form);


            Console.ReadKey();

            TestClass me = new TestClass() { Name = "Foo", Age = 29 };

            //Settings settings1 = SettingsManager.Open(Path.Combine(Environment.CurrentDirectory, "test3.xml"));
            //Settings settings2 = SettingsManager.Open(Path.Combine(Environment.CurrentDirectory, "test3.xml"));

            Hashtable a = new Hashtable();
            a.Add("hello", DateTime.Now);

            //settings1.SetValue("time0", a);
            //settings1.SetValue("time", DateTime.Now);
            //settings1.Remove("asdf");
            ////settings1.SetValue("time", DateTime.Now);
            ////settings1.SetValue("time", DateTime.Now);
            ////settings1.SetValue("txt1", "hello1");
            ////settings1.SetValue("color", (ConsoleColor)9);
            //settings1.SetValue("me", me);
            ////settings2.SetValue("time1", DateTime.Now);
            ////settings2.SetValue("time2", DateTime.Now);
            ////settings2.SetValue("time3", DateTime.Now);
            ////settings2.SetValue("txt2", "hello2");
            ////settings2.SetValue("color5", (ConsoleColor)15);
            ////settings2.SetValue("me1", me);
            //settings1.GetValue<DateTime>("time").ConsoleOutput();
            //settings1.GetValue<ConsoleColor>("color").ConsoleOutput();
            ////settings1.GetValue<TestClass>("me").Name.ConsoleOutput();
            ////settings1.GetValue<TestClass>("me").Age.ConsoleOutput();
            //settings1.GetValue<string>("hello2", "defalut").ConsoleOutput();
            //settings1.Save();
            //settings2.Save();
            //Dictionary<string, object> a = new Dictionary<string, object>();
            //IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
            //a["key1"] = 1;
            //a["key2"] = "hello string";
            //a["key3"] = DateTime.Now;
            //a["key4"] = me;

            //a.ForEach((k, v) => { Console.WriteLine(k.ToString() + v.ToString()); });

            List<string> alist = new List<string>() { "a", "b", "c" };
            List<TestClass> blist = new List<TestClass>() { me, me, me };

            Config config = ConfigManager.Open("zzzz.xml");
            config.SetValue("hello", "a");
            config.SetValue("hello", blist);
            config.SetValue("hello", "b");
            config.SetValue("hello2", Guid.NewGuid());
            config["g1"] = Guid.NewGuid();
            config.Save();
            config.Values.ForEach(p => p.ToString().ConsoleOutput());



            Settings setting = SettingsManager.Open("zzz.xml");
            Settings setting1 = SettingsManager.Open("zzz.xml");

            //setting["key1"] = 1;
            //setting["key2"] = "hello string";
            //setting["key3"] = DateTime.Now;
            //setting["key4"] = me;
            //setting["key5"] = alist;
            //setting["key6"] = blist;
            setting.Save();

            setting1["key3"] = DateTime.Now;
            setting1["key2"] = "hello string123";
            setting1["key4"] = Guid.NewGuid();
            setting1.Save();
            setting.Reload();
            setting.ConfigFile.ConsoleOutput();
            setting.Values.ForEach(p => p.ToString().ConsoleOutput());
        }

        private static void PrintMethodName(string name)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0} is running...", name);
            Console.ForegroundColor = originalForeColor;
        }

        private static void client_DataSent(object sender, AsyncSocketSessionEventArgs e)
        {

            Console.WriteLine();
        }

        private static void svr_DataReceived(object sender, AsyncSocketSessionEventArgs e)
        {

            //svr.Send(e.ConnectionId, e.TransferredRawData);
            Console.WriteLine();
        }

        private static void testEventClassObject_OnTestMe(object sender, EventArgs e)
        {
            (sender as TestClass).Name.ConsoleOutput();
        }
    }

    public class TestConfig
    {
        public TestConfig()
        {
            this.MySpell = new List<SpellingOptions>();
            this.MyString = Guid.NewGuid().ToString();
        }

        public int MyInt { get; set; }
        public string MyString { get; set; }

        [Editor(typeof(ConfigPropertyGridCollectionEditor), typeof(UITypeEditor))]
        public List<SpellingOptions> MySpell { get; set; }

        public override string ToString()
        {
            return this.MyString;
        }
    }

    [DataContract()]
    [Serializable]
    public class TestClass
    {
        public event EventHandler<EventArgs> OnTestMe;

        public Person APerson { get; set; }

        public SpellingOptions Spell { get; set; }

        [DataMember()]
        public string Name
        {
            get;
            set;
        }

        [DataMember()]
        public int Age
        {
            get;
            set;
        }

        public void TestMe()
        {
            Console.WriteLine("TestClass.TestMe() done!");
            OnTestMe.RaiseEvent(this, new EventArgs());
        }

        public string ShowAndExit()
        {
            try
            {
                return "TestClass.ShowAndExit";
            }
            finally
            {
                var p = Process.GetCurrentProcess();
            }
        }

        public int TestAdd(int a, int b)
        {
            return a + b;
        }
    }

    public interface IFoo<T>
    {
        [DataMember]
        string Name { get; set; }
    }

    public interface IFoo
    {
        [DataMember]
        string Name { get; set; }
    }

    public class Bar<T> : IFoo
    {
        public string Name { get; set; }
    }

    public class Bar1 : IFoo
    {
        public string Name { get; set; }
    }

    public interface IPerson
    {
        [DataMember]
        string FirstName { get; set; }

        [DataMember]
        string LastName { get; set; }
    }

    [Serializable]
    public class Person : MarshalByRefObject, IPerson
    {
        public event EventHandler<ErrorEventArgs> Error;

        public string DoTryf()
        {
            Error.RaiseEvent(this);

            try
            {
                try
                {
                    return "done";
                }
                finally
                {
                    Console.WriteLine("inside finally");
                }
            }
            finally
            {
                Console.WriteLine("outside finally");
            }
        }


        [DataMember]
        public SpellingOptions Foo { get; set; }

        public Person()
        {
            MyProperty = new List<FooBar>();
        }

        public List<FooBar> MyProperty { get; set; }

        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public int ID { get; set; }

        public Person(string newfName, string newLName, int newID)
        {
            FirstName = newfName;
            LastName = newLName;
            ID = newID;
        }

        public void ShowName()
        {
            Console.WriteLine("Normal Method");
        }

        public void ShowName(string a)
        {
            Console.WriteLine("Normal Method a");
        }

        public void ShowName<T>()
        {
            Console.WriteLine("Generic Method");
        }

        public void ShowName<T>(string a)
        {
            Console.WriteLine("Generic Method a");
        }

        public void ShowName<T0, T1>()
        {
            Console.WriteLine("Generic Method");
        }

        public override string ToString()
        {
            return string.Format("FirstName= {0} LastName= {1} Id= {2}", this.FirstName, this.LastName, this.ID);
        }

        public Person Get()
        {
            return this;
        }
    }

    [Serializable]
    public class FooBar : MarshalByRefObject
    {
        [Parameter(Required = true)]
        public string foo { get; set; }

        [Parameter("b", "bar", "barr", Required = false, DefaultValue = 123)]
        public int bar { get; set; }

        [Parameter("o", "ok", "okk", DefaultValue = true, Required = true)]
        public bool IsOk { get; set; }

        public bool IsOk2 { get; set; }
    }

    public class Company
    {
        public Boss[] Bosses { get; set; }
    }

    public class Boss
    {
        public string Name { get; set; }
        public Address Home { get; set; }
    }

    public class Address
    {
        public int PostCode { get; set; }
        public string Street { get; set; }
    }

    [DataContract]
    [Serializable]
    public class SpellingOptions
    {
        public SpellingOptions()
        {
            this.SpellCheckCAPS = false;
            this.SpellCheckWhileTyping = true;
            this.SuggestCorrections = true;
        }

        [DataMember]
        public bool SpellCheckWhileTyping
        { get; set; }

        [DataMember]
        public bool SpellCheckCAPS
        { get; set; }

        [DataMember]
        public bool SuggestCorrections
        { get; set; }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}", this.SpellCheckCAPS, this.SpellCheckWhileTyping, this.SuggestCorrections).GetHashCode();
        }


    }

    public class MyDatabaseQueue<T> : IProducerConsumerQueue<T>
    {
        public MyDatabaseQueue()
        {
            // open connection to database.
        }

        public void Enqueue(T item)
        {
            // insert item to databse.
        }

        public long Enqueue(IEnumerable<T> items)
        {
            // insert items to databse, and return the number of items be inserted.
            return items.LongCount();
        }

        public T Dequeue()
        {
            // query one item and remove from database.
            throw new NotImplementedException();
        }

        public T Peek()
        {
            // query one item and keep it from database.
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            // query item is in database or not.
            throw new NotImplementedException();
        }

        public long Count()
        {
            // query total rows in database.
            throw new NotImplementedException();
        }

        public void Enqueue(object item)
        {
            // insert item to databse.
        }

        public long Enqueue(IEnumerable items)
        {
            // insert items to databse, and return the number of items be inserted.
            throw new NotImplementedException();
        }

        object IProducerConsumerQueue.Dequeue()
        {
            // query one item and remove from database.
            throw new NotImplementedException();
        }

        object IProducerConsumerQueue.Peek()
        {
            // query one item and keep it from database.
            throw new NotImplementedException();
        }

        public bool Contains(object item)
        {
            // query item is in database or not.
            throw new NotImplementedException();
        }

        public void Clear()
        {
            // remove all rows in database.
            throw new NotImplementedException();
        }
    }


}
