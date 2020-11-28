// Decompiled with JetBrains decompiler
// Type: ActivityBrowser.BrowserForm
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

using ControlLib;
using Heffsoft.Piggyback3.ActivationLib;
using ModuleInterface;
using ModuleInterface.V1;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using Utility;
using vbAccelerator.Components.Shell;

namespace ActivityBrowser
{
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public partial class BrowserForm : Window, IComponentConnector
    {
        private const string HashSalt = "OJWaVS_X44/^!zGyDD@GARI/EQm8[R$60mx7ihEN8v}O0tJqxC=5*^xpbCQ0}RO1/]b#JwR3(8^GJfSUYC0+t2,Z=\\zx;SqO$Bap@q+zTca,#c2Ek>|GD_[ezzmn5Y";
        private ModuleHost moduleHost;
        private ActivationSystem actSystem;
        private DispatcherTimer regTimer;
        private Control currentActivityControl;
        private BitmapImage documentIcon;
        private BitmapImage computerIcon;
        private BitmapImage folderIcon;
        private bool simulatedCrash;
        private XpsDocument aboutPage;
        private XpsDocument manualPage;
        private EnhancedSoundPlayer beatSound;
        private DispatcherTimer beatTimer;
        private byte[] ScrubPattern = new byte[4]
        {
      byte.MaxValue,
      (byte) 170,
      (byte) 85,
      (byte) 0
        };

        public BrowserForm()
        {
            Thread.CurrentThread.Name = "Interface";
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Setting unhandled exception handler");
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Loading plug-in modules");
            this.moduleHost = new ModuleHost(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Data\\Modules");
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Starting activation system");
            this.actSystem = new ActivationSystem();
            this.actSystem.WriteLog += new ActivationSystem.WriteLogDelegate(this.actSystem_WriteLog);
            this.actSystem.InitActivationSystem();
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Initialising Form..");
            this.InitializeComponent();
            this.currentActivityControl = (Control)null;
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Loading beat sound");
            this.beatSound = ModuleUtils.GetBeatPlayer();
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Creating beat timer");
            this.beatTimer = new DispatcherTimer();
            this.beatTimer.Tick += new EventHandler(this.beatTimer_Tick);
            this.beatTimer.IsEnabled = false;
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Loading form images");
            this.documentIcon = this.LoadImageFromResource("Images/Document.png");
            this.computerIcon = this.LoadImageFromResource("Images/Computer.png");
            this.folderIcon = this.LoadImageFromResource("Images/Folder.png");
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Loading form XPS documents");
            this.aboutPage = ResourceLoader.loader.LoadXPF("ActivityBrowser.XpsFiles.About.xps");
            this.manualPage = ResourceLoader.loader.LoadXPF("ActivityBrowser.XpsFiles.Manual.xps");
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Populating menus");
            this.ActivityPanel.IsEnabled = false;
            this.ActivityPanel.Children.Clear();
            try
            {
                foreach (IModule_V1 module in (IEnumerable<IModule_V1>)this.moduleHost.Modules)
                {
                    Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Creating accordion control for " + module.Name);
                    Expander expander = new Expander();
                    expander.Header = (object)module.Name;
                    expander.ToolTip = (object)module.Description;
                    TreeView treeView = new TreeView();
                    treeView.ItemTemplate = (DataTemplate)this.Resources[(object)"ExerciseTemplate"];
                    treeView.HorizontalAlignment = HorizontalAlignment.Stretch;
                    treeView.VerticalAlignment = VerticalAlignment.Stretch;
                    treeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.ModuleTree_SelectedItemChanged);
                    expander.Content = (object)treeView;
                    List<ExerciseItem> exerciseItemList1 = new List<ExerciseItem>();
                    foreach (ILevel_V1 level in module.Levels)
                    {
                        Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Creating TreeView level node for \"" + module.Name + "\".\"" + level.Name + "\"");
                        List<ExerciseItem> exerciseItemList2 = new List<ExerciseItem>();
                        foreach (IExercise_V1 exercise in level.Exercises)
                        {
                            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Creating TreeView exercise node for \"" + module.Name + "\".\"" + level.Name + "\".\"" + exercise.Name + "\"");
                            ExerciseItem exerciseItem = new ExerciseItem(exercise.Name, exercise.Description, exercise.Activity != null ? this.computerIcon : this.documentIcon, (object)exercise, new ExerciseItem[0]);
                            exerciseItemList2.Add(exerciseItem);
                        }
                        ExerciseItem exerciseItem1 = new ExerciseItem(level.Name, level.Name, this.folderIcon, (object)null, exerciseItemList2.ToArray());
                        exerciseItemList2.Clear();
                        exerciseItemList1.Add(exerciseItem1);
                    }
                    treeView.ItemsSource = (IEnumerable)exerciseItemList1.ToArray();
                    this.ActivityPanel.Children.Add((UIElement)expander);
                }
                this.ActivityPanel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.Error, "BrowserForm.BrowserForm", ex.ToString());
            }
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Displaying about document as default page");
            this.aboutMenu_Click((object)this, (RoutedEventArgs)null);
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Starting timer to check activation status");
            this.regTimer = new DispatcherTimer();
            this.regTimer.Tick += new EventHandler(this.RegTimer_Tick);
            this.regTimer.Interval = new TimeSpan(0, 0, 1);
            this.regTimer.Start();
            Log.Write(LogLevel.Information, "BrowserForm.BrowserForm", "Form initilisation complete!");
        }

        private void actSystem_WriteLog(
          ASLogLevel level,
          string moduleName,
          string className,
          string logMessage)
        {
            Log.Write((LogLevel)level, moduleName, className, logMessage);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (!this.simulatedCrash)
                    Log.Write(LogLevel.Warning, nameof(BrowserForm), "Caught Unhandled Exception");
                else
                    Log.Write(LogLevel.Information, nameof(BrowserForm), "Processing Debug Dump");
                string path = Path.GetTempPath() + Guid.NewGuid().ToString() + "\\";
                Directory.CreateDirectory(path);
                MiniDump.TryDump(path + "minidump.dmp", MiniDumpType.Normal);
                Exception exceptionObject = (Exception)e.ExceptionObject;
                File.WriteAllText(path + "exception.txt", exceptionObject.ToString(), Encoding.UTF8);
                Log.DumpLog(path + "programlog.gz");
                BitmapSource source = CaptureScreenshot.Capture(new HandleRef((object)this, new WindowInteropHelper((Window)this).Handle));
                if (source != null)
                {
                    using (FileStream fileStream = new FileStream(path + "screenshot.png", FileMode.Create))
                    {
                        BitmapEncoder bitmapEncoder = (BitmapEncoder)new PngBitmapEncoder();
                        bitmapEncoder.Frames.Add(BitmapFrame.Create(source));
                        bitmapEncoder.Save((Stream)fileStream);
                    }
                }
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (path.EndsWith("\\"))
                    path = path.Substring(0, path.Length - 1);
                string fileName = directoryName + "\\ErrorReporter.exe";
                string arguments = "-reportDir \"" + path + "\"";
                if (this.simulatedCrash)
                    arguments += " -simCrash";
                Process.Start(fileName, arguments);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private void ModuleTree_SelectedItemChanged(
          object sender,
          RoutedPropertyChangedEventArgs<object> e)
        {
            if (!((sender as TreeView).SelectedItem is ExerciseItem selectedItem) || selectedItem.Tag == null)
                return;
            this.ExerciseSelected((IExercise_V1)selectedItem.Tag);
        }

        private BitmapImage LoadImageFromResource(string resourceName)
        {
            Log.Write(LogLevel.Information, "BrowserForm.LoadImageFromResource", "Loading Image: " + resourceName);
            try
            {
                Stream stream = Application.GetResourceStream(new Uri("/Piggyback3;component/" + resourceName, UriKind.Relative)).Stream;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.Error, "BrowserForm.LoadImageFromResource", "Failed to load image file! (" + resourceName + ")");
                throw;
            }
        }

        private void RegTimer_Tick(object sender, EventArgs e)
        {
            if (!this.actSystem.Initialised)
                return;
            if (true || this.actSystem.IsAuthorised)
            {
                Log.Write(LogLevel.Information, "BrowserForm.RegTimer_Tick", "Program activated, allowing access to content.");
                this.ActivityPanel.IsEnabled = true;
                this.regTimer.Stop();
            }
            else
            {
                this.regTimer.Stop();
                Log.Write(LogLevel.Information, "BrowserForm.RegTimer_Tick", "Program not activated, prompting for activation.");
                if (MessageBox.Show("Piggyback has not been activated. Would you like to activate online now?", "Piggyback Activation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;
                Log.Write(LogLevel.Information, "BrowserForm.RegTimer_Tick", "Showing activation window");
                this.ActivateItem_Click((object)this.activateMenu, (RoutedEventArgs)null);
            }
        }

        private void ExerciseSelected(IExercise_V1 exercise)
        {
            Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Selected exercise '" + exercise.Name + "'");
            Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Disabling Beat");
            ModuleUtils.SetCheckMarks(this.beatMenu, this.offBeatMenuItem);
            this.beatTimer.IsEnabled = false;
            this.InstructionTab.IsSelected = true;
            XpsDocument instructionSheet = exercise.InstructionSheet;
            if (instructionSheet != null)
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Displaying instruction sheet.");
                this.InstructionViewer.Document = (IDocumentPaginatorSource)instructionSheet.GetFixedDocumentSequence();
                this.InstructionViewer.IsEnabled = true;
            }
            else
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "No instruction sheet, disabling");
                this.InstructionViewer.Document = (IDocumentPaginatorSource)null;
                this.InstructionViewer.IsEnabled = false;
            }
            XpsDocument recordingSheet = exercise.RecordingSheet;
            if (recordingSheet != null)
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Displaying recording sheet.");
                this.RecordingViewer.Document = (IDocumentPaginatorSource)recordingSheet.GetFixedDocumentSequence();
                this.RecordingViewer.IsEnabled = true;
                this.RecordingTab.IsEnabled = true;
            }
            else
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "No recording sheet, disabling");
                this.RecordingViewer.Document = (IDocumentPaginatorSource)null;
                this.RecordingViewer.IsEnabled = false;
                this.RecordingTab.IsEnabled = false;
            }
            if (this.currentActivityControl != null)
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Clearing previous activity.");
                if (this.currentActivityControl is IActivityControl_V1 currentActivityControl)
                {
                    Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Previous activity supports resetting. So resetting");
                    currentActivityControl.Reset();
                }
                this.currentActivityControl = (Control)null;
            }
            if (exercise.Activity != null)
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "Displaying activity.");
                this.ActivityGrid.Children.Clear();
                this.ActivityGrid.Children.Add((UIElement)exercise.Activity);
                this.ActivityTab.IsEnabled = true;
                this.currentActivityControl = exercise.Activity;
            }
            else
            {
                Log.Write(LogLevel.Information, "BrowserForm.ExerciseSelected", "No activity, disabling.");
                this.ActivityGrid.Children.Clear();
                this.ActivityTab.IsEnabled = false;
            }
        }

        private void ThemeItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            MenuItem menuItem = (MenuItem)sender;
            string tag = (string)menuItem.Tag;
            foreach (object obj in (IEnumerable)this.ThemeMenu.Items)
            {
                if (obj.GetType() == typeof(MenuItem))
                    ((MenuItem)obj).IsChecked = false;
            }
            menuItem.IsChecked = true;
            if (string.IsNullOrEmpty(tag))
                return;
            try
            {
                Log.Write(LogLevel.Information, "BrowserForm.ThemeItem_Click", "Setting skin: " + tag);
                resourceDictionary.Source = new Uri("Themes/" + tag + ".xaml", UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.Warning, "BrowserForm.ThemeItem_Click", "There was an error setting the theme. Aborted");
            }
        }

        private void ActivateItem_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as MenuItem).Tag as string;
            Log.Write(LogLevel.Information, "BrowserForm.ActivateItem_Click", "User started activation process");
            if (this.actSystem.RegisterApplication(!tag.Equals("1")))
            {
                Log.Write(LogLevel.Information, "BrowserForm.ActivateItem_Click", "Activation was successful. Showing successful message");
                int num = (int)MessageBox.Show("Activation was successful. Thank You!", "Activation", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.ActivityPanel.IsEnabled = true;
            }
            else
            {
                Log.Write(LogLevel.Information, "BrowserForm.ActivateItem_Click", "Activation failed. Showing failure message");
                int num = (int)MessageBox.Show("There was a problem processing the activation or the activation was cancelled. Please try again and verify your details are correct.", "Activation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Log.Write(LogLevel.Information, "BrowserForm.ExitItem_Click", "User selected exit menu");
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Log.Write(LogLevel.Information, "BrowserForm.Window_Closing", "Browser window is closing");
            this.regTimer.Stop();
            while (!this.actSystem.Initialised)
                Thread.Sleep(10);
            Log.CloseLog();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12 && Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                Log.Write(LogLevel.Information, "BrowserForm.Window_PreviewKeyDown", "User pressed debug dump key combination");
                this.simulatedCrash = true;
                this.CurrentDomain_UnhandledException((object)this, new UnhandledExceptionEventArgs((object)new Exception("Simulated Exception"), false));
            }
            if (e.Key != Key.F11 || Keyboard.Modifiers != (ModifierKeys.Control | ModifierKeys.Shift))
                return;
            if (DateTime.Now.Month == 12 && DateTime.Now.Day == 5)
            {
                Log.Write(LogLevel.Information, "BrowserForm.Window_PreviewKeyDown", "SECRET FOUND!!");
                new SecretAbout().ShowDialog();
            }
            else
            {
                Log.Write(LogLevel.Information, "BrowserForm.Window_PreviewKeyDown", "User pressed diagnostic code key combination");
                DiagnosticDialog diagnosticDialog = new DiagnosticDialog();
                bool? nullable = diagnosticDialog.ShowDialog();
                if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
                    return;
                switch (this.HashString(diagnosticDialog.DiagnosticCode.ToUpper()).ToLower())
                {
                    case "441ea6877b60d4e03281c34ecd0dd736edd5d195":
                        this.CreateLink();
                        break;
                }
            }
        }

        public void RC4(ref byte[] bytes, byte[] key)
        {
            byte[] numArray1 = new byte[256];
            byte[] numArray2 = new byte[256];
            for (int index = 0; index < 256; ++index)
            {
                numArray1[index] = (byte)index;
                numArray2[index] = key[index % key.GetLength(0)];
            }
            int index1 = 0;
            for (int index2 = 0; index2 < 256; ++index2)
            {
                index1 = (index1 + (int)numArray1[index2] + (int)numArray2[index2]) % 256;
                byte num = numArray1[index2];
                numArray1[index2] = numArray1[index1];
                numArray1[index1] = num;
            }
            int index3;
            int index4 = index3 = 0;
            for (int index2 = 0; index2 < bytes.GetLength(0); ++index2)
            {
                index4 = (index4 + 1) % 256;
                index3 = (index3 + (int)numArray1[index4]) % 256;
                byte num = numArray1[index4];
                numArray1[index4] = numArray1[index3];
                numArray1[index3] = num;
                int index5 = ((int)numArray1[index4] + (int)numArray1[index3]) % 256;
                bytes[index2] ^= numArray1[index5];
            }
        }

        private void CreateLink()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Piggyback 3.lnk";
            if (File.Exists(path))
                File.Delete(path);
            new ShellLink()
            {
                Arguments = "-forceBypassMode",
                Target = Assembly.GetEntryAssembly().Location,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                Description = "Piggyback 3",
                DisplayMode = ShellLink.LinkDisplayMode.edmNormal
            }.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Piggyback 3.lnk");
        }

        private string HashString(string toHash)
        {
            SHA1 shA1 = (SHA1)new SHA1Managed();
            byte[] bytes = Encoding.UTF8.GetBytes("OJWaVS_X44/^!zGyDD@GARI/EQm8[R$60mx7ihEN8v}O0tJqxC=5*^xpbCQ0}RO1/]b#JwR3(8^GJfSUYC0+t2,Z=\\zx;SqO$Bap@q+zTca,#c2Ek>|GD_[ezzmn5Y" + toHash);
            byte[] hash = shA1.ComputeHash(bytes);
            shA1.Dispose();
            for (int index = 0; index < bytes.Length; ++index)
            {
                foreach (byte num in this.ScrubPattern)
                    bytes[index] = num;
            }
            string hex = this.ByteArrayToHex(hash);
            for (int index = 0; index < hash.Length; ++index)
            {
                foreach (byte num in this.ScrubPattern)
                    hash[index] = num;
            }
            return hex;
        }

        private string ByteArrayToHex(byte[] byteArray)
        {
            char[] chArray = new char[byteArray.Length * 2];
            for (int index = 0; index < byteArray.Length; ++index)
            {
                byte num1 = (byte)((uint)byteArray[index] >> 4);
                chArray[index * 2] = num1 > (byte)9 ? (char)((int)num1 + 55) : (char)((int)num1 + 48);
                byte num2 = (byte)((uint)byteArray[index] & 15U);
                chArray[index * 2 + 1] = num2 > (byte)9 ? (char)((int)num2 + 55) : (char)((int)num2 + 48);
            }
            return new string(chArray);
        }

        private void aboutMenu_Click(object sender, RoutedEventArgs e)
        {
            Log.Write(LogLevel.Information, "BrowserForm.aboutMenu_Click", "User accessed about menu. Show about document");
            this.InstructionViewer.Document = (IDocumentPaginatorSource)this.aboutPage.GetFixedDocumentSequence();
            this.InstructionViewer.IsEnabled = true;
            this.InstructionTab.IsEnabled = true;
            this.ActivityTab.IsEnabled = false;
            this.RecordingTab.IsEnabled = false;
            this.InstructionTab.Focus();
        }

        private void manualMenu_Click(object sender, RoutedEventArgs e)
        {
            Log.Write(LogLevel.Information, "BrowserForm.manualMenu_Click", "User accessed manual menu. Show manual document");
            this.InstructionViewer.Document = (IDocumentPaginatorSource)this.manualPage.GetFixedDocumentSequence();
            this.InstructionViewer.IsEnabled = true;
            this.InstructionTab.IsEnabled = true;
            this.ActivityTab.IsEnabled = false;
            this.RecordingTab.IsEnabled = false;
            this.InstructionTab.Focus();
        }

        private void beatMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Log.Write(LogLevel.Information, "BrowserForm.beatMenuItem_Click", "User accessed beat menu");
            MenuItem menuItem = sender as MenuItem;
            ModuleUtils.SetCheckMarks(this.beatMenu, menuItem);
            int menuTagInt32 = ModuleUtils.ParseMenuTagInt32(menuItem);
            if (menuTagInt32 == 0)
            {
                Log.Write(LogLevel.Information, "BrowserForm.beatMenuItem_Click", "User stopped beat");
                this.beatTimer.IsEnabled = false;
            }
            else
            {
                Log.Write(LogLevel.Information, "BrowserForm.beatMenuItem_Click", string.Format("User set beat to {0}ms", (object)menuTagInt32));
                this.beatTimer.Interval = new TimeSpan(0, 0, 0, 0, menuTagInt32);
                this.beatTimer.IsEnabled = true;
            }
        }

        private void beatTimer_Tick(object sender, EventArgs e) => this.beatSound.Play();
    }
}
