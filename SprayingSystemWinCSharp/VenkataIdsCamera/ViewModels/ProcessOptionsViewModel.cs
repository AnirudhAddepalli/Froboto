﻿using SprayingSystem.Models;
using System;

namespace SprayingSystem.ViewModels
{
    public class ProcessOptionsViewModel : ViewModelBase
    {
        private bool _blot;
        private bool _BackBlot = true;
        private bool _FrontBlot = false;
        private bool _spray;
        private bool _recordSpray;
        private bool _rpiRecordSpray;
        private bool _sonicateTweezers;


        public ProcessOptionsViewModel(UserOptions userOptions, Action<string> log)
        {
        }

        public bool RecordSpray
        {
            get { return _recordSpray; }
            set
            {
                _recordSpray = value;
                OnPropertyChanged();
            }
        }

        public bool RpiRecordSpray
        {
            get { return _rpiRecordSpray; }
            set
            {
                _rpiRecordSpray = value;
                OnPropertyChanged();
            }
        }

        public bool Spray
        {
            get { return _spray; }
            set
            {
                _spray = value;
                OnPropertyChanged();
            }
        }

        public bool Blot
        {
            get { return _blot;}
            set
            {
                _blot = value;
                OnPropertyChanged();
            }
        }

        public bool Blot_BackBlot
        {
            get { return _BackBlot; }
            set
            {
                _BackBlot = value;
                OnPropertyChanged();
            }
        }

        public bool Blot_FrontBlot
        {
            get { return _FrontBlot; }
            set
            {
                _FrontBlot = value;
                OnPropertyChanged();
            }
        }

        public bool SonicateTweezers
        {
            get { return _sonicateTweezers; }
            set
            {
                _sonicateTweezers = value;
                OnPropertyChanged();
            }
        }
    }
}
