﻿// Accord (Experimental) Audio Library
// The Accord.NET Framework
// http://accord.googlecode.com
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Audio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Timers;

    /// <summary>
    ///   Virtual Metronome.
    /// </summary>
    /// 
    /// <remarks>
    ///   Objects from this class acts as virtual metronomes. If connected
    ///   to a beat detector, it can be used to determine the tempo (in
    ///   beats per minute) of a signal. It can also be used in manual mode
    ///   by calling <see cref="Tap"/> method. For more details, see the
    ///   Beat detection sample application which comes together with the
    ///   framework.
    /// </remarks>
    /// 
    public sealed class Metronome : IDisposable
    {
        private int taps;
        private List<TimeSpan> timeSpan;
        private DateTime lastTick;
        private Timer timeUp;
        private Timer metronome;

        /// <summary>
        ///   Gets or sets the Beats per Minute for this metronome.
        /// </summary>
        /// 
        public int BeatsPerMinute
        {
            get { return (int)((1000.0 * 60.0) / metronome.Interval); }
            set { metronome.Interval = (60.0 * 1000.0) / value; }
        }

        /// <summary>
        ///   Gets whether the metronome is currently detecting the tempo being tapped.
        /// </summary>
        /// 
        public bool Detecting
        {
            get { return timeUp.Enabled; }
        }

        /// <summary>
        ///   Fired when the metronome has figured the tapped tempo.
        /// </summary>
        /// 
        public event EventHandler TempoDetected;

        /// <summary>
        ///   Metronome tick.
        /// </summary>
        /// 
        public event ElapsedEventHandler Tick
        {
            add { metronome.Elapsed += value; }
            remove { metronome.Elapsed -= value; }
        }

        /// <summary>
        ///   Synchronizing object for thread safety.
        /// </summary>
        /// 
        public ISynchronizeInvoke SynchronizingObject
        {
            get { return timeUp.SynchronizingObject; }
            set
            {
                timeUp.SynchronizingObject = value;
                metronome.SynchronizingObject = value;
            }
        }

        /// <summary>
        ///   Constructs a new Metronome.
        /// </summary>
        /// 
        public Metronome()
        {
            timeUp = new Timer();
            metronome = new Timer();
            timeSpan = new List<TimeSpan>();

            // set timeup as 2 seconds
            timeUp.Interval = 2000;
            timeUp.Elapsed += timeUp_Elapsed;
        }

        /// <summary>
        ///   Taps the metronome (for tempo detection)
        /// </summary>
        /// 
        public void Tap()
        {
            metronome.Stop();

            DateTime now = DateTime.Now;

            if (Detecting)
            {
                timeUp.Stop();
                TimeSpan span = now - lastTick;
                timeSpan.Add(span);
            }
            lastTick = now;
            timeUp.Start();
        }

        /// <summary>
        ///   Starts the metronome.
        /// </summary>
        /// 
        public void Start()
        {
            metronome.Start();
        }

        /// <summary>
        ///   Stops the metronome.
        /// </summary>
        /// 
        public void Stop()
        {
            metronome.Stop();
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// 
        public void Dispose()
        {
            //GC.SuppressFinalize(this);

            timeUp.Dispose();
            metronome.Dispose();
        }

        private void timeUp_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeUp.Stop();

            this.taps = timeSpan.Count;

            if (taps <= 1)
            {
                timeSpan.Clear();
                return;
            }

            double mean = 0;
            for (int i = 0; i < taps; i++)
                mean += timeSpan[0].Milliseconds;
            mean = mean / taps;

            timeSpan.Clear();
            this.metronome.Interval = mean;

            if (TempoDetected != null)
                TempoDetected(this, EventArgs.Empty);
        }

    }
}
