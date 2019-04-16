using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ToolBox.Connections.Databases;

namespace InterfaceMH.ViewModels
{
    interface ITabVM
    {
        /// <summary>
        /// Fast Fourier Transform
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        double[] FFT(double[] data);
        /// <summary>
        /// Record the micro
        /// </summary>
        void Record();
        /// <summary>
        /// Add accord in the json
        /// </summary>
        void AddAccord();
        /// <summary>
        /// Recupère les info du json
        /// </summary>
        /// <returns></returns>
        string LoadAccord();
        /// <summary>
        /// stop le record
        /// </summary>
        void Stop();
        /// <summary>
        /// vérifie si il ne record pas déjà
        /// </summary>
        /// <returns>bool</returns>
        bool CanRec();
        /// <summary>
        /// vérifie si il record
        /// </summary>
        /// <returns></returns>
        bool CanStop();
        /// <summary>
        /// défini un timer (miliseconde)
        /// </summary>
        void SetTimer();
        /// <summary>
        /// fonction effectué à chaque tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTimerEvent(object sender, ElapsedEventArgs e);

        /// <summary>
        /// les mêmes fonctions précédentes mais adapté au fichier MIDI
        /// </summary>
        void RecordMidi();
        void AddAccordWithMidi(NoteEvent sender);
        void OnTimerEventWithMidi(object sender, ElapsedEventArgs e);
        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e);
        void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e);
    }
}
