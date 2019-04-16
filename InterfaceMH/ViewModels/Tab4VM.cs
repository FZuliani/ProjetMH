using DataLibrary;
using InterfaceMH.Service;
using InterfaceMH.Views;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using ToolBox.Patterns.Locator;
using ToolBoxMVVM.Commands;
using T = ToolBox.Connections.Databases;
using API_MH.Models;
using System.Collections.ObjectModel;
using NAudio.Midi;

namespace InterfaceMH.ViewModels
{

    public class Tab4VM : ITabVM
    {
        #region Rythme

        private string _Bpm;
        public string Bpm
        {
            get { return _Bpm; }
            set
            {
                _Bpm = value;
                
            }
        }

       private static Timer timer;  

        #endregion

        #region Variable De Record

        private string _Accord;
        private ObservableCollection<Corde> _cordes;

        public ObservableCollection<Corde> cordes
        {
            get { return _cordes; }
            set
            {
                _cordes = value;
               
            }
        }
        
        private WaveIn wi;
        public BufferedWaveProvider bwp;
        private object _lock;
        private int RATE = 44100; // sample rate of the sound card
        private int BUFFERSIZE = (int)Math.Pow(2, 13); // must be a multiple of 2 
        private bool _Onair;

        
        public bool OnAir
        {
            get
            {
                return _Onair;
            }
            set
            {
                _Onair = value;
                StopCommand.RaiseCanExecuteChanged();
                RecCommand.RaiseCanExecuteChanged();
            }
        }
        
        #endregion

        #region Variable Fichier Midi


        private MidiIn midiInput;
        

        #endregion

        #region Command


        private Command _RecCommand;
        public Command RecCommand
        {
            get
            {
                return _RecCommand = _RecCommand ?? new Command(Record, CanRec);
            }
        }

        private Command _StopCommand;

        public Command StopCommand
        {
            get
            {
                return _StopCommand = _StopCommand ?? new Command(Stop, CanStop);
            }
        }
        #endregion

        #region Variable d'affichage

        public string Offset
        {
            get
            {
                return App.serviceLocator.Container.GetInstance<IServiceStorage>().Offset;
            }
            set
            {
                App.serviceLocator.Container.GetInstance<IServiceStorage>().Offset = value;
            }
        }


        public string accord
        {
            get { return _Accord; }
            private set { _Accord = value; }
        }

        private string[] Frettes
        {
            get; set;
        }
        #endregion

        #region Constructeur

        public Tab4VM()
        {
            OnAir = false;
            Bpm = "";
            Offset = " ";
            _lock = Offset;

            cordes = new ObservableCollection<Corde>();

            cordes.Add(new Corde { corde = "mi", frette= ""});
            cordes.Add(new Corde { corde = "Si", frette = "" });
            cordes.Add(new Corde { corde = "Sol", frette = "" });
            cordes.Add(new Corde { corde = "Re", frette = "" });
            cordes.Add(new Corde { corde = "La", frette = "" });
            cordes.Add(new Corde { corde = "Mi", frette = "" });
        }

        #endregion

        #region Fonction d'ajout/recup du fichier

        
        public void AddAccord() //Ajoute un accord dans le fichier json
        {
            lock (_lock)
            {
                int frameSize = BUFFERSIZE;
                var frames = new byte[BUFFERSIZE];
                bwp.Read(frames, 0, frameSize);

                int SAMPLE_RESOLUTION = 16;
                int BYTES_PER_POINT = SAMPLE_RESOLUTION / 8;
                Int32[] vals = new Int32[frames.Length / BYTES_PER_POINT];
                double[] Ys = new double[frames.Length / BYTES_PER_POINT];
                double[] Ys2 = new double[frames.Length / BYTES_PER_POINT];
                
                for (int i = 0; i < vals.Length; i++)
                {
                    if(frames[i] != 0)
                    {
                        // bit shift the byte buffer into the right variable format
                        byte hByte = frames[i * 2 + 1];
                        byte lByte = frames[i * 2 + 0];
                        vals[i] = (short)((hByte << 8) | lByte);
                        Ys[i] = vals[i];
                    }
                }

                Ys2 = FFT(Ys);
                
                foreach (double frequence in Ys)
                {
                    //accord = await App.serviceLocator.Container.GetInstance<IAccordServices>().Get(frequence);
                    if (frequence != 0)
                    {
                        T.Command command = new T.Command("SELECT [Accord] FROM dbo.Accord WHERE Note = @frequence");
                        command.AddParameter("frequence", frequence);
                        string accord = (string)ResourceLocator.Instance.connection.ExecuteScalar(command);

                        if (!(accord is null))
                        {
                            Offset += accord + '|';
                        }
                    }
                }
            }
        }
        
        public string LoadAccord() //Récupère les accords du fichier json au format " {accord}|"
        {
            lock (_lock)
            {
                return App.serviceLocator.Container.GetInstance<IServiceStorage>().Offset;
            }
        }

        #endregion

        #region Transformation de Fourier FFT(double[])

        public double[] FFT(double[] data)
        {
            double[] fft = new double[data.Length]; // this is where we will store the output (fft)
            Complex[] fftComplex = new Complex[data.Length]; // the FFT function requires complex format

            for (int i = 0; i < data.Length; i++)
            {
                fftComplex[i] = new Complex(data[i], 0.0); // make it complex format (imaginary = 0)
            }

            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
            {
                fft[i] = fftComplex[i].Magnitude; // back to double

            }
            return fft;
        }
        #endregion

        #region Record/Stop

        public void Record() // Command qui lance le record 
        {
            OnAir = true;
           
            wi = new WaveIn
            {
                DeviceNumber = 0,
                WaveFormat = new NAudio.Wave.WaveFormat(RATE, 1),
                BufferMilliseconds = 100
            };

            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;

            bwp.DiscardOnBufferOverflow = true;
            // create a wave buffer and start the recording
            wi.StartRecording();
            SetTimer();
        }


        public void Stop() //Command qui stop le record
        {
            timer.Enabled = false;
            wi.StopRecording();
            OnAir = false;
            Bpm = string.Empty;
        }

        //Active ou désactive le record
        public bool CanRec()
        {
            return (OnAir == false);
        }

        public bool CanStop()
        {
            return OnAir;
        }

        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        #endregion

        #region Start Timer

        public void SetTimer()
        {
            timer = new Timer(100);

            timer.Elapsed += OnTimerEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        #endregion

        #region Fonction à chaque tick
        
        public void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            AddAccord();
            //AddAccordWithMidi();
            string accords = LoadAccord();
            
            if (accords != " ")
            {
                accords.Remove(0, 2);
                Frettes = accords.Split('|');
                
                
                foreach (string accord in Frettes)
                {
                    
                    string[] Cordes = accord.Split(',');
                    if (Cordes.Length == 6)
                    {
                        if (Cordes[0].Length == 3)
                        {
                            cordes[0].frette += Cordes[0].ToString()[2] + "--";
                            cordes[1].frette += Cordes[1].ToString() + "--";
                            cordes[2].frette += Cordes[2].ToString() + "--";
                            cordes[3].frette += Cordes[3].ToString() + "--";
                            cordes[4].frette += Cordes[4].ToString() + "--";
                            cordes[5].frette += Cordes[5].ToString()[0] + "--";
                        }

                        else
                        {
                            cordes[0].frette += Cordes[0].ToString()[1] + "--";
                            cordes[1].frette += Cordes[1].ToString() + "--";
                            cordes[2].frette += Cordes[2].ToString() + "--";
                            cordes[3].frette += Cordes[3].ToString() + "--";
                            cordes[4].frette += Cordes[4].ToString() + "--";
                            cordes[5].frette += Cordes[5].ToString()[0] + "--";
                        }
                    }
                }
            }
        }

        #endregion

        #region Version MIDI

        public void RecordMidi()
        {
            OnAir = true;
            int nbDevices =  MidiIn.NumberOfDevices;
            midiInput = new MidiIn(0);
            midiInput.MessageReceived += midiIn_MessageReceived;
            midiInput.ErrorReceived += midiIn_ErrorReceived;
            midiInput.Start();
            SetTimer();
        }

        //récupère le fichier MIDI et le compare avec la DB
        public void AddAccordWithMidi(NoteEvent sender)
        {
            lock (_lock)
            {
                T.Command command = new T.Command("SELECT * FROM MidiNotes WHERE NoteNumber = @NoteNumber");
                command.AddParameter("NoteNumber", sender.NoteNumber);
                var Frette = ResourceLocator.Instance.connection.ExecuteReader(command, e => new { frette = (int)e["frette"], corde = (int)e["corde"] });

                if (Frette != null)
                {
                    Offset += Frette.FirstOrDefault().frette.ToString()+ "-" + Frette.FirstOrDefault().corde.ToString() + "|";
                }
            }
        }

        //exécuté à chaque tick 
        public void OnTimerEventWithMidi(object sender, ElapsedEventArgs e)
        {
            string accords = LoadAccord();

            if (accords != " ")
            {
                accords.Remove(0, 2);
                Frettes = accords.Split('|');

                foreach (string accord in Frettes)
                {
                    string[] Cordes = accord.Split('-');
                    cordes[int.Parse(Cordes[1])].frette = Cordes[0];
                }
            }
        }

        public void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            
        }

        //executé à chaque fois que le logiciel récupère un fichier MIDI
        public void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            AddAccordWithMidi((NoteEvent)e.MidiEvent);
        }
        
        #endregion
    }
}
